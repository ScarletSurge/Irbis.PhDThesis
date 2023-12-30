using System.Numerics;

using PhDThesis.Domain.Helpers.Guarding;

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
        Guardant.Instance
            .ThrowIfLowerThan(value, BigInteger.Zero);

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
        Guardant.Instance
            .ThrowIfLowerThan(n, BigInteger.Zero);

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

        var partialResult = BigInteger.One;
        for (BigInteger i = n - k + 1; i <= n; i++)
        {
            partialResult *= i;
        }
        
        return partialResult / k.Factorial();
    }
    
}