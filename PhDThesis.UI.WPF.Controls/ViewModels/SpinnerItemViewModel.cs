using Irbis.WPF.MVVM.Core.ViewModels;

namespace PhDThesis.UI.WPF.Controls.ViewModels;

/// <summary>
/// 
/// </summary>
public sealed class SpinnerItemViewModel:
    ViewModelBase
{
    
    #region Fields

    private double _x;
    private double _y;
    private double _angle;
    
    #endregion
    
    #region Properties
    
    public double X
    {
        get =>
            _x;

        set
        {
            _x = value;
            OnPropertyChanged(nameof(X));
        }
    }

    public double Y
    {
        get =>
            _y;

        set
        {
            _y = value;
            OnPropertyChanged(nameof(Y));
        }
    }

    public double Angle
    {
        get =>
            _angle;

        set
        {
            _angle = value;
            OnPropertyChanged(nameof(Angle));
        }
    }
    
    #endregion

}