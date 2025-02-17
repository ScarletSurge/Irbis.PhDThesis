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
    
    #region [Obsolete] Recursive restoration
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="simplicesMaxCount"></param>
    /// <param name="verticesDegreesSum"></param>
    /// <param name="addedSimplices"></param>
    /// <returns></returns>
    private IEnumerable<HomogenousHypergraph?> RestoreAllInnerRecursive(
        Domain.VerticesDegreesVector from,
        int simplicesDimension,
        BigInteger simplicesMaxCount,
        uint verticesDegreesSum,
        ISet<HyperEdge> addedSimplices)
    {
        if (verticesDegreesSum == 0)
        {
            yield return new HomogenousHypergraph(from.VerticesCount, simplicesDimension, addedSimplices.ToArray());
            yield break;
        }

        var lastAddedSimplex = addedSimplices.LastOrDefault();
        var lastAddedSimplexIndex = lastAddedSimplex is null
            ? -1
            : HomogenousHypergraph.SimplexToBitIndex(lastAddedSimplex, simplicesDimension, from.VerticesCount, simplicesMaxCount);

        for (var nextSimplexToAddIndex = lastAddedSimplexIndex + 1; nextSimplexToAddIndex < simplicesMaxCount; nextSimplexToAddIndex++)
        {
            var simplexToAdd = HomogenousHypergraph.BitIndexToSimplex(nextSimplexToAddIndex, simplicesDimension, from.VerticesCount, simplicesMaxCount);
            if (!from.TryRemoveSimplex(simplexToAdd))
            {
                continue;
            }

            addedSimplices.Add(simplexToAdd);

            var recursiveRestorations = RestoreAllInnerRecursive(from, simplicesDimension, simplicesMaxCount, (uint)(verticesDegreesSum - simplicesDimension), addedSimplices);

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
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="simplicesMaxCount"></param>
    /// <param name="verticesDegreesSum"></param>
    /// <param name="addedSimplices"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async IAsyncEnumerable<HomogenousHypergraph?> RestoreAllInnerRecursiveAsync(
        Domain.VerticesDegreesVector from,
        int simplicesDimension,
        BigInteger simplicesMaxCount,
        uint verticesDegreesSum,
        ISet<HyperEdge> addedSimplices,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (verticesDegreesSum == 0)
        {
            yield return new HomogenousHypergraph(from.VerticesCount, simplicesDimension, addedSimplices.ToArray());
            yield break;
        }

        var lastAddedSimplex = addedSimplices.LastOrDefault();
        var lastAddedSimplexIndex = lastAddedSimplex is null
            ? -1
            : HomogenousHypergraph.SimplexToBitIndex(lastAddedSimplex, simplicesDimension, from.VerticesCount, simplicesMaxCount);

        for (var nextSimplexToAddIndex = lastAddedSimplexIndex + 1; nextSimplexToAddIndex < simplicesMaxCount; nextSimplexToAddIndex++)
        {
            var simplexToAdd = HomogenousHypergraph.BitIndexToSimplex(nextSimplexToAddIndex, simplicesDimension, from.VerticesCount, simplicesMaxCount);
            if (!from.TryRemoveSimplex(simplexToAdd))
            {
                continue;
            }

            addedSimplices.Add(simplexToAdd);
            
            await foreach (var recursiveRestoration in RestoreAllInnerRecursiveAsync(from, simplicesDimension, simplicesMaxCount, (uint)(verticesDegreesSum - simplicesDimension), addedSimplices, cancellationToken))
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
        return RestoreAllInnerRecursive((Domain.VerticesDegreesVector)from.Clone(), simplicesDimension, BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension), (uint)from.Sum(x => x), new HashSet<HyperEdge>());
    }
    
    /// <inheritdoc cref="HomogenousHypergraphRestorerBase{T}.RestoreAsync" />
    protected override IAsyncEnumerable<HomogenousHypergraph?> RestoreAllInnerAsync(
        Domain.VerticesDegreesVector from,
        int simplicesDimension,
        CancellationToken cancellationToken = default)
    {
        return RestoreAllInnerRecursiveAsync((Domain.VerticesDegreesVector)from.Clone(), simplicesDimension, BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension), (uint)from.Sum(x => x), new HashSet<HyperEdge>(), cancellationToken);
    }
    
    #endregion
    
}