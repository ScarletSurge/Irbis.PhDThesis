using Irbis.PhDThesis.Domain.Extensions;
using Irbis.PhDThesis.Domain.Helpers.Guarding;

namespace Irbis.PhDThesis.Math.Domain.Reconstruction;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class HomogenousHypergraphRestorerBase<T>:
    IHomogenousHypergraphRestorer<T>
{
    
    #region Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <returns></returns>
    protected abstract IEnumerable<HomogenousHypergraph?> RestoreAllInner(
        T from,
        int simplicesDimension);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="restorationSerialNumber"></param>
    /// <returns></returns>
    private HomogenousHypergraph? RestoreInner(
        T from,
        int simplicesDimension,
        int restorationSerialNumber)
    {
        var allRestorations = RestoreAllInner(from, simplicesDimension);
        var takenRestorations = allRestorations.Take(restorationSerialNumber).ToArray();

        Guardant.Instance
            .ThrowIfGreaterThanOrEqualTo(restorationSerialNumber, takenRestorations.Length);

        return takenRestorations.Last();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract IAsyncEnumerable<HomogenousHypergraph?> RestoreAllInnerAsync(
        T from,
        int simplicesDimension,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="restorationSerialNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<HomogenousHypergraph?> RestoreInnerAsync(
        T from,
        int simplicesDimension,
        int restorationSerialNumber,
        CancellationToken cancellationToken = default)
    {
        var allRestorations = RestoreAllInnerAsync(from, simplicesDimension, cancellationToken);
        var leftRestorations = await allRestorations.SkipAsync(restorationSerialNumber, cancellationToken).ToArrayAsync(cancellationToken);
        
        Guardant.Instance
            .ThrowIfEqual(0, leftRestorations.Length);

        return leftRestorations.First();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    protected abstract HomogenousHypergraphRestorerBase<T> ThrowIfInvalidInputPrototype(
        T from,
        int simplicesDimension);
    
    #endregion
    
    #region Irbis.PhDThesis.Math.Domain.Reconstruction.IHomogenousHypergraphRestorer<in T> implementation
    
    /// <inheritdoc cref="IHomogenousHypergraphRestorer{T}.RestoreAll" />
    public IEnumerable<HomogenousHypergraph?> RestoreAll(
        T from,
        int simplicesDimension)
    {
        return ThrowIfInvalidInputPrototype(from, simplicesDimension)
            .RestoreAllInner(from, simplicesDimension);
    }

    /// <inheritdoc cref="IHomogenousHypergraphRestorer{T}.Restore" />
    public HomogenousHypergraph? Restore(
        T from,
        int simplicesDimension,
        int restorationSerialNumber)
    {
        Guardant.Instance
            .ThrowIfLowerThan(restorationSerialNumber, 0);
        
        return ThrowIfInvalidInputPrototype(from, simplicesDimension)
            .RestoreInner(from, simplicesDimension, restorationSerialNumber);
    }
    
    /// <inheritdoc cref="IHomogenousHypergraphRestorer{T}.RestoreAllAsync" />
    public IAsyncEnumerable<HomogenousHypergraph?> RestoreAllAsync(
        T from,
        int simplicesDimension,
        CancellationToken cancellationToken = default)
    {
        return ThrowIfInvalidInputPrototype(from, simplicesDimension)
            .RestoreAllInnerAsync(from, simplicesDimension, cancellationToken);
    }
    
    /// <inheritdoc cref="IHomogenousHypergraphRestorer{T}.RestoreAsync" />
    public Task<HomogenousHypergraph?> RestoreAsync(
        T from,
        int simplicesDimension,
        int restorationSerialNumber,
        CancellationToken cancellationToken = default)
    {
        Guardant.Instance
            .ThrowIfLowerThan(restorationSerialNumber, 0);
        
        return ThrowIfInvalidInputPrototype(from, simplicesDimension)
            .RestoreInnerAsync(from, simplicesDimension, restorationSerialNumber, cancellationToken);
    }
    
    #endregion

}