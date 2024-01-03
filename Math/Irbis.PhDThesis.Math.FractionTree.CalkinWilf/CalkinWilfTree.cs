using System.Numerics;

using Irbis.PhDThesis.Domain.Helpers.Guarding;
using Irbis.PhDThesis.Math.Domain;
using Irbis.PhDThesis.Math.Domain.Fraction;
using Irbis.PhDThesis.Math.Domain.FractionTree;

namespace Irbis.PhDThesis.Math.FractionTree.CalkinWilf;

/// <summary>
/// 
/// </summary>
public sealed class CalkinWilfTree:
    IFractionTree
{
    
    /// <inheritdoc cref="IFractionTree.FindFractionByPath" />
    public Fraction FindFractionByPath(
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

        return new Fraction(numerator, denominator);
    }
    
    /// <inheritdoc cref="IFractionTree.FindPathByFraction" />
    public BitArray FindPathByFraction(
        Fraction fraction)
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
                path.Add(true);
                numerator -= denominator;
            }
            else
            {
                path.Add(false);
                denominator -= numerator;
            }
        }

        path.Reverse();
        
        return new BitArray(path);
    }
    
}