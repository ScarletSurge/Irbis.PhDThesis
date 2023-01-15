namespace PhDThesis.Math.Domain.Reconstruction;

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
    HomogenousHypergraph Restore(
        T from,
        int simplicesDimension);

}