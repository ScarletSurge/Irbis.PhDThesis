using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

using Irbis.WPF.MVVM.Core.ViewModels;

namespace Irbis.PhDThesis.UI.WPF.Controls.ViewModels;

/// <summary>
/// 
/// </summary>
public sealed class SpinnerViewModel:
    ViewModelBase
{
    
    #region Constants

    private const double TwoPiInDegrees = 360.0;
    
    #endregion
    
    #region Nested
    
    /// <summary>
    /// 
    /// </summary>
    public enum SpinnerRotationDirection
    {
        /// <summary>
        /// 
        /// </summary>
        Clockwise = -1,
        /// <summary>
        /// 
        /// </summary>
        Counterclockwise = 1
    }
    
    #endregion
    
    #region Fields

    /// <summary>
    /// 
    /// </summary>
    private readonly DispatcherTimer _rotationTimer;
    
    /// <summary>
    /// 
    /// </summary>
    private Size _sceneSize;
    
    /// <summary>
    /// 
    /// </summary>
    private SpinnerRotationDirection _rotationDirection;

    /// <summary>
    /// 
    /// </summary>
    private SpinnerItemViewModel[]? _items;
    
    /// <summary>
    /// 
    /// </summary>
    private double _angle;

    #endregion
    
    #region Constructors
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemsCount"></param>
    /// <param name="rotationDirection"></param>
    /// <param name="rotationAngleIncrement"></param>
    /// <param name="rotationInvalidateTimeoutMs"></param>
    public SpinnerViewModel(
        int itemsCount,
        SpinnerRotationDirection rotationDirection,
        double rotationAngleIncrement,
        int rotationInvalidateTimeoutMs)
    {
        Items = RecreateItems(itemsCount);
        RotationDirection = rotationDirection;
        RotationAngleIncrement = rotationAngleIncrement;
        _rotationTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(rotationInvalidateTimeoutMs),
            DispatcherPriority.Normal, OnTimerTick, Dispatcher.CurrentDispatcher);
    }
    
    #endregion
    
    #region Properties
    
    /// <summary>
    /// 
    /// </summary>
    public int TimerInterval
    {
        set =>
            UpdateTimerInterval(value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public Size SceneSize
    {
        private get =>
            _sceneSize;

        set
        {
            _sceneSize = value;
            UpdateSceneSize(SceneSize);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public SpinnerRotationDirection RotationDirection
    {
        private get =>
            _rotationDirection;

        set
        {
            _rotationDirection = value;
            UpdateItems();
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public double RotationAngleIncrement
    {
        private get;

        set;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public int ItemsCount
    {
        private get =>
            Items.Length;

        set =>
            Items = RecreateItems(value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public SpinnerItemViewModel[] Items
    {
        get =>
            _items!;

        private set =>
            SetProperty(ref _items, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    private double Angle
    {
        get =>
            _angle;

        set =>
            _angle = value % 360;
    }
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newIntervalInMs"></param>
    private void UpdateTimerInterval(
        int newIntervalInMs)
    {
        _rotationTimer.Interval = TimeSpan.FromMilliseconds(newIntervalInMs);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newItemsCount"></param>
    /// <returns></returns>
    private SpinnerItemViewModel[] RecreateItems(
        int newItemsCount)
    {
        if (newItemsCount == (Items?.Length ?? 0))
        {
            return Items ??= Array.Empty<SpinnerItemViewModel>();
        }

        var newItems = new SpinnerItemViewModel[newItemsCount];
        var (halfHeight, halfWidth) = (SceneSize.Height / 2, SceneSize.Width / 2);
        var matrix = new Matrix();
        matrix.Translate(Math.Min(SceneSize.Height, SceneSize.Width) * 3 / 8, 0);

        for (var i = 0; i < newItemsCount; i++)
        {
            var newSpinnerItemSize = Math.Min(SceneSize.Height, SceneSize.Width) / (i + 4);
            var location = matrix.Transform(new Point(halfWidth, halfHeight));
            newItems[i] = new SpinnerItemViewModel
            {
                X = location.X - newSpinnerItemSize / 2,
                Y = location.Y - newSpinnerItemSize / 2,
                Size = newSpinnerItemSize
            };
            matrix.RotateAt(-TwoPiInDegrees / (newItemsCount + 1), halfWidth, halfHeight);
        }

        return newItems;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newSceneSize"></param>
    private void UpdateSceneSize(
        Size newSceneSize)
    {
        var halfHeight = newSceneSize.Height / 2;
        var halfWidth = newSceneSize.Width / 2;
        var matrix = new Matrix();
        matrix.Translate(Math.Min(newSceneSize.Height, newSceneSize.Width) * 3 / 8, 0);
        matrix.RotateAt(Angle, halfWidth, halfHeight);

        for (var i = 0; i < Items.Length; i++)
        {
            var location = matrix.Transform(new Point(halfWidth, halfHeight));
            Items[i].Size = Math.Min(SceneSize.Height, SceneSize.Width) / (i + 4);
            Items[i].X = location.X - Items[i].Size / 2;
            Items[i].Y = location.Y - Items[i].Size / 2;
            matrix.RotateAt(-TwoPiInDegrees / (Items.Length + 1), halfWidth, halfHeight);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void UpdateItems()
    {
        UpdateSceneSize(SceneSize);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void Stop()
    {
        _rotationTimer.Stop();
        _rotationTimer.Tick -= OnTimerTick;
    }
    
    #region Event handlers

    private void OnTimerTick(
        object? sender,
        EventArgs eventArgs)
    {
        Angle += (int)RotationDirection * RotationAngleIncrement;
        UpdateItems();
    }
    
    #endregion
    
    #endregion

}