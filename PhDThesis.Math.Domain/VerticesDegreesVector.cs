using System.Collections;

using PhDThesis.Domain.Helpers.Guarding;

namespace PhDThesis.Math.Domain;

/// <summary>
/// 
/// </summary>
public sealed class VerticesDegreesVector
    : IEnumerable<uint>,
      ICloneable
{
    
    #region Fields
    
    /// <summary>
    /// 
    /// </summary>
    private readonly uint[] _verticesDegrees;
    
    #endregion
    
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
        
        _verticesDegrees = verticesDegrees.ToArray();
    }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertexId"></param>
    public uint this[int vertexId] =>
        _verticesDegrees[vertexId];
    
    /// <summary>
    /// 
    /// </summary>
    public int VerticesCount =>
        _verticesDegrees.Length;

    #endregion
    
    #region Methods
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hyperEdge"></param>
    public void AddSimplex(
        HyperEdge hyperEdge)
    {
        Guard.ThrowIfNullOrEmpty(hyperEdge);

        throw new NotImplementedException();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hyperEdge"></param>
    public void RemoveSimplex(
        HyperEdge hyperEdge)
    {
        Guard.ThrowIfNullOrEmpty(hyperEdge);

        throw new NotImplementedException();
    }
    
    #endregion

    #region System.Object overrides

    public override string ToString()
    {
        return $"[ {string.Join(", ", _verticesDegrees)} ]";
    }

    #endregion
    
    #region System.Collections.IEnumerable implementation

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
    
    #region System.Collections.Generic.IEnumerable<out T> implementation

    public IEnumerator<uint> GetEnumerator()
    {
        return ((IEnumerable<uint>)_verticesDegrees).GetEnumerator();
    }
    
    #endregion
    
    #region System.ICloneable implementation

    public object Clone()
    {
        return new VerticesDegreesVector(_verticesDegrees);
    }
    
    #endregion

}