using Irbis.PhDThesis.Domain.Extensions;
using Irbis.PhDThesis.Math.Domain;
using Irbis.PhDThesis.Math.Domain.Reconstruction;

namespace Irbis.PhDThesis.Math.Reconstruction.Greedy;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraphFromVerticesDegreesVectorGreedyReconstructor:
    HomogenousHypergraphFromVerticesDegreesVectorReconstructorBase
{
    
    #region Recursive restoration
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="verticesDegreesSum"></param>
    /// <param name="addedSimplices"></param>
    /// <returns></returns>
    [Obsolete]
    private HomogenousHypergraph? RestoreInnerRecursive(
        VerticesDegreesVector from,
        int simplicesDimension,
        uint verticesDegreesSum,
        HashSet<HyperEdge> addedSimplices)
    {
        var simplicesMaxCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension);
        
        if (verticesDegreesSum == 0)
        {
            return new HomogenousHypergraph(from.VerticesCount, simplicesDimension, addedSimplices.ToArray());
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

            var restored = RestoreInnerRecursive(from, simplicesDimension, (uint)(verticesDegreesSum - simplicesDimension), addedSimplices);
            if (restored is not null)
            {
                return restored;
            }
            
            addedSimplices.Remove(simplexToAdd);
            from.AddSimplex(simplexToAdd);
        }

        return null;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="verticesDegreesSum"></param>
    /// <param name="addedSimplices"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Obsolete]
    private async Task<HomogenousHypergraph?> RestoreInnerRecursiveAsync(
        VerticesDegreesVector from,
        int simplicesDimension,
        uint verticesDegreesSum,
        HashSet<HyperEdge> addedSimplices,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var simplicesMaxCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension);
        
        if (verticesDegreesSum == 0)
        {
            return new HomogenousHypergraph(from.VerticesCount, simplicesDimension, addedSimplices.ToArray());
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

            var restored = await RestoreInnerRecursiveAsync(from, simplicesDimension, (uint)(verticesDegreesSum - simplicesDimension), addedSimplices, cancellationToken);
            if (restored is not null)
            {
                return restored;
            }
            
            addedSimplices.Remove(simplexToAdd);
            from.AddSimplex(simplexToAdd);
        }

        return null;
    }
    
    #endregion
    
    #region Iterative restoration
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <returns></returns>
    private HomogenousHypergraph? RestoreInnerIterative(
        VerticesDegreesVector from,
        int simplicesDimension)
    {
        var simplicesMaxCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension);
        var simplicesTargetCount = from.Sum(x => x) / simplicesDimension;
        var addedSimplicesSequence = new Stack<(int, HyperEdge)>();
        var addedSimplicesHashSet = new HashSet<int>();
        var currentSimplexToAddIndex = 0;

        while (addedSimplicesSequence.Count != simplicesTargetCount)
        {
            if (++currentSimplexToAddIndex == simplicesMaxCount)
            {
                if (addedSimplicesSequence.Count == 0)
                {
                    return null;
                }
                
                addedSimplicesHashSet.Remove(currentSimplexToAddIndex = addedSimplicesSequence.Pop().Item1 + 1);
            }
            
            if (!addedSimplicesHashSet.Contains(currentSimplexToAddIndex))
            {
                var simplexToPush = HomogenousHypergraph.BitIndexToSimplex(currentSimplexToAddIndex, simplicesDimension, from.VerticesCount, simplicesMaxCount);
                if (from.TryRemoveSimplex(simplexToPush))
                {
                    addedSimplicesSequence.Push((currentSimplexToAddIndex, simplexToPush));
                    addedSimplicesHashSet.Add(currentSimplexToAddIndex);
                }
            }
        }

        return new HomogenousHypergraph(from.VerticesCount, simplicesDimension, addedSimplicesSequence.Select(x => x.Item2).ToArray());
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private Task<HomogenousHypergraph?> RestoreInnerIterativeAsync(
        VerticesDegreesVector from,
        int simplicesDimension,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var simplicesMaxCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension);
            var simplicesTargetCount = from.Sum(x => x) / simplicesDimension;
            var addedSimplicesSequence = new Stack<(int, HyperEdge)>();
            var addedSimplicesHashSet = new HashSet<int>();
            var currentSimplexToAddIndex = -1;

            while (addedSimplicesSequence.Count != simplicesTargetCount)
            {
                cancellationToken.ThrowIfCancellationRequested();

                while (++currentSimplexToAddIndex == simplicesMaxCount)
                {
                    if (addedSimplicesSequence.Count == 0)
                    {
                        return Task.FromResult<HomogenousHypergraph?>(null);
                    }

                    var poppedSimplex = addedSimplicesSequence.Pop();
                    addedSimplicesHashSet.Remove(currentSimplexToAddIndex = poppedSimplex.Item1);
                    from.AddSimplex(poppedSimplex.Item2);
                }

                if (!addedSimplicesHashSet.Contains(currentSimplexToAddIndex))
                {
                    var simplexToPush = HomogenousHypergraph.BitIndexToSimplex(currentSimplexToAddIndex,
                        simplicesDimension, from.VerticesCount, simplicesMaxCount);
                    if (from.TryRemoveSimplex(simplexToPush))
                    {
                        addedSimplicesSequence.Push((currentSimplexToAddIndex, simplexToPush));
                        addedSimplicesHashSet.Add(currentSimplexToAddIndex);
                    }
                }
            }

            return Task.FromResult<HomogenousHypergraph?>(new HomogenousHypergraph(from.VerticesCount,
                simplicesDimension, addedSimplicesSequence.Select(x => x.Item2).ToArray()));
        }, cancellationToken);
    }
    
    #endregion
    
    #region PhDThesis.Math.Domain.Reconstruction.HomogenousHypergraphReconstructorBase<VerticesDegreesVector> overrides
    
    /// <inheritdoc cref="HomogenousHypergraphReconstructorBase{T}.Restore" />
    protected override HomogenousHypergraph? RestoreInner(
        VerticesDegreesVector from,
        int simplicesDimension)
    {
        return RestoreInnerIterative((VerticesDegreesVector)from.Clone(), simplicesDimension);
    }
    
    /// <inheritdoc cref="HomogenousHypergraphReconstructorBase{T}.RestoreAsync"/>
    protected override Task<HomogenousHypergraph?> RestoreInnerAsync(
        VerticesDegreesVector from,
        int simplicesDimension,
        CancellationToken cancellationToken = default)
    {
        return RestoreInnerIterativeAsync((VerticesDegreesVector)from.Clone(), simplicesDimension, cancellationToken);
    }
    
    #endregion
    
}