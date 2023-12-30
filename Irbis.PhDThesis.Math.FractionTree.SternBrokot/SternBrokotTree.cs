﻿using System.Numerics;

using Irbis.PhDThesis.Domain.Helpers.Guarding;
using Irbis.PhDThesis.Math.Domain;
using Irbis.PhDThesis.Math.Domain.Fraction;
using Irbis.PhDThesis.Math.Domain.FractionTree;

namespace Irbis.PhDThesis.Math.FractionTree.SternBrokot;

/// <summary>
/// 
/// </summary>
public sealed class SternBrokotTree:
    IFractionTree
{
    
    /// <inheritdoc cref="IFractionTree.FindFractionByPath" />
    public Fraction FindFractionByPath(
        BitArray path)
    {
        Guardant.Instance
            .ThrowIfNull(path);
        
        var numerator = BigInteger.One;
        var denominator = BigInteger.One;
        var leftMediantNumerator = BigInteger.Zero;
        var leftMediantDenominator = BigInteger.One;
        var rightMediantNumerator = BigInteger.One;
        var rightMediantDenominator = BigInteger.Zero;
        
        foreach (var pathPart in path)
        {
            if (pathPart)
            {
                leftMediantNumerator = numerator;
                leftMediantDenominator = denominator;
            }
            else
            {
                rightMediantNumerator = numerator;
                rightMediantDenominator = denominator;
            }

            numerator = leftMediantNumerator + rightMediantNumerator;
            denominator = leftMediantDenominator + rightMediantDenominator;
        }

        return new Fraction(numerator, denominator);
    }
    
    /// <inheritdoc cref="IFractionTree.FindPathByFraction" />
    public BitArray FindPathByFraction(
        Fraction fraction)
    {
        var approximation = new Fraction(BigInteger.One, BigInteger.One);
        var leftMediantNumerator = BigInteger.Zero;
        var leftMediantDenominator = BigInteger.One;
        var rightMediantNumerator = BigInteger.One;
        var rightMediantDenominator = BigInteger.Zero;
        var path = new List<bool>();

        while (approximation != fraction)
        {
            if (approximation > fraction)
            {
                path.Add(false);
                (rightMediantNumerator, rightMediantDenominator) = approximation;
            }
            else
            {
                path.Add(true);
                (leftMediantNumerator, leftMediantDenominator) = approximation;
            }
            
            approximation = new Fraction(leftMediantNumerator + rightMediantNumerator, leftMediantDenominator + rightMediantDenominator);
        }

        return new BitArray(path);
    }
    
}