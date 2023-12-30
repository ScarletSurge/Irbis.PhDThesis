using System.Numerics;

using PhDThesis.Domain.Extensions;
using PhDThesis.Math.Domain;
using PhDThesis.Math.Domain.Fraction;
using PhDThesis.Math.Domain.FractionTree;
using PhDThesis.Math.FractionTree.CalkinWilf;
using PhDThesis.Math.FractionTree.SternBrokot;

void TestHypergraphConstruction()
{
    var hg1 = new HomogenousHypergraph(3, 2, new HyperEdge(0, 1), new (0, 2), new(1, 2));
}

void TestHypergraphIteration()
{
    var hg = new HomogenousHypergraph(3, 2, new HyperEdge(0, 1), new (0, 2), new(1, 2));
    
    for (var i = 0; i < BigIntegerExtensions.CombinationsCount(hg.VerticesCount, hg.SimplicesDimension); i++)
    {
        Console.WriteLine($"{i}: {{{string.Join(", ", hg.BitIndexToSimplex(i))}}} - {(hg.ContainsSimplex(i) ? string.Empty : "not ")}set");
    }
}

void TestHypergraphEtc()
{
    var hg = new HomogenousHypergraph(11, 7, new HyperEdge(10, 9, 8, 7, 6, 5, 4), new HyperEdge(1,2,3,4,5,6,7));

    foreach (var simplex in hg)
    {
        Console.WriteLine($"{{{simplex}}}");
    }
}

void TestContinuedFraction(Fraction targetFraction)
{
    var i = 0;
    
    Console.Write($"{targetFraction} == [ ");
    var targetFractionAsContinuedFraction = targetFraction.ToContinuedFraction();
    foreach (var targetFractionCoefficient in targetFractionAsContinuedFraction)
    {
        Console.Write(targetFractionCoefficient);
        Console.Write(++i == 1
            ? "; "
            : ", ");
    }
    Console.Write($"] == {targetFractionAsContinuedFraction.ToFraction()}");
}

void TestFractionTrees(int? randomSource = null)
{
    randomSource ??= new Random().Next();
    var random = new Random(randomSource.Value);
    Console.WriteLine($"Seed: {randomSource.Value}");
    
    var pathLength = random.Next(256, 1024);
    Console.WriteLine($"Bits count: {pathLength}");
    
    var bits = Enumerable
        .Repeat(0, pathLength)
        .Select(_ => random.Next(2) == 0)
        .ToArray();
    var targetPath = new BitArray(bits);
    Console.WriteLine("Initial path: ");
    foreach (var pathPart in targetPath)
    {
        Console.Write($"{pathPart} ");
    }
    Console.WriteLine();
    
    var trees = new IFractionTree[] { new SternBrokotTree(), new CalkinWilfTree() }
        .Zip(new [] { "Stern-Brokot tree", "Calkin-Wilf tree" });
    foreach (var tree in trees)
    {
        var restoredFraction = tree.First.FindFractionByPath(targetPath);
        var bitsForNumerator = Math.Ceiling(BigInteger.Log(restoredFraction.Numerator, 2));
        var bitsForDenominator = Math.Ceiling(BigInteger.Log(restoredFraction.Denominator, 2));
        Console.WriteLine($"Total bits for numerator and denominator as numbers == {bitsForNumerator + bitsForDenominator}");
        Console.WriteLine($"Restored fraction by path with {tree.Second}: {restoredFraction}");
        var restoredPath = tree.First.FindPathByFraction(restoredFraction);
        Console.WriteLine($"Restored path by restored fraction with {tree.Second}: ");
        foreach (var pathPart in restoredPath)
        {
            Console.Write($"{pathPart} ");
        }
        Console.WriteLine();
    }
}

TestHypergraphConstruction();
TestHypergraphIteration();
TestHypergraphEtc();
TestContinuedFraction(new Fraction(13, 29));
TestFractionTrees(12346);