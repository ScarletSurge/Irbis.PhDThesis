using PhDThesis.Math.Domain;
using PhDThesis.Math.Domain.Reconstruction;

namespace PhDThesis.Math.Reconstruction.Greedy;

/// <summary>
/// 
/// </summary>
public sealed class HomogenousHypergraphFromVerticesDegreesVectorGreedyReconstructor
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