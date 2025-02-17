namespace Irbis.PhDThesis.Math.Domain.Reconstruction;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IHomogenousHypergraphRestorer<in T>
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <returns></returns>
    IEnumerable<HomogenousHypergraph?> RestoreAll(
        T from,
        int simplicesDimension);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="restorationSerialNumber"></param>
    /// <returns></returns>
    HomogenousHypergraph? Restore(
        T from,
        int simplicesDimension,
        int restorationSerialNumber);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    IAsyncEnumerable<HomogenousHypergraph?> RestoreAllAsync(
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
    Task<HomogenousHypergraph?> RestoreAsync(
        T from,
        int simplicesDimension,
        int restorationSerialNumber,
        CancellationToken cancellationToken = default);

}