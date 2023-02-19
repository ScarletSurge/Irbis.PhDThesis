using PhDThesis.Math.Domain;
using PhDThesis.Math.Domain.Reconstruction;

namespace PhDThesis.Math.Reconstruction.Greedy;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraphFromVerticesDegreesVectorGreedyReconstructor
    : HomogenousHypergraphReconstructorBase<VerticesDegreesVector>
{
    
    /// <inheritdoc cref="HomogenousHypergraphReconstructorBase{T}.Restore" />
    protected override HomogenousHypergraph RestoreInner(
        VerticesDegreesVector from,
        int simplicesDimension)
    {
        throw new NotImplementedException();
    }
    
    /// <inheritdoc cref="HomogenousHypergraphReconstructorBase{T}.ThrowIfInvalidInputPrototype" />
    protected override void ThrowIfInvalidInputPrototype(
        VerticesDegreesVector from,
        int simplicesDimension)
    {
        throw new NotImplementedException();
    }
    
}