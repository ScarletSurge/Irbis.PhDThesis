using PhDThesis.Domain.Extensions;
using PhDThesis.Math.Domain;

Console.WriteLine(BigIntegerExtensions.CombinationsCount(128, 3) / 8);

var hg = new HomogenousHypergraph(64, 7);
Console.WriteLine(hg.ContainsSimplex(new [] { 0, 1, 6 }.OrderBy(x => x)));
var x = 5;

for (var i = 0; i < BigIntegerExtensions.CombinationsCount(64, 7); i++)
{
    Console.WriteLine($"{i}: {{{string.Join(',', i)}}} - {(hg.ContainsSimplex(i) ? string.Empty : "not ")}set");
}