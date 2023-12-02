using System.Numerics;

using PhDThesis.Domain.Helpers.Guarding;

namespace PhDThesis.Math.Domain.FractionTree;

/// <summary>
/// 
/// </summary>
public sealed class CalkinWilfTree:
    IFractionTree
{
    
    /// <inheritdoc cref="IFractionTree.FindFractionByPath" />
    public Fraction.Fraction FindFractionByPath(
        BitArray path)
    {
        Guardant.Instance
            .ThrowIfNullOrEmpty(path);

        var numerator = BigInteger.One;
        var denominator = BigInteger.One;

        foreach (var pathPart in path)
        {
            if (pathPart)
            {
                numerator += denominator;
            }
            else
            {
                denominator += numerator;
            }
        }

        return new Fraction.Fraction(numerator, denominator);
    }
    
    /// <inheritdoc cref="IFractionTree.FindPathByFraction" />
    public BitArray FindPathByFraction(
        Fraction.Fraction fraction)
    {
        Guardant.Instance
            .ThrowIfLowerThan(fraction.Numerator, BigInteger.Zero)
            .ThrowIfLowerThanOrEqualTo(fraction.Denominator, BigInteger.Zero);
        
        var numerator = fraction.Numerator;
        var denominator = fraction.Denominator;
        var path = new List<bool>();

        while (numerator != denominator)
        {
            if (numerator > denominator)
            {
                path.Insert(0, true);
                numerator -= denominator;
            }
            else
            {
                path.Insert(0, false);
                denominator -= numerator;
            }
        }

        return new BitArray(path);
    }
    
}