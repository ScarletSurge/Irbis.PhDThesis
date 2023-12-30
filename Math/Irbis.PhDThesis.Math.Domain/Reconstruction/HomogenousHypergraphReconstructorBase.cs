namespace Irbis.PhDThesis.Math.Domain.Reconstruction;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class HomogenousHypergraphReconstructorBase<T>:
    IHomogenousHypergraphReconstructor<T>
{

    /// <inheritdoc cref="IHomogenousHypergraphReconstructor{T}.Restore" />
    public HomogenousHypergraph? Restore(
        T from,
        int simplicesDimension)
    {
        return ThrowIfInvalidInputPrototype(from, simplicesDimension)
            .RestoreInner(from, simplicesDimension);
    }
    
    /// <inheritdoc cref="IHomogenousHypergraphReconstructor{T}.RestoreAsync" />
    public Task<HomogenousHypergraph?> RestoreAsync(
        T from,
        int simplicesDimension,
        CancellationToken cancellationToken = default)
    {
        return ThrowIfInvalidInputPrototype(from, simplicesDimension)
            .RestoreInnerAsync(from, simplicesDimension, cancellationToken);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <returns></returns>
    protected abstract HomogenousHypergraph? RestoreInner(
        T from,
        int simplicesDimension);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task<HomogenousHypergraph?> RestoreInnerAsync(
        T from,
        int simplicesDimension,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    protected abstract HomogenousHypergraphReconstructorBase<T> ThrowIfInvalidInputPrototype(
        T from,
        int simplicesDimension);

}