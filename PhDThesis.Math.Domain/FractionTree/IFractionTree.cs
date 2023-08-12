namespace PhDThesis.Math.Domain.FractionTree;

/// <summary>
/// 
/// </summary>
public interface IFractionTree
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public Fraction.Fraction FindFractionByPath(
        BitArray path);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fraction"></param>
    /// <returns></returns>
    public BitArray FindPathByFraction(
        Fraction.Fraction fraction);
}