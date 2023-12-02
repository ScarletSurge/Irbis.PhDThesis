using Irbis.WPF.MVVM.Core.ViewModels;

namespace PhDThesis.UI.WPF.Controls.ViewModels;

/// <summary>
/// 
/// </summary>
public sealed class SpinnerItemViewModel:
    ViewModelBase
{
    
    #region Fields
    
    /// <summary>
    /// 
    /// </summary>
    private double _x;
    
    /// <summary>
    /// 
    /// </summary>
    private double _y;
    
    /// <summary>
    /// 
    /// </summary>
    private double _size;
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// 
    /// </summary>
    public double X
    {
        get =>
            _x;

        set =>
            SetProperty(ref _x, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public double Y
    {
        get =>
            _y;

        set =>
            SetProperty(ref _y, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public double Size
    {
        get =>
            _size;

        set =>
            SetProperty(ref _size, value);
    }
    
    #endregion

}