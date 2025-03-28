using Syncfusion.Maui.Toolkit.SegmentedControl;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WeatherMonitor
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        private bool isCelsius = true;
        private string temperatureYPath = nameof(WeatherDataModel.Celsius);
        private string feelsYPath = nameof(WeatherDataModel.FeelsLike);
        private float minTemperature;
        private float maxTemperature;
        private int interval = 10;
        private int yAxisMax = 50;
        private bool seriesIsVisible;

        public WeatherViewModel()
        {
            City = "Los Santos";
            DateTime time = DateTime.Today;
            Date = time.ToString("MMMM dd, yyyy");

            WeatherData = new ObservableCollection<WeatherDataModel>();
            Random random = new();

            for (int i = 1; i <= 24; i++)
            {
                float temp = MathF.Round(random.Next(24, 28) + random.NextSingle(), 1);

                WeatherData.Add(new WeatherDataModel
                {
                    Time = time,
                    Temperature = temp,
                    WindSpeed = MathF.Round(random.Next(1, 15) + random.NextSingle(), 1),
                    WindDirection = random.Next(0, 360),
                    Humidity = random.Next(40, 90),
                    UVIndex = MathF.Round(random.Next(0, 12) + random.NextSingle(), 1),
                    Dew = MathF.Round(random.Next(5, 20) + random.NextSingle(), 1),
                    FeelsLike = MathF.Round(temp + random.Next(5, 11) + random.NextSingle(), 1)
                });

                time = time.AddHours(1);
            }

            MinTemperature = WeatherData.Min(min => min.Temperature);
            MaxTemperature = WeatherData.Max(max => max.Temperature);

            RainData = new ObservableCollection<WeatherDataModel>
            {
                new() { Category = "Rain", ChancePercentage = 55 },
                new() { Category = "Humidity", ChancePercentage = WeatherData[0].Humidity },
                new() { Category = "Dew", ChancePercentage = WeatherData[0].Dew }
            };

            SegmentItems = new ObservableCollection<SfSegmentItem>
            {
                new() { Text = "Temperature" },
                new() { Text = "Overview" }
            };

            TemperatureScales = new ObservableCollection<WeatherDataModel>
            {
                new() { Scale = Enums.TempScale.Celsius },
                new() { Scale = Enums.TempScale.Fahrenheit }
            };
        }

        public string? City { get; set; }

        public string Date { get; set; }

        public ObservableCollection<WeatherDataModel> WeatherData { get; set; }

        public ObservableCollection<WeatherDataModel> RainData { get; set; }

        public ObservableCollection<SfSegmentItem> SegmentItems { get; set; }

        public ObservableCollection<WeatherDataModel> TemperatureScales { get; set; }

        public string CurrentTemperature => IsCelsius ? $"{WeatherData[0].Celsius}°C" : $"{WeatherData[0].Fahrenheit}°F";

        public int YAxisMax
        {
            get => yAxisMax;
            set
            {
                yAxisMax = value;
                OnPropertyChanged(nameof(YAxisMax));
            }
        }

        public bool SeriesIsVisible
        {
            get => seriesIsVisible;
            set
            {
                seriesIsVisible = value;
                OnPropertyChanged(nameof(SeriesIsVisible));
            }
        }

        public float MinTemperature
        {
            get => IsCelsius ? minTemperature : (minTemperature * 9 / 5) + 32;
            set
            {
                minTemperature = value;
                OnPropertyChanged(nameof(MinTemperature));
            }
        }

        public float MaxTemperature
        {
            get => IsCelsius ? maxTemperature : (maxTemperature * 9 / 5) + 32;
            set
            {
                maxTemperature = value;
                OnPropertyChanged(nameof(MaxTemperature));
            }
        }

        public bool IsCelsius
        {
            get => isCelsius;
            set
            {
                isCelsius = value;
                OnPropertyChanged(nameof(IsCelsius));
                OnPropertyChanged(nameof(CurrentTemperature));
                OnPropertyChanged(nameof(MinTemperature));
                OnPropertyChanged(nameof(MaxTemperature));
            }
        }

        public int Interval
        {
            get => interval;
            set
            {
                interval = value;
                OnPropertyChanged(nameof(Interval));
            }
        }

        public string TemperatureYPath
        {
            get => temperatureYPath;
            set
            {
                temperatureYPath = value;
                OnPropertyChanged(nameof(TemperatureYPath));
            }
        }

        public string FeelsYPath
        {
            get => feelsYPath;
            set
            {
                feelsYPath = value;
                OnPropertyChanged(nameof(FeelsYPath));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
