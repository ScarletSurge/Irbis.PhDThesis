using System.Collections;
using PhDThesis.Domain.Helpers.Guarding;

namespace PhDThesis.Math.Domain;

/// <summary>
/// 
/// </summary>
public sealed class HyperEdge : IEnumerable<uint>
{
    
    #region Fields
    
    /// <summary>
    /// 
    /// </summary>
    private SortedSet<uint> _vertices;
    
    #endregion
    
    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public HyperEdge(
        params uint[] values)
    {
        Guard.ThrowIfNull(values);
        Guard.ThrowIfEmpty(values);

        _vertices = new SortedSet<uint>(values);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    public HyperEdge(
        IEnumerable<uint> values)
    {
        Guard.ThrowIfNull(values);
        Guard.ThrowIfEmpty(values);
        
        _vertices = new SortedSet<uint>(values);
    }
    
    #endregion
    
    #region System.Object overrides

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return string.Join(", ", _vertices);
    }

    #endregion
    
    #region System.Collections.IEnumerator implementation
    
    /// <inheritdoc cref="IEnumerable.GetEnumerator" />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    #endregion
    
    #region System.Collections.Generic.IEnumerable<out T> implementation
    
    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    public IEnumerator<uint> GetEnumerator()
    {
        return _vertices.GetEnumerator();
    }
    
    #endregion

}