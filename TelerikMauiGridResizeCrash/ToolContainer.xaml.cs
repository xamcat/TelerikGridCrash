using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TelerikMauiGridResizeCrash.MDIControl;

namespace TelerikMauiGridResizeCrash;

public partial class ToolContainer : ContentView
{
    private ISettingsService Settings => Context.Settings;
    private readonly List<View> _gestureTargets = new List<View>();
    private readonly List<View> _gestureResizeTargetsOnly = new List<View>();
    private bool _initialized = false;
    private Color _resizingColor;
    private Image _pausePlaceholder;

    public ToolContainerViewModel Context
    {
        get => (ToolContainerViewModel)this.BindingContext;
        set => this.BindingContext = value;
    }

    public static readonly BindableProperty ToolViewProperty =
           BindableProperty.Create(nameof(ToolView), typeof(IToolView), typeof(ToolContainer), null, propertyChanged: OnToolViewContentChanged);

    public IToolView ToolView
    {
        get => (IToolView)GetValue(ToolViewProperty);
        set => SetValue(ToolViewProperty, value);
    }

    public static void OnToolViewContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var contentView = bindable as ToolContainer;
        contentView.SetContent(newValue as IView);
    }


    public ToolContainer()
	{
        Debug.WriteLine("Constructing ToolContainer");
        InitializeComponent();
        SetupGestureTargets();
    }

    public void SetContent(IView content)
    {
        WindowContentContainer.Children.Clear();
        WindowContentContainer.Add(content);
    }


    private void SetupGestureTargets()
    {
        _gestureTargets.Add(WindowHeader);
        _gestureTargets.Add(WindowBorderLeft);
        _gestureTargets.Add(WindowBorderTop);
        _gestureTargets.Add(WindowBorderRight);
        _gestureTargets.Add(WindowBorderBottom);
        _gestureTargets.Add(WindowBorderRightBottom);

        _gestureResizeTargetsOnly.Add(WindowBorderRight);
        _gestureResizeTargetsOnly.Add(WindowBorderBottom);
        _gestureResizeTargetsOnly.Add(WindowBorderRightBottom);
    }

    public void SetViewState(Color resizingColor)
    {
        _resizingColor = resizingColor;
    }

    public void InitializeLayout(PanGestureRecognizer panGesture, TapGestureRecognizer tapGesture)
    {
        if (_initialized)
            return;

        _gestureTargets.ForEach(v => v.GestureRecognizers.Add(panGesture));
        WindowHeader.GestureRecognizers.Add(tapGesture);
  
        _initialized = true;
    }

    public void ReleaseLayout()
    {
        if (!_initialized)
            return;

        _gestureTargets.ForEach(v => v.GestureRecognizers.Clear());
        _initialized = false;
    }

    public ToolContainerInteractionState GetInteractionStateFromTarget(Element gestureTarget)
    {
        var result = ToolContainerInteractionState.Idle;

        if (gestureTarget == WindowHeader
            || gestureTarget == WindowBorderLeft
            || gestureTarget == WindowBorderTop)
        {
            result = ToolContainerInteractionState.Moving;
        }
        else if (gestureTarget == WindowBorderRight)
            result = ToolContainerInteractionState.ResizingRight;
        else if (gestureTarget == WindowBorderBottom)
            result = ToolContainerInteractionState.ResizingBottom;
        else if (gestureTarget == WindowBorderRightBottom)
            result = ToolContainerInteractionState.ResizingRightBottom;

        return result;
    }

    public void OnRearrangeCompleted(bool self)
    {
        SwitchContentVisibilityState(true);

        if (self)
            _gestureResizeTargetsOnly.ForEach(v => v.BackgroundColor = Colors.Transparent);
    }
    private void SwitchContentVisibilityState(bool isVisible)
    {
        if (_pausePlaceholder != null && WindowContentContainer.Contains(_pausePlaceholder))
            WindowContentContainer.Remove(_pausePlaceholder);

        if (!isVisible)
        {
            _pausePlaceholder = new Image()
            {
                Source = new FileImageSource() { File = "dotnet_bot.png" },
                Aspect = Aspect.Center,
            };

            WindowContentContainer.Add(_pausePlaceholder);
            //UpdatePlaceholderAsync().HandleResult();
        }

        if (WindowContentContainer != null)
            WindowContentContainer.IsVisible = isVisible;

        //if (WindowHeaderContent != null)
        //    ((View)WindowHeaderContent).IsVisible = isVisible;
    }
    public void OnRearrangeStarted(bool self, ToolContainerInteractionState forState)
    {
        switch (Settings.PauseUpdatesOnRearrange)
        {
            case RearrangeUpdateBehavior.DoNothing:
                break;
            case RearrangeUpdateBehavior.HideMine:
                if (self)
                    SwitchContentVisibilityState(false);
                break;
            case RearrangeUpdateBehavior.HideAll:
                SwitchContentVisibilityState(false);
                break;
            case RearrangeUpdateBehavior.PauseMine:
                if (self)
                    Context.OnPauseUpdates();
                break;
            case RearrangeUpdateBehavior.PauseAll:
                Context.OnPauseUpdates();
                break;
            case RearrangeUpdateBehavior.PauseAndHideMine:
                Context.OnPauseUpdates();
                if (self)
                    SwitchContentVisibilityState(false);
                break;
            case RearrangeUpdateBehavior.PauseAndHideAll:
                Context.OnPauseUpdates();
                SwitchContentVisibilityState(false);
                break;
            default:
                break;
        }

        if (self && forState != ToolContainerInteractionState.Moving)
            _gestureResizeTargetsOnly.ForEach(v => v.BackgroundColor = _resizingColor);
    }

    private ToolContainerViewModel GetChildViewModelContext()
    {
        var content = WindowContentContainer.Children.FirstOrDefault();
        if (content != null
            && ((View)content).BindingContext is ToolContainerViewModel childViewModel
            && childViewModel != this.BindingContext)
        {
            return childViewModel;
        }

        return null;
    }
}

public interface IToolView : IView
{
}