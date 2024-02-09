using System.Numerics;
using System.Text;
using Irbis.PhDThesis.Domain.Extensions;
using Irbis.PhDThesis.Math.Domain;
using Irbis.PhDThesis.Math.Domain.Fraction;
using Irbis.PhDThesis.Math.Domain.FractionTree;
using Irbis.PhDThesis.Math.FractionTree.CalkinWilf;
using Irbis.PhDThesis.Math.FractionTree.SternBrokot;

int GetFractionBitsCount(
    Fraction fraction)
{
    var bitsForNumerator = Math.Ceiling(BigInteger.Log(fraction.Numerator + 1, 2));
    var bitsForDenominator = Math.Ceiling(BigInteger.Log(fraction.Denominator + 1, 2));

    return (int)(bitsForNumerator + bitsForDenominator);
}

void TestBitsCountInBitArray(
    int iterationsCount = 10000)
{
    for (var i = 0; i < 1001; i++)
    {
        var bitsCountExpected = (uint) i;
        var bitArray = new BitArray(bitsCountExpected);
        Console.WriteLine($"[{i:0000}] {(bitsCountExpected == bitArray.BitsCount ? "P" : "Not p")}assed: expected - {bitsCountExpected:0000}, actual - {bitArray.BitsCount:0000}");
    }

    for (var i = 0; i < 1001; i++)
    {
        var bitsCountExpected = (uint) i;
        var bitArray = new BitArray(Enumerable.Repeat(false, i));
        Console.WriteLine($"[{i:0000}] {(bitsCountExpected == bitArray.BitsCount ? "P" : "Not p")}assed: expected - {bitsCountExpected:0000}, actual - {bitArray.BitsCount:0000}");
    }
}

void TestHomogenousHypergraphConstruction()
{
    var hg1 = new HomogenousHypergraph(3, 2, new HyperEdge[] { new (0, 1), new (0, 2), new(1, 2) });
}

void TestHomogenousHypergraphIteration()
{
    var hg = new HomogenousHypergraph(3, 2, new HyperEdge[] { new (0, 1), new (0, 2), new(1, 2) });
    
    for (var i = 0; i < BigIntegerExtensions.CombinationsCount(hg.VerticesCount, hg.SimplicesDimension); i++)
    {
        Console.WriteLine($"{i}: {{{string.Join(", ", hg.BitIndexToSimplex(i))}}} - {(hg.ContainsSimplex(i) ? string.Empty : "not ")}set");
    }
}

void TestHomogenousHypergraphEtc()
{
    var hg = new HomogenousHypergraph(11, 7, new HyperEdge[] { new (10, 9, 8, 7, 6, 5, 4), new (1, 2, 3, 4, 5, 6, 7) });

    foreach (var simplex in hg)
    {
        Console.WriteLine($"{{{simplex}}}");
    }
}

void TestContinuedFraction(
    params Fraction[] fractions)
{
    foreach (var fraction in fractions)
    {
        var i = 0;

        Console.Write($"{fraction} == [ ");
        var continuedFraction = fraction.ToContinuedFraction();
        using var continuedFractionCoefficientsEnumerator = continuedFraction.GetEnumerator();
        if (continuedFractionCoefficientsEnumerator.MoveNext())
        {
            while (true)
            {
                Console.Write(continuedFractionCoefficientsEnumerator.Current);
                var enumeratorMoveNextSucceeded = continuedFractionCoefficientsEnumerator.MoveNext();

                if (i == 0)
                {
                    i = 1;
                    Console.Write("; ");
                    continue;
                }

                if (!enumeratorMoveNextSucceeded)
                {
                    break;
                }

                Console.Write(", ");
            }
        }

        Console.WriteLine($" ] == {continuedFraction.ToFraction()}");
    }
}

void TestFractionTrees(
    int pathLengthMinBound = 1000,
    int pathLengthMaxBound = 2000,
    int? randomSource = null)
{
    randomSource ??= new Random().Next();
    var random = new Random(randomSource.Value);
    Console.WriteLine($"Seed: {randomSource.Value}");
    
    var pathLength = random.Next(pathLengthMinBound, pathLengthMaxBound);
    Console.WriteLine($"Bits count: {pathLength}");
    
    var bits = Enumerable
        .Repeat(0, pathLength)
        .Select(_ => random.Next(2) == 0)
        .ToArray();
    var targetPath = new BitArray(bits);
    
    //Console.WriteLine("Initial path: ");
    //foreach (var pathPart in targetPath)
    //{
    //    Console.Write($"{pathPart} ");
    //}
    //Console.WriteLine();
    
    var trees = new IFractionTree[] { new SternBrokotTree(), new CalkinWilfTree() }
        .Zip(new [] { "Stern-Brokot tree", "Calkin-Wilf tree" });
    foreach (var tree in trees)
    {
        var restoredFraction = tree.First.FindFractionByPath(targetPath);
        Console.WriteLine($"Total bits for numerator and denominator as numbers == {GetFractionBitsCount(restoredFraction)}");

        //var continuedFraction = restoredFraction.ToContinuedFraction();
        //Console.Write($"Continued fraction: ");
        //foreach (var continuedFractionComponent in continuedFraction)
        //{
        //    Console.Write($"{continuedFractionComponent} ");
        //}
        //Console.WriteLine();

        Console.WriteLine($"Restored fraction by path with {tree.Second}: {restoredFraction}");
        var restoredPath = tree.First.FindPathByFraction(restoredFraction);
        Console.WriteLine("Restored path by restored fraction with {0} is {1}equal to initial path", tree.Second, restoredPath.SequenceEqual(targetPath)? string.Empty : "not ");
    }
}

