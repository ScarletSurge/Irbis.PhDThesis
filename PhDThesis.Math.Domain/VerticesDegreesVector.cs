using PhDThesis.Domain.Helpers.Guarding;

namespace PhDThesis.Math.Domain;

/// <summary>
/// 
/// </summary>
public sealed class VerticesDegreesVector
{
    
    /// <summary>
    /// 
    /// </summary>
    private uint[] _verticesDegrees;
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="verticesDegrees"></param>
    public VerticesDegreesVector(
        params uint[]? verticesDegrees)
    {
        if (verticesDegrees is null)
        {
            throw new ArgumentNullException(nameof(verticesDegrees));
        }
        
        if (verticesDegrees.Length == 0)
        {
            throw new ArgumentException("Vertices degrees vector can't be empty", nameof(verticesDegrees));
        }
        
        _verticesDegrees = verticesDegrees;
    }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertexId"></param>
    public uint this[int vertexId] =>
        _verticesDegrees[vertexId];

    #endregion
    
    #region Methods
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="simplexVertices"></param>
    public void AddSimplex(
        IOrderedEnumerable<int> simplexVertices)
    {
        Guard.ThrowIfNull(simplexVertices);
        Guard.ThrowIfEmpty(simplexVertices);
        
        
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="simplexVertices"></param>
    public void RemoveSimplex(
        IOrderedEnumerable<int> simplexVertices)
    {
        Guard.ThrowIfNull(simplexVertices);
        Guard.ThrowIfEmpty(simplexVertices);
        
        
    }
    
    #endregion

    #region System.Object overrides

    public override string ToString()
    {
        return $"[ {string.Join(", ", _verticesDegrees)} ]";
    }

    #endregion

}