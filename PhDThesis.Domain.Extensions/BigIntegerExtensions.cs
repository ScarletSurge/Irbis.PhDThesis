using System.Numerics;

namespace PhDThesis.Domain.Extensions;

/// <summary>
/// 
/// </summary>
public static class BigIntegerExtensions
{
    
    /// <summary>
    /// 
    /// </summary>
    public static BigInteger Two =>
        2;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static BigInteger LeastCommonMultiple(
        BigInteger left,
        BigInteger right)
    {
        return BigInteger.Abs(left * right) / BigInteger.GreatestCommonDivisor(left, right);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static BigInteger Factorial(
        this BigInteger value)
    {
        
        if (value < BigInteger.Zero)
        {
            throw new ArgumentException("value must be GT 0", nameof(value));
        }

        var factorial = BigInteger.One;

        for (var i = Two; i <= value; i++)
        {
            factorial *= i;
        }

        return factorial;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="n"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public static BigInteger CombinationsCount(
        BigInteger n,
        BigInteger k)
    {
        // TODO: speed up
        // TODO: check args values

        if (n == k)
        {
            return BigInteger.One;
        }

        if (k == BigInteger.One)
        {
            return n;
        }

        if (n < k)
        {
            return BigInteger.Zero;
        }

        return n.Factorial() / k.Factorial() / (n - k).Factorial();
    }
    
}