using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Irbis.WPF.MVVM.Core.ViewModels;

namespace PhDThesis.UI.WPF.Controls;

/// <summary>
/// 
/// </summary>
public partial class Spinner : UserControl
{

    #region Nested
    
    /// <summary>
    /// 
    /// </summary>
    public sealed class SpinnerItem:
        ViewModelBase
    {
        
        #region Fields
        
        /// <summary>
        /// 
        /// </summary>
        private double _size;
        
        /// <summary>
        /// 
        /// </summary>
        private double _top;
        
        /// <summary>
        /// 
        /// </summary>
        private double _left;
        
        #endregion
        
        #region Properties
        
        public double Size
        {
            get =>
                _size;
            
            set =>
                SetProperty(ref _size, value);
        }

        public double Top
        {
            get =>
                _top;
            
            set =>
                SetProperty(ref _top, value);
        }

        public double Left
        {
            get =>
                _left;
            
            set =>
                SetProperty(ref _left, value);
        }
        
        #endregion
        
    }

    #endregion
    
    #region Fields

    private DispatcherTimer _rotationTimer;
    
    #endregion
    
    #region Constructors

    public Spinner()
    {
        InitializeComponent();

        _rotationTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 15), DispatcherPriority.Normal,
            (sender, eventArgs) => { Angle = (Angle + (IsClockwiseRotation ? 5 : -5)) % 360; },
            Dispatcher.CurrentDispatcher);

        Loaded += (_, _) =>
        {
            var window = Window.GetWindow(this);
            window!.Closing += OnClosing;
        };

        _rotationTimer.Start();
    }
    
    #endregion
    
    #region Events handlers
    
    private void OnClosing(object? sender, CancelEventArgs e)
    {
        _rotationTimer.Stop();
        var window = Window.GetWindow(this);
        window!.Closing -= OnClosing;
    }
    
    #endregion

    #region Dependency properties
    
    /// <summary>
    /// 
    /// </summary>
    public double SpinnerSize
    {
        get =>
            (double)GetValue(SpinnerSizeProperty);
        
        set =>
            SetValue(SpinnerSizeProperty, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty SpinnerSizeProperty = DependencyProperty.Register(
        nameof(SpinnerSizeProperty), typeof(double), typeof(Spinner), new PropertyMetadata(250.0, (d, e) =>
        {
            var spinner = d as Spinner;
            var spinnerNewSize = (double) e.NewValue;

            for (var i = 0; i < (spinner.SpinnerItems?.Length ?? 0); i++)
            {
                var angle = -360.0 / (spinner.SpinnerItems.Length + 1) * i;
                var matrix = new Matrix();
                matrix.Translate(spinnerNewSize * 3 / 8, 0);
                matrix.RotateAt(angle, spinnerNewSize / 2, spinnerNewSize / 2);
                var location = matrix.Transform(new Point(spinnerNewSize / 2, spinnerNewSize / 2));
                spinner.SpinnerItems[i].Top = location.Y - spinner.SpinnerItems[i].Size / 2;
                spinner.SpinnerItems[i].Left = location.X - spinner.SpinnerItems[i].Size / 2;
            }
        }));
    
    /// <summary>
    /// 
    /// </summary>
    public double Angle
    {
        get =>
            (double)GetValue(AngleProperty);
        
        set =>
            SetValue(AngleProperty, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
        nameof(Angle), typeof(double), typeof(Spinner), new PropertyMetadata(0.0));
    
    /// <summary>
    /// 
    /// </summary>
    public SpinnerItem[] SpinnerItems
    {
        get =>
            (SpinnerItem[]) GetValue(SpinnerItemsProperty);
        
        set =>
            SetValue(SpinnerItemsProperty, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty SpinnerItemsProperty = DependencyProperty.Register(
        nameof(SpinnerItems), typeof(SpinnerItem[]), typeof(Spinner));
    
    /// <summary>
    /// 
    /// </summary>
    public int SpinnerItemsCount
    {
        get =>
            (int)GetValue(SpinnerItemsCountProperty);
        
        set =>
            SetValue(SpinnerItemsCountProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty SpinnerItemsCountProperty = DependencyProperty.Register(
        nameof(SpinnerItemsCount), typeof(int), typeof(Spinner), new PropertyMetadata((d, e) =>
        {
            var spinner = d as Spinner;
            var spinnerItemsNewCount = (int) e.NewValue;
            var spinnerNewItems = new SpinnerItem[spinnerItemsNewCount];

            for (var i = 0; i < spinnerNewItems.Length; i++)
            {
                spinnerNewItems[i] = new SpinnerItem
                {
                    Size = spinner.SpinnerSize / (i + 4)
                };

                var angle = -360.0 / (spinnerItemsNewCount + 1) * i;
                var matrix = new Matrix();
                matrix.Translate(spinner.SpinnerSize * 3 / 8, 0);
                matrix.RotateAt(angle, spinner.SpinnerSize / 2, spinner.SpinnerSize / 2);
                var location = matrix.Transform(new Point(spinner.SpinnerSize / 2, spinner.SpinnerSize / 2));
                spinnerNewItems[i].Top = location.Y - spinnerNewItems[i].Size / 2;
                spinnerNewItems[i].Left = location.X - spinnerNewItems[i].Size / 2;
            }

            spinner.SpinnerItems = spinnerNewItems;
        }));
    
    /// <summary>
    /// 
    /// </summary>
    public Brush SpinnerItemsColor
    {
        get =>
            (Brush)GetValue(SpinnerItemsColorProperty);
        
        set =>
            SetValue(SpinnerItemsColorProperty, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty SpinnerItemsColorProperty = DependencyProperty.Register(
        nameof(SpinnerItemsColor), typeof(Brush), typeof(Spinner), new PropertyMetadata(Brushes.Black));
    
    /// <summary>
    /// 
    /// </summary>
    public bool IsClockwiseRotation
    {
        get =>
            (bool)GetValue(IsClockwiseRotationProperty);
        
        set =>
            SetValue(IsClockwiseRotationProperty, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty IsClockwiseRotationProperty = DependencyProperty.Register(
        nameof(IsClockwiseRotation), typeof(bool), typeof(Spinner));

    #endregion

}