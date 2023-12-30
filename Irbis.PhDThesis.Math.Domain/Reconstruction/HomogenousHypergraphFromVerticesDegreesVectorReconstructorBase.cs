using Irbis.PhDThesis.Domain.Extensions;
using Irbis.PhDThesis.Domain.Helpers.Guarding;

namespace Irbis.PhDThesis.Math.Domain.Reconstruction;

/// <summary>
/// 
/// </summary>
public abstract class HomogenousHypergraphFromVerticesDegreesVectorReconstructorBase:
    HomogenousHypergraphReconstructorBase<VerticesDegreesVector>
{
    
    #region PhDThesis.Math.Domain.Reconstruction.HomogenousHypergraphReconstructorBase<VerticesDegreesVector> overrides

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    protected sealed override HomogenousHypergraphReconstructorBase<VerticesDegreesVector> ThrowIfInvalidInputPrototype(
        VerticesDegreesVector from,
        int simplicesDimension)
    {
        var combinationsCount = BigIntegerExtensions.CombinationsCount(from.VerticesCount - 1, simplicesDimension - 1);
        
        Guardant.Instance
            .ThrowIfNullOrEmpty(from)
            .ThrowIfLowerThan(simplicesDimension, 2)
            .ThrowIfGreaterThan(simplicesDimension, from.VerticesCount)
            .ThrowIfAny(from, vertexDegree => vertexDegree > combinationsCount, "Can't restore homogenous hypergraph: one or more vertices degree is too big.")
            .ThrowIf(from, innerFrom => innerFrom.Sum(vertexDegree => vertexDegree) % simplicesDimension != 0, "Can't restore homogenous hypergraph: sum of vertices degrees vector components must be divisible by simplices dimension.");

        return this;
    }
    
    #endregion
    
}