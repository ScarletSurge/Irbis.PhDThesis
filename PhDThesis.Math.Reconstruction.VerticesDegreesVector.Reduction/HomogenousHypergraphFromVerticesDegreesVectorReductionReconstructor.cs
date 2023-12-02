using PhDThesis.Domain.Extensions;
using PhDThesis.Math.Domain;
using PhDThesis.Math.Domain.Reconstruction;

namespace PhDThesis.Math.Reconstruction.Reduction;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraphFromVerticesDegreesVectorReductionReconstructor:
    HomogenousHypergraphFromVerticesDegreesVectorReconstructorBase
{
    
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
    
    private Task<HomogenousHypergraph?> RestoreInnerRecursiveAsync(
        VerticesDegreesVector from,
        int simplicesDimension,
        uint verticesDegreesSum,
        ISet<HyperEdge> addedSimplices,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var simplicesMaxCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension);

            if (verticesDegreesSum == 0)
            {
                return Task.FromResult(new HomogenousHypergraph(from.VerticesCount, simplicesDimension,
                    addedSimplices.ToArray()));
            }
            
            var lastAddedSimplex = addedSimplices.LastOrDefault();
            var lastAddedSimplexIndex = lastAddedSimplex is null
                ? -1
                : HomogenousHypergraph.SimplexToBitIndex(lastAddedSimplex, simplicesDimension, from.VerticesCount,
                    simplicesMaxCount);

            for (var nextSimplexToAddIndex = lastAddedSimplexIndex + 1;
                 nextSimplexToAddIndex < simplicesMaxCount;
                 nextSimplexToAddIndex++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var simplexToAdd = HomogenousHypergraph.BitIndexToSimplex(nextSimplexToAddIndex, simplicesDimension,
                    from.VerticesCount, simplicesMaxCount);
                if (!from.TryRemoveSimplex(simplexToAdd))
                {
                    continue;
                }

                addedSimplices.Add(simplexToAdd);

                var restored = RestoreInnerRecursiveAsync(from, simplicesDimension,
                    (uint) (verticesDegreesSum - simplicesDimension), addedSimplices, cancellationToken).GetAwaiter().GetResult();
                if (restored is not null)
                {
                    return Task.FromResult(restored);
                }

                addedSimplices.Remove(simplexToAdd);
                from.AddSimplex(simplexToAdd);
            }

            return Task<HomogenousHypergraph?>.FromResult(default(HomogenousHypergraph?));
        }, cancellationToken);
    }
    
    #region PhDThesis.Math.Domain.Reconstruction.HomogenousHypergraphReconstructorBase<VerticesDegreesVector> overrides
    
    /// <inheritdoc cref="HomogenousHypergraphReconstructorBase{T}.RestoreInner" />
    protected override HomogenousHypergraph? RestoreInner(
        VerticesDegreesVector from,
        int simplicesDimension)
    {
        return RestoreInnerRecursive((VerticesDegreesVector)from.Clone(), simplicesDimension, (uint)from.Sum(x => x), new HashSet<HyperEdge>());
    }
    
    /// <inheritdoc cref="HomogenousHypergraphReconstructorBase{T}.RestoreInnerAsync" />
    protected override Task<HomogenousHypergraph?> RestoreInnerAsync(
        VerticesDegreesVector from,
        int simplicesDimension,
        CancellationToken cancellationToken = default)
    {
        return RestoreInnerRecursiveAsync((VerticesDegreesVector)from.Clone(), simplicesDimension, (uint)from.Sum(x => x), new HashSet<HyperEdge>(), cancellationToken);
    }
    
    #endregion
    
}