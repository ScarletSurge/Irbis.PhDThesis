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
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="verticesDegrees"></param>
    public VerticesDegreesVector(
        params uint[] verticesDegrees)
    {
        _verticesDegrees = verticesDegrees.ToArray();
    }

}