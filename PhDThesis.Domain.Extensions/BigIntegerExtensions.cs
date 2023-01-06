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

        for (var i = (BigInteger)2; i <= value; i++)
        {
            factorial *= i;
        }

        return factorial;
    }

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