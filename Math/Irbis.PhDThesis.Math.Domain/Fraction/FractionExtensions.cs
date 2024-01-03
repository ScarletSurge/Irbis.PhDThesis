using System.Numerics;

using Irbis.PhDThesis.Domain.Helpers.Guarding;

namespace Irbis.PhDThesis.Math.Domain.Fraction;

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
        var numerator = fraction.Numerator;
        var denominator = fraction.Denominator;
        
        do
        {
            yield return numerator / denominator;
            (numerator, denominator) = (denominator, numerator % denominator);
        } while (!denominator.IsZero);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="continuedFraction"></param>
    /// <returns></returns>
    public static Fraction ToFraction(
        this IEnumerable<BigInteger> continuedFraction)
    {
        Guardant.Instance
            .ThrowIfNullOrEmpty(continuedFraction);

        var continuedFractionComponents = continuedFraction.ToArray();

        var previousNumerator = BigInteger.One;
        var previousDenominator = BigInteger.Zero;
        var currentNumerator = continuedFractionComponents.First();
        var currentDenominator = BigInteger.One;

        foreach (var continuedFractionCoefficient in continuedFractionComponents.Skip(1))
        {
            var nextNumerator = continuedFractionCoefficient * currentNumerator + previousNumerator;
            previousNumerator = currentNumerator;
            currentNumerator = nextNumerator;

            var nextDenominator = continuedFractionCoefficient * currentDenominator + previousDenominator;
            previousDenominator = currentDenominator;
            currentDenominator = nextDenominator;
        }

        return new Fraction(currentNumerator, currentDenominator);
    }
    
}