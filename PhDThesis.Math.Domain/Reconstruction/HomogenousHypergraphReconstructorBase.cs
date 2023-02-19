namespace PhDThesis.Math.Domain.Reconstruction;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class HomogenousHypergraphReconstructorBase<T>
    : IHomogenousHypergraphReconstructor<T>
{

    /// <inheritdoc cref="IHomogenousHypergraphReconstructor{T}.Restore" />
    public HomogenousHypergraph Restore(
        T from,
        int simplicesDimension)
    {
        ThrowIfInvalidInputPrototype(from, simplicesDimension);
        return RestoreInner(from, simplicesDimension);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <returns></returns>
    protected abstract HomogenousHypergraph RestoreInner(
        T from,
        int simplicesDimension);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    protected abstract void ThrowIfInvalidInputPrototype(
        T from,
        int simplicesDimension);

}