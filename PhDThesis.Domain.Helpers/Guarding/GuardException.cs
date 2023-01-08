namespace PhDThesis.Domain.Helpers.Guarding;

/// <summary>
/// TODO: other base exception type?
/// </summary>
public sealed class GuardException : Exception
{
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public GuardException(
        string message,
        Exception? innerException = null)
        : base(message, innerException)
    {
        
    }
    
    #endregion
    
}