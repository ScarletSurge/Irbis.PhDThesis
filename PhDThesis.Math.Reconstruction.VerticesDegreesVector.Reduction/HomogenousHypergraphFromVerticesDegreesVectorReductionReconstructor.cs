using PhDThesis.Math.Domain;
using PhDThesis.Math.Domain.Reconstruction;

namespace PhDThesis.Math.Reconstruction.Reduction;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraphFromVerticesDegreesVectorReductionReconstructor:
    HomogenousHypergraphFromVerticesDegreesVectorReconstructorBase
{
    
    #region PhDThesis.Math.Domain.Reconstruction.HomogenousHypergraphReconstructorBase<VerticesDegreesVector> overrides
    
    /// <inheritdoc cref="HomogenousHypergraphReconstructorBase{T}.RestoreInner" />
    protected override HomogenousHypergraph RestoreInner(
        VerticesDegreesVector from,
        int simplicesDimension)
    {
        var restoredHomogenousHypergraph = new HomogenousHypergraph(from.VerticesCount, simplicesDimension);
        
        return restoredHomogenousHypergraph;
    }
    
    /// <inheritdoc cref="HomogenousHypergraphReconstructorBase{T}.RestoreInnerAsync" />
    protected override Task<HomogenousHypergraph> RestoreInnerAsync(
        VerticesDegreesVector from,
        int simplicesDimension,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    #endregion
    
}