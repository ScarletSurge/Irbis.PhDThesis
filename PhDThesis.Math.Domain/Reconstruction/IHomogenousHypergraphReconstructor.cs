namespace PhDThesis.Math.Domain.Reconstruction;

/// <summary>
/// 
/// </summary>
public interface IHomogenousHypergraphReconstructor<in T>
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="simplicesDimension"></param>
    /// <returns></returns>
    HomogenousHypergraph Restore(
        T from,
        int simplicesDimension);
    
}