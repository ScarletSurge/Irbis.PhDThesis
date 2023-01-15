using PhDThesis.Domain.Extensions;
using PhDThesis.Math.Domain;

var hg = new HomogenousHypergraph(11, 7, new HyperEdge(10, 9, 8, 7, 6, 5, 4),
    new HyperEdge(1,2,3,4,5,6,7));
Console.WriteLine(hg.ContainsSimplex(new HyperEdge(10, 9, 8, 7, 6, 5, 4)));

/*for (var i = 0; i < BigIntegerExtensions.CombinationsCount(11, 7); i++)
{
    Console.WriteLine($"{i}: {{{string.Join(", ", hg.BitIndexToSimplex(i))}}} - {(hg.ContainsSimplex(i) ? string.Empty : "not ")}set");
}*/

foreach (var simplex in hg)
{
    Console.WriteLine($"{{{simplex}}}");
}