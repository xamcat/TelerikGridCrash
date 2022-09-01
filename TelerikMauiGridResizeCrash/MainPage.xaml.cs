using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using TelerikMauiGridResizeCrash.MDIControl;

namespace TelerikMauiGridResizeCrash;

public partial class MainPage : ContentPage
{
    internal MDILayout MdiContainer => this.MdiRoot;
    public MainPage()
	{
		InitializeComponent();

        //add containers

        var view = new ToolContainer();
        AtpCustomLayout.SetLayoutBounds(view, new Rect(150, 200, 500, 300));
        view.WidthRequest = 500;
        view.HeightRequest = 300;
        view.ZIndex = Int32.MaxValue;
        MdiRoot.Children.Add(view);
    }
}