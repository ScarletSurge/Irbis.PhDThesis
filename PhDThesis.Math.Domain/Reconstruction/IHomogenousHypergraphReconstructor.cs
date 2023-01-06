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
    /// <returns></returns>
    HomogenousHypergraph Restore(
        T from);
    
}