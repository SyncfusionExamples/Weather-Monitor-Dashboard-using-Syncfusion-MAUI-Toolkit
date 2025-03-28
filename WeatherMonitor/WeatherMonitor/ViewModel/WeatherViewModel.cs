using Microsoft.Maui.Controls.Shapes;
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

        public Geometry? LocationPath { get; set; } = (Geometry?)new PathGeometryConverter().ConvertFromInvariantString("M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z");
        public Geometry? DewPath { get; set; } = (Geometry?)new PathGeometryConverter().ConvertFromInvariantString("M7 4V3C7 1.89543 6.10457 1 5 1C3.89543 1 3 1.89543 3 3V13.5351C1.8044 14.2267 1 15.5194 1 17C1 19.2091 2.79086 21 5 21C7.20914 21 9 19.2091 9 17C9 15.5194 8.1956 14.2267 7 13.5351V10M7 4H9M7 4V7M7 7H10M7 7V10M7 10H9M13 10.8889C13 12.6071 14.3431 14 16 14C17.6569 14 19 12.6071 19 10.8889C19 9.17067 16 6 16 6C16 6 13 9.17067 13 10.8889Z");
        public Geometry? UVIndexPath { get; set; } = (Geometry?)new PathGeometryConverter().ConvertFromInvariantString("M11 1V3M11 19V21M3 11H1M5.31412 5.31412L3.8999 3.8999M16.6859 5.31412L18.1001 3.8999M5.31412 16.69L3.8999 18.1042M16.6859 16.69L18.1001 18.1042M21 11H19M16 11C16 13.7614 13.7614 16 11 16C8.23858 16 6 13.7614 6 11C6 8.23858 8.23858 6 11 6C13.7614 6 16 8.23858 16 11Z");
        public Geometry? MaximumPath { get; set; } = (Geometry?)new PathGeometryConverter().ConvertFromInvariantString("M3 13C3.55228 13 4 12.5523 4 12C4 11.4477 3.55228 11 3 11V13ZM1 11C0.447715 11 0 11.4477 0 12C0 12.5523 0.447715 13 1 13V11ZM15 13.5351L15.5007 14.4007C15.8097 14.222 16 13.8921 16 13.5351H15ZM19 13.5351H18C18 13.8921 18.1903 14.222 18.4993 14.4007L19 13.5351ZM11.3848 9.02478C11.9325 9.09561 12.4339 8.70901 12.5048 8.16129C12.5756 7.61356 12.189 7.11213 11.6413 7.04129L11.3848 9.02478ZM8.5019 16.3312C8.98031 16.6072 9.59183 16.443 9.86776 15.9646C10.1437 15.4862 9.97955 14.8747 9.50114 14.5987L8.5019 16.3312ZM10 4C10 4.55228 10.4477 5 11 5C11.5523 5 12 4.55228 12 4H10ZM12 2C12 1.44772 11.5523 1 11 1C10.4477 1 10 1.44772 10 2H12ZM6.32133 18.0929C6.71186 17.7024 6.71186 17.0692 6.32133 16.6787C5.93081 16.2882 5.29764 16.2882 4.90712 16.6787L6.32133 18.0929ZM3.49291 18.0929C3.10238 18.4834 3.10238 19.1166 3.49291 19.5071C3.88343 19.8976 4.51659 19.8976 4.90712 19.5071L3.49291 18.0929ZM4.70711 7.12132C5.09763 7.51184 5.7308 7.51184 6.12132 7.12132C6.51184 6.7308 6.51184 6.09763 6.12132 5.70711L4.70711 7.12132ZM4.70711 4.29289C4.31658 3.90237 3.68342 3.90237 3.29289 4.29289C2.90237 4.68342 2.90237 5.31658 3.29289 5.70711L4.70711 4.29289ZM3 11H1V13H3V11ZM14 3V13.5351H16V3H14ZM20 17C20 18.6569 18.6569 20 17 20V22C19.7614 22 22 19.7614 22 17H20ZM17 20C15.3431 20 14 18.6569 14 17H12C12 19.7614 14.2386 22 17 22V20ZM14 17C14 15.8908 14.6014 14.921 15.5007 14.4007L14.4993 12.6695C13.0074 13.5325 12 15.148 12 17H14ZM18.4993 14.4007C19.3986 14.921 20 15.8908 20 17H22C22 15.148 20.9926 13.5325 19.5007 12.6695L18.4993 14.4007ZM17 2C17.5523 2 18 2.44772 18 3H20C20 1.34315 18.6569 0 17 0V2ZM17 0C15.3431 0 14 1.34315 14 3H16C16 2.44772 16.4477 2 17 2V0ZM11.6413 7.04129C10.4948 6.89303 9.33244 7.14659 8.3519 7.75882L9.41114 9.45529C9.99947 9.08796 10.6969 8.93582 11.3848 9.02478L11.6413 7.04129ZM8.3519 7.75882C7.37136 8.37105 6.63324 9.30413 6.26316 10.3993L8.1579 11.0396C8.37995 10.3825 8.82281 9.82263 9.41114 9.45529L8.3519 7.75882ZM6.26316 10.3993C5.89308 11.4944 5.91389 12.6839 6.32206 13.7655L8.19324 13.0593C7.94833 12.4104 7.93585 11.6966 8.1579 11.0396L6.26316 10.3993ZM6.32206 13.7655C6.73023 14.847 7.50054 15.7537 8.5019 16.3312L9.50114 14.5987C8.90032 14.2522 8.43814 13.7082 8.19324 13.0593L6.32206 13.7655ZM12 4V2H10V4H12ZM4.90712 16.6787L3.49291 18.0929L4.90712 19.5071L6.32133 18.0929L4.90712 16.6787ZM6.12132 5.70711L4.70711 4.29289L3.29289 5.70711L4.70711 7.12132L6.12132 5.70711ZM17 17V19C18.1046 19 19 18.1046 19 17H17ZM17 17H15C15 18.1046 15.8954 19 17 19V17ZM17 17V15C15.8954 15 15 15.8954 15 17H17ZM17 17H19C19 15.8954 18.1046 15 17 15V17ZM18 3V13.5351H20V3H18Z");
        public Geometry? WindSpeedPath { get; set; } = (Geometry?)new PathGeometryConverter().ConvertFromInvariantString("M15.7639 4.5C16.3132 3.88625 17.1115 3.5 18 3.5C19.6569 3.5 21 4.84315 21 6.5C21 8.15685 19.6569 9.5 18 9.5H12M5.7639 2C6.31322 1.38625 7.1115 1 8 1C9.65686 1 11 2.34315 11 4C11 5.65685 9.65686 7 8 7H1M9.7639 18C10.3132 18.6137 11.1115 19 12 19C13.6569 19 15 17.6569 15 16C15 14.3431 13.6569 13 12 13H1");

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
