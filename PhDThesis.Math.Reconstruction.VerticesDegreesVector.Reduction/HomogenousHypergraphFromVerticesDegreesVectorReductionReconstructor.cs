using PhDThesis.Math.Domain;
using PhDThesis.Math.Domain.Reconstruction;

namespace PhDThesis.Math.Reconstruction.Reduction;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraphFromVerticesDegreesVectorReductionReconstructor
    : IHomogenousHypergraphReconstructor<VerticesDegreesVector>
{
    
    /// <inheritdoc cref="IHomogenousHypergraphReconstructor{T}.Restore" />
    public HomogenousHypergraph Restore(
        VerticesDegreesVector from,
        int simplicesDimension)
    {
        throw new NotImplementedException();
    }
    
}