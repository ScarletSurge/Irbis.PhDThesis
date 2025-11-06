using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using Irbis.PhDThesis.Domain.Extensions;
using Irbis.PhDThesis.Math.Domain;
using Irbis.PhDThesis.Math.Domain.Reconstruction;

namespace Irbis.PhDThesis.Math.Reconstruction.VerticesDegreesVector.Greedy;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraphFromVerticesDegreesVectorGreedyRestorer:
    HomogenousHypergraphFromVerticesDegreesVectorRestorerBase
{
    
    #region Fields
    
    /// <summary>
    /// 
    /// </summary>
    private static readonly Dictionary<(int SimplicesDimension, int VerticesCount), IImmutableDictionary<int, HyperEdge>> _mappings;
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    static HomogenousHypergraphFromVerticesDegreesVectorGreedyRestorer()
    {
        _mappings = new Dictionary<(int SimplicesDimension, int VerticesCount), IImmutableDictionary<int, HyperEdge>>();
    }
    
    #endregion

    #region [Obsolete] Recursive restoration
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="indexToHyperEdgeMappings"></param>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="simplicesMaxCount"></param>
    /// <param name="verticesDegreesSum"></param>
    /// <param name="addedSimplices"></param>
    /// <param name="lastAddedSimplexIndex"></param>
    /// <returns></returns>
    private IEnumerable<HomogenousHypergraph?> RestoreAllInnerRecursive(
        IImmutableDictionary<int, HyperEdge> indexToHyperEdgeMappings,
        Domain.VerticesDegreesVector from,
        int simplicesDimension,
        BigInteger simplicesMaxCount,
        uint verticesDegreesSum,
        ISet<HyperEdge> addedSimplices,
        int lastAddedSimplexIndex)
    {
        if (verticesDegreesSum == 0)
        {
            yield return new HomogenousHypergraph(from.VerticesCount, simplicesDimension, addedSimplices.ToArray());
            yield break;
        }

        for (var nextSimplexToAddIndex = lastAddedSimplexIndex + 1; nextSimplexToAddIndex < simplicesMaxCount; nextSimplexToAddIndex++)
        {
            var simplexToAdd = indexToHyperEdgeMappings[nextSimplexToAddIndex];
            if (!from.TryRemoveSimplex(simplexToAdd))
            {
                continue;
            }

            addedSimplices.Add(simplexToAdd);

            var recursiveRestorations = RestoreAllInnerRecursive(indexToHyperEdgeMappings, from, simplicesDimension, simplicesMaxCount, (uint)(verticesDegreesSum - simplicesDimension), addedSimplices, nextSimplexToAddIndex);

            foreach (var recursiveRestoration in recursiveRestorations)
            {
                yield return recursiveRestoration;
            }
            
            addedSimplices.Remove(simplexToAdd);
            from.AddSimplex(simplexToAdd);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="indexToHyperEdgeMappings"></param>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="simplicesMaxCount"></param>
    /// <param name="verticesDegreesSum"></param>
    /// <param name="addedSimplices"></param>
    /// <param name="lastAddedSimplexIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async IAsyncEnumerable<HomogenousHypergraph?> RestoreAllInnerRecursiveAsync(
        IImmutableDictionary<int, HyperEdge> indexToHyperEdgeMappings,
        Domain.VerticesDegreesVector from,
        int simplicesDimension,
        BigInteger simplicesMaxCount,
        uint verticesDegreesSum,
        ISet<HyperEdge> addedSimplices,
        int lastAddedSimplexIndex,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (verticesDegreesSum == 0)
        {
            yield return new HomogenousHypergraph(from.VerticesCount, simplicesDimension, addedSimplices.ToArray());
            yield break;
        }

        for (var nextSimplexToAddIndex = lastAddedSimplexIndex + 1; nextSimplexToAddIndex < simplicesMaxCount; nextSimplexToAddIndex++)
        {
            var simplexToAdd = indexToHyperEdgeMappings[nextSimplexToAddIndex];
            if (!from.TryRemoveSimplex(simplexToAdd))
            {
                continue;
            }

            addedSimplices.Add(simplexToAdd);
            
            await foreach (var recursiveRestoration in RestoreAllInnerRecursiveAsync(indexToHyperEdgeMappings, from, simplicesDimension, simplicesMaxCount, (uint)(verticesDegreesSum - simplicesDimension), addedSimplices, nextSimplexToAddIndex, cancellationToken))
            {
                yield return recursiveRestoration;
            }
            
            addedSimplices.Remove(simplexToAdd);
            from.AddSimplex(simplexToAdd);
        }
    }
    
    #endregion
    
    #region [Obsolete] Iterative restoration
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <returns></returns>
    [Obsolete]
    private IEnumerable<HomogenousHypergraph?> RestoreAllInnerIterative(
        Domain.VerticesDegreesVector from,
        int simplicesDimension)
    {
        var simplicesMaxCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension);
        var simplicesTargetCount = from.Sum(x => x) / simplicesDimension;
        var addedSimplicesSequence = new Stack<(int, HyperEdge)>();
        var addedSimplicesIndices = new HashSet<int>();
        var currentSimplexToAddIndex = 0;

        while (addedSimplicesSequence.Count != simplicesTargetCount)
        {
            if (++currentSimplexToAddIndex == simplicesMaxCount)
            {
                if (addedSimplicesSequence.Count == 0)
                {
                    yield return null;
                }
                
                addedSimplicesIndices.Remove(currentSimplexToAddIndex = addedSimplicesSequence.Pop().Item1 + 1);
            }
            
            if (!addedSimplicesIndices.Contains(currentSimplexToAddIndex))
            {
                var simplexToPush = HomogenousHypergraph.BitIndexToSimplex(currentSimplexToAddIndex, simplicesDimension, from.VerticesCount, simplicesMaxCount);
                if (from.TryRemoveSimplex(simplexToPush))
                {
                    addedSimplicesSequence.Push((currentSimplexToAddIndex, simplexToPush));
                    addedSimplicesIndices.Add(currentSimplexToAddIndex);
                }
            }
        }

        yield return new HomogenousHypergraph(from.VerticesCount, simplicesDimension, addedSimplicesSequence.Select(x => x.Item2).ToArray());
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Obsolete]
    private async IAsyncEnumerable<HomogenousHypergraph?> RestoreAllInnerIterativeAsync(
        Domain.VerticesDegreesVector from,
        int simplicesDimension,
        CancellationToken cancellationToken = default)
    {
        var simplicesMaxCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension);
        var simplicesTargetCount = from.Sum(x => x) / simplicesDimension;
        var addedSimplicesSequence = new Stack<(int, HyperEdge)>();
        var addedSimplicesIndices = new HashSet<int>();
        var currentSimplexToAddIndex = -1;

        while (addedSimplicesSequence.Count != simplicesTargetCount)
        {
            cancellationToken.ThrowIfCancellationRequested();

            while (++currentSimplexToAddIndex == simplicesMaxCount)
            {
                if (addedSimplicesSequence.Count == 0)
                {
                    yield return null;
                }

                var poppedSimplex = addedSimplicesSequence.Pop();
                addedSimplicesIndices.Remove(currentSimplexToAddIndex = poppedSimplex.Item1);
                from.AddSimplex(poppedSimplex.Item2);
            }

            if (addedSimplicesIndices.Contains(currentSimplexToAddIndex))
            {
                continue;
            }
                
            var simplexToPush = HomogenousHypergraph.BitIndexToSimplex(currentSimplexToAddIndex, simplicesDimension, from.VerticesCount, simplicesMaxCount);
            if (!from.TryRemoveSimplex(simplexToPush))
            {
                continue;
            }
                
            addedSimplicesSequence.Push((currentSimplexToAddIndex, simplexToPush));
            addedSimplicesIndices.Add(currentSimplexToAddIndex);
        }
    }
    
    #endregion
    
    #region Irbis.PhDThesis.Math.Domain.Reconstruction.HomogenousHypergraphRestorerBase<VerticesDegreesVector> overrides
    
    /// <inheritdoc cref="HomogenousHypergraphRestorerBase{T}.Restore" />
    protected override IEnumerable<HomogenousHypergraph?> RestoreAllInner(
        Domain.VerticesDegreesVector from,
        int simplicesDimension)
    {
        var simplicesMaxCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension);

        if (!_mappings.TryGetValue((simplicesDimension, from.VerticesCount), out var indexToHyperEdgeMappings))
        {
            var indexToHyperEdgeMappingsToAdd = new Dictionary<int, HyperEdge>();
            for (var i = 0; i < HomogenousHypergraph.GetSimplicesMaxCount(from.VerticesCount, simplicesDimension); ++i)
            {
                var hypedEdgeByIndex = HomogenousHypergraph.BitIndexToSimplex(i, simplicesDimension, from.VerticesCount, simplicesMaxCount);
            
                indexToHyperEdgeMappingsToAdd.Add(i, hypedEdgeByIndex);
            }
            
            _mappings.Add((simplicesDimension, from.VerticesCount), indexToHyperEdgeMappings = indexToHyperEdgeMappingsToAdd.ToImmutableDictionary());
        }
        
        return RestoreAllInnerRecursive(indexToHyperEdgeMappings, (Domain.VerticesDegreesVector)from.Clone(), simplicesDimension, simplicesMaxCount, (uint)from.Sum(x => x), new HashSet<HyperEdge>(), -1);
    }
    
    /// <inheritdoc cref="HomogenousHypergraphRestorerBase{T}.RestoreAsync" />
    protected override IAsyncEnumerable<HomogenousHypergraph?> RestoreAllInnerAsync(
        Domain.VerticesDegreesVector from,
        int simplicesDimension,
        CancellationToken cancellationToken = default)
    {
        var simplicesMaxCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension);
        
        if (!_mappings.TryGetValue((simplicesDimension, from.VerticesCount), out var indexToHyperEdgeMappings))
        {
            var indexToHyperEdgeMappingsToAdd = new Dictionary<int, HyperEdge>();
            for (var i = 0; i < HomogenousHypergraph.GetSimplicesMaxCount(from.VerticesCount, simplicesDimension); ++i)
            {
                var hypedEdgeByIndex = HomogenousHypergraph.BitIndexToSimplex(i, simplicesDimension, from.VerticesCount, simplicesMaxCount);
            
                indexToHyperEdgeMappingsToAdd.Add(i, hypedEdgeByIndex);
            }
            
            _mappings.Add((simplicesDimension, from.VerticesCount), indexToHyperEdgeMappings = indexToHyperEdgeMappingsToAdd.ToImmutableDictionary());
        }
        
        return RestoreAllInnerRecursiveAsync(indexToHyperEdgeMappings, (Domain.VerticesDegreesVector)from.Clone(), simplicesDimension, simplicesMaxCount, (uint)from.Sum(x => x), new HashSet<HyperEdge>(), -1, cancellationToken);
    }
    
    #endregion
    
}