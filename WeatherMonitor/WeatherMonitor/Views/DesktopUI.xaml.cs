using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.Gauges;

namespace WeatherMonitor;

public partial class DesktopUI : ContentPage
{
	public DesktopUI()
	{
		InitializeComponent();
	}

    private void Temperature_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        var selectedItem = e.AddedItems!.FirstOrDefault();

        if (selectedItem is WeatherDataModel obj)
        {
            if (obj.Scale is Enums.TempScale.Celsius && viewModel.TemperatureYPath is not nameof(obj.Celsius))
            {
                viewModel.TemperatureYPath = nameof(obj.Celsius);
                viewModel.FeelsYPath = nameof(obj.FeelsLike);
                viewModel.IsCelsius = true;
                viewModel.Interval = 10;
                viewModel.YAxisMax = 50;
            }
            else if (obj.Scale is Enums.TempScale.Fahrenheit)
            {
                viewModel.TemperatureYPath = nameof(obj.Fahrenheit);
                viewModel.FeelsYPath = nameof(obj.FeelsLikeF);
                viewModel.IsCelsius = false;
                viewModel.Interval = 20;
                viewModel.YAxisMax = 120;
            }
        }
    }

    private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    {
        var index = e.NewIndex;

        if (index == 1)
        {
            chartView.Content = new ColumnChart() { BindingContext = viewModel };
        }
        else
        {
            chartView.Content = new SplineAreaChart() { BindingContext = viewModel };
        }
    }

    private void RadialAxis_LabelCreated(object sender, LabelCreatedEventArgs e)
    {
        GaugeLabelStyle labelStyle = new() { FontSize = 16, FontFamily = "Sen", TextColor = Color.FromArgb("#4FACFE"), FontAttributes = FontAttributes.Bold };

        switch (e.Text)
        {
            case "0":
                e.Text = "N";
                e.Style = labelStyle;
                break;
            case "45":
                e.Text = "NE";
                break;
            case "90":
                e.Text = "E";
                e.Style = labelStyle;
                break;
            case "135":
                e.Text = "SE";
                break;
            case "180":
                e.Text = "S";
                e.Style = labelStyle;
                break;
            case "225":
                e.Text = "SW";
                break;
            case "270":
                e.Text = "W";
                e.Style = labelStyle;
                break;
            case "315":
                e.Text = "NW";
                break;
            default:
                e.Text = string.Empty;
                break;
        }
    }

    private void SfSwitch_StateChanged(object sender, SwitchStateChangedEventArgs e)
    {
        viewModel.SeriesIsVisible = !viewModel.SeriesIsVisible;
    }
}