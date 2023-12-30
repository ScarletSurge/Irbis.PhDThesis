namespace Irbis.PhDThesis.Math.Domain.Reconstruction;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IHomogenousHypergraphReconstructor<in T>
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <returns></returns>
    HomogenousHypergraph? Restore(
        T from,
        int simplicesDimension);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<HomogenousHypergraph?> RestoreAsync(
        T from,
        int simplicesDimension,
        CancellationToken cancellationToken = default);

}