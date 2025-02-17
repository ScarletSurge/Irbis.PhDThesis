using Irbis.PhDThesis.Domain.Extensions;
using Irbis.PhDThesis.Math.Domain;
using Irbis.PhDThesis.Math.Domain.Reconstruction;

namespace Irbis.PhDThesis.Math.Reconstruction.VerticesDegreesVector.Reduction;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraphFromVerticesDegreesVectorReductionRestorer:
    HomogenousHypergraphFromVerticesDegreesVectorRestorerBase
{
    
    private IEnumerable<HomogenousHypergraph?> RestoreAllInnerRecursive(
        Domain.VerticesDegreesVector from,
        int simplicesDimension,
        uint verticesDegreesSum,
        HashSet<HyperEdge> addedSimplices)
    {
        var simplicesMaxCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension);
        
        if (verticesDegreesSum == 0)
        {
            yield return new HomogenousHypergraph(from.VerticesCount, simplicesDimension, addedSimplices.ToArray());
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

            var recursiveRestorations = RestoreAllInnerRecursive(from, simplicesDimension, (uint)(verticesDegreesSum - simplicesDimension), addedSimplices);

            foreach (var recursiveRestoration in recursiveRestorations ?? Enumerable.Empty<HomogenousHypergraph?>())
            {
                if (recursiveRestoration is not null)
                {
                    yield return recursiveRestoration;
                }
            }

            addedSimplices.Remove(simplexToAdd);
            from.AddSimplex(simplexToAdd);
        }
    }
    
    private async IAsyncEnumerable<HomogenousHypergraph?> RestoreAllInnerRecursiveAsync(
        Domain.VerticesDegreesVector from,
        int simplicesDimension,
        uint verticesDegreesSum,
        ISet<HyperEdge> addedSimplices,
        CancellationToken cancellationToken = default)
    {
        var simplicesMaxCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount, simplicesDimension);

        if (verticesDegreesSum == 0)
        {
            yield return new HomogenousHypergraph(from.VerticesCount, simplicesDimension,
                addedSimplices.ToArray());
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

            var recursiveRestorations = RestoreAllInnerRecursiveAsync(from, simplicesDimension,
                (uint) (verticesDegreesSum - simplicesDimension), addedSimplices, cancellationToken);
            if (recursiveRestorations is not null)
            {
                foreach (var recursiveRestoration in await recursiveRestorations.ToArrayAsync(cancellationToken))
                {
                    yield return recursiveRestoration;
                }
            }

            addedSimplices.Remove(simplexToAdd);
            from.AddSimplex(simplexToAdd);
        }
    }
    
    #region Irbis.PhDThesis.Math.Domain.Reconstruction.HomogenousHypergraphRestorerBase<VerticesDegreesVector> overrides
    
    /// <inheritdoc cref="HomogenousHypergraphRestorerBase{T}.RestoreInner" />
    protected override IEnumerable<HomogenousHypergraph?> RestoreAllInner(
        Domain.VerticesDegreesVector from,
        int simplicesDimension)
    {
        return RestoreAllInnerRecursive((Domain.VerticesDegreesVector)from.Clone(), simplicesDimension, (uint)from.Sum(x => x), new HashSet<HyperEdge>());
    }
    
    /// <inheritdoc cref="HomogenousHypergraphRestorerBase{T}.RestoreInnerAsync" />
    protected override IAsyncEnumerable<HomogenousHypergraph?> RestoreAllInnerAsync(
        Domain.VerticesDegreesVector from,
        int simplicesDimension,
        CancellationToken cancellationToken = default)
    {
        return RestoreAllInnerRecursiveAsync((Domain.VerticesDegreesVector)from.Clone(), simplicesDimension, (uint)from.Sum(x => x), new HashSet<HyperEdge>(), cancellationToken);
    }
    
    #endregion
    
}