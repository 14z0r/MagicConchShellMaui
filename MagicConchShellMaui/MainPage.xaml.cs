using CommunityToolkit.Maui.Media;
using MagicConchShellMaui.Views;

namespace MagicConchShellMaui;

public partial class MainPage : ContentPage
{ 
	public MainPage()
    {
		InitializeComponent();
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        Globals.MainGrid = MainGrid;
        BindingContext = new MainPageView(SpeechToText.Default);
    }
}

