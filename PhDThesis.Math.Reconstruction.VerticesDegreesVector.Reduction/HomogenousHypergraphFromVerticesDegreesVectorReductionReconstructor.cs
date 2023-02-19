using PhDThesis.Domain.Helpers.Guarding;
using PhDThesis.Math.Domain;
using PhDThesis.Math.Domain.Reconstruction;

namespace PhDThesis.Math.Reconstruction.Reduction;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraphFromVerticesDegreesVectorReductionReconstructor
    : HomogenousHypergraphReconstructorBase<VerticesDegreesVector>
{
    
    /// <inheritdoc cref="HomogenousHypergraphReconstructorBase{T}.RestoreInner" />
    protected override HomogenousHypergraph RestoreInner(
        VerticesDegreesVector from,
        int simplicesDimension)
    {
        Guard.ThrowIfLowerThan(simplicesDimension, 2);
        
        var restoredHomogenousHypergraph = new HomogenousHypergraph(from.VerticesCount, simplicesDimension);
        
        return restoredHomogenousHypergraph;
    }
    
    /// <inheritdoc cref="HomogenousHypergraphReconstructorBase{T}.ThrowIfInvalidInputPrototype" />
    protected override void ThrowIfInvalidInputPrototype(
        VerticesDegreesVector from,
        int simplicesDimension)
    {
        throw new NotImplementedException();
    }
    
}