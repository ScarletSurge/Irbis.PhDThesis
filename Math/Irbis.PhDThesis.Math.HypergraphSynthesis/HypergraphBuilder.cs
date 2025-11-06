using System.Numerics;

using Irbis.PhDThesis.Domain.Extensions;
using Irbis.PhDThesis.Math.Domain;

namespace Irbis.PhDThesis.Math.HypergraphSynthesis;

/// <summary>
/// 
/// </summary>
public sealed class HypergraphBuilder
{
    
    #region Fields
    
    
    
    #endregion
    
    #region Constructors
    
    
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="verticesCount"></param>
    /// <param name="simplicesDimension"></param>
    /// <param name="seed"></param>
    /// <returns></returns>
    public HomogenousHypergraph Randomize(
        int verticesCount,
        int simplicesDimension,
        int seed)
    {
        var componentsDisjointSet = new List<ISet<int>>();
        var randomSource = new Random(seed);

        var simplicesIndices = new HashSet<int>();
        var componentsToUnion = new HashSet<ISet<int>>();

        var simplicesMaxCount = HomogenousHypergraph.GetSimplicesMaxCount(verticesCount, simplicesDimension);
        var simplicesTargetCount = new RandomBigInteger().NextBigInteger(simplicesMaxCount / 4, 3 * simplicesMaxCount / 4);

        var simplicesCurrentCount = BigInteger.Zero;
        while (simplicesCurrentCount != simplicesTargetCount)
        {
            var simplexBitIndex = randomSource.Next((int)simplicesMaxCount);

            if (!simplicesIndices.Add(simplexBitIndex))
            {
                continue;
            }
            
            ++simplicesCurrentCount;
            
            if (componentsDisjointSet.Count == 1 && componentsDisjointSet.Single().Count == verticesCount)
            {
                continue;
            }
            
            var simplex = HomogenousHypergraph.BitIndexToSimplex(simplexBitIndex, simplicesDimension, verticesCount, simplicesMaxCount);
            componentsToUnion.Clear();
            componentsToUnion.Add(simplex.ToHashSet());
            
            foreach (var component in componentsDisjointSet)
            {
                if (simplex.Intersect(component).SequenceEqual(Enumerable.Empty<int>()))
                {
                    continue;
                }

                componentsToUnion.Add(component);
            }

            componentsDisjointSet = componentsDisjointSet.Except(componentsToUnion).ToList();
            componentsDisjointSet.Add(componentsToUnion.SelectMany(x => x).Distinct().ToHashSet());
        }

        return new HomogenousHypergraph(verticesCount, simplicesDimension, simplicesIndices.ToArray());
    }
    
    #endregion
    
}