void TestDavid(
    int? randomSource = null)
{
    randomSource ??= new Random().Next();
    var random = new Random(randomSource.Value);
    Console.WriteLine($"Seed: {randomSource.Value}");
    
    var cwTree = new CalkinWilfTree();
    var sbTree = new SternBrokotTree();
    //var continuedFraction = Enumerable
    //    .Repeat(0, random.Next(10, 51))
    //    .Select(x => (BigInteger)random.Next(1, 129))
    //    .ToArray();
    //Array.Sort(continuedFraction);
    //continuedFraction = continuedFraction.Reverse().ToArray();
    //var fraction = continuedFraction.ToFraction();
    var fraction = new Fraction(3, 4);
    Console.Write(fraction);
    Console.WriteLine($", total bits for numerator and denominator as numbers == {GetFractionBitsCount(fraction)}");

    var cwTreePath = cwTree.FindPathByFraction(fraction);
    var sbTreePath = sbTree.FindPathByFraction(fraction);
    Console.WriteLine($"Calkin-Wilf representation length = {cwTreePath.BitsCount}");
    Console.WriteLine($"Stern-Brokot representation length = {sbTreePath.BitsCount}");
}

void TestWhoIsShorter(
    int iterationsCount = 1000,
    int pathLengthMinBound = 1000,
    int pathLengthMaxBound = 2000,
    int? randomSource = null)
{
    randomSource ??= new Random().Next();
    var random = new Random(randomSource.Value);
    Console.WriteLine($"Seed: {randomSource.Value}");
    
    var cwTree = new CalkinWilfTree();
    var sbTree = new SternBrokotTree();
    
    for (var i = 1; i <= iterationsCount; i++)
    {
        var pathLength = random.Next(pathLengthMinBound, pathLengthMaxBound);
        var bits = Enumerable
            .Repeat(0, pathLength)
            .Select(_ => random.Next(2) == 0)
            .ToArray();
        var targetPath = new BitArray(bits);
        var sbTreeFraction = sbTree.FindFractionByPath(targetPath);
        var fractionBitsCount = GetFractionBitsCount(sbTreeFraction);
        var comparisonResult = fractionBitsCount.CompareTo(targetPath.BitsCount);
        Console.Write($"[{i:00000}][Stern-Brokot] path length = {targetPath.BitsCount:00000}, fraction length = {fractionBitsCount:00000} => ");
        Console.Write(comparisonResult switch
        {
            0 => "Equal",
            < 0 => "Fraction",
            _ => "Path"
        });
        Console.WriteLine(" is shorter.");
        
        var cwTreeFraction = cwTree.FindFractionByPath(targetPath);
        fractionBitsCount = GetFractionBitsCount(cwTreeFraction);
        comparisonResult = fractionBitsCount.CompareTo(targetPath.BitsCount);
        Console.Write($"[{i:00000}][Calkin-Wilf] path length = {targetPath.BitsCount:00000}, fraction length = {fractionBitsCount:00000} => ");
        Console.Write(comparisonResult switch
        {
            0 => "Equal",
            < 0 => "Fraction",
            _ => "Path"
        });
        Console.WriteLine(" is shorter.");
    }
}

void TestSternBrokotTreePaths()
{
    IFractionTree tree = new SternBrokotTree();
    var builder = new StringBuilder();

    for (var i = 1; i <= 11; i++)
    {
        for (var j = (int)BigInteger.Pow(2, i - 1) - 1; j >= 0; j--)
        {
            var str = Convert.ToString(j, 2);
            builder
                .Append(new string(Enumerable.Repeat('0', i - (str.Length > i ? i : str.Length)).ToArray()))
                .Append(str);
            var bitArray = new BitArray(builder.ToString().Select(x => int.Parse(x.ToString()) == 0));
            builder.Clear();
            var fraction = tree.FindFractionByPath(bitArray);
            var difference = fraction.Numerator - fraction.Denominator;

            Console.Write($"{fraction} ");
        }
    
        Console.WriteLine();
    }
}

// TestBitsCountInBitArray();
// TestHomogenousHypergraphConstruction();
// TestHomogenousHypergraphIteration();
// TestHomogenousHypergraphEtc();
// TestContinuedFraction(new Fraction(44435, 10587));
// TestFractionTrees();
// TestDavid();
// TestWhoIsShorter();

TestSternBrokotTreePaths();