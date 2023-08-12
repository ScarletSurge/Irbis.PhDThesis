using System.Numerics;
using PhDThesis.Domain.Helpers.Guarding;

namespace PhDThesis.Math.Domain.Fraction;

/// <summary>
/// 
/// </summary>
public static class FractionExtensions
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fraction"></param>
    /// <returns></returns>
    public static IEnumerable<BigInteger> ToContinuedFraction(
        this Fraction fraction)
    {
        yield return fraction.Numerator / fraction.Denominator;

        var nextDenominator = fraction.Numerator % fraction.Denominator;
        if (nextDenominator.IsZero)
        {
            yield break;
        }

        foreach (var component in ToContinuedFraction(new Fraction(fraction.Denominator, nextDenominator)))
        {
            yield return component;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="continuedFraction"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static Fraction ToFraction(
        this IEnumerable<BigInteger> continuedFraction)
    {
        Guardant.Instance
            .ThrowIfNullOrEmpty(continuedFraction);

        return FromContinuedFractionInner(continuedFraction);

        static Fraction FromContinuedFractionInner(
            IEnumerable<BigInteger> continuedFraction)
        {
            throw new NotImplementedException();
        }
    }
    
}