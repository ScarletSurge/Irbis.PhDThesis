using PhDThesis.Domain.Helpers.Guarding;

namespace PhDThesis.Math.Domain.Reconstruction;

public abstract class HomogenousHypergraphFromVerticesDegreesVectorReconstructorBase:
    HomogenousHypergraphReconstructorBase<VerticesDegreesVector>
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    protected sealed override HomogenousHypergraphReconstructorBase<VerticesDegreesVector> ThrowIfInvalidInputPrototype(
        VerticesDegreesVector from,
        int simplicesDimension)
    {
        Guardant.Instance
            .ThrowIfNullOrEmpty(from)
            .ThrowIfLowerThan(simplicesDimension, 2);

        if (from.Sum(x => x) % simplicesDimension != 0)
        {
            throw new ArgumentException("Can't restore homogenous hypergraph: invalid vertices degrees vector", nameof(from));
        }

        return this;
    }
    
}