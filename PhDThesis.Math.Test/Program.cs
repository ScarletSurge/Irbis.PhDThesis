using PhDThesis.Domain.Extensions;
using PhDThesis.Math.Domain;
using PhDThesis.Math.Domain.Fraction;
using PhDThesis.Math.Domain.FractionTree;

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

void TestContinuedFraction()
{
    var i = 0;
    
    Console.Write("[ ");
    foreach (var component in new Fraction(1425673, 8137).ToContinuedFraction())
    {
        Console.Write(component);
        Console.Write(++i == 1
            ? "; "
            : ", ");
    }
    Console.Write(']');
}

void TestCalkinWilfTree(int? randomSource = null)
{
    randomSource ??= new Random().Next();
    var random = new Random(randomSource.Value);
    Console.WriteLine($"Seed: {randomSource.Value}");
    
    var pathLength = random.Next(500, 1000);
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
    
    var trees = new IFractionTree[] { new SternBrokotTree(), new CalkinWilfTree() }.Zip(new [] { "Stern-Brokot tree", "Calkin-Wilf tree" });
    foreach (var tree in trees)
    {
        var restoredFraction = tree.First.FindFractionByPath(targetPath);
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

//TestHypergraphConstruction();
//TestHypergraphIteration();
//TestHypergraphEtc();
//TestContinuedFraction();
TestCalkinWilfTree(12345);
//TestSternBrokotTree();