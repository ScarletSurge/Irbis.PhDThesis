using PhDThesis.Domain.Extensions;
using PhDThesis.Math.Domain;

Console.WriteLine(BigIntegerExtensions.CombinationsCount(128, 3)/8);

var hg = new HomogenousHypergraph(3, 3,
    new[] { 0, 1, 2 }.OrderBy(x => x));
Console.WriteLine(hg.ContainsSimplex(new [] {0,1,6}.OrderBy(x => x)));
var x = 5;

for (var i = 0; i < BigIntegerExtensions.CombinationsCount(128, 3); i++)
{
    var simplex = hg.BitIndexToSimplex(i);
    Console.WriteLine($"{i}: {{{string.Join(',', simplex)}}} - {(hg.ContainsSimplex(simplex) ? "" : "not ")}set");
}