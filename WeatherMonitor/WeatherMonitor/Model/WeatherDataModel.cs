namespace WeatherMonitor
{
    public class WeatherDataModel
    {
        private float temperature;
        private static readonly Random random = new Random();

        public float Temperature
        {
            get => temperature;
            set
            {
                temperature = Celsius = value;
                Fahrenheit = MathF.Round((value * 9 / 5) + 32, 1);
            }
        }

        public DateTime Time { get; set; }

        public float Celsius { get; set; }

        public float Fahrenheit { get; set; }

        public float FeelsLike { get; set; }

        public float FeelsLikeF => MathF.Round(Fahrenheit + random.Next(5, 11) + random.NextSingle(), 1);

        public int AirQuality { get; set; }

        public float WindSpeed { get; set; }

        public float WindDirection { get; set; }

        public float Feelslike { get; set; }

        public float Humidity { get; set; }

        public float UVIndex { get; set; }

        public float MaxTemperature { get; set; }

        public float MinTemperature { get; set; }

        public float Dew { get; set; }

        public string? WeatherDescription { get; set; }

        public string? Category { get; set; }
        public double ChancePercentage { get; set; }

        public Enums.TempScale Scale { get; set; }
    }
}
