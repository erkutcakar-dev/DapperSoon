namespace DapperSoon.Dtos
{
    public class ChartDataDto
    {
        public string Label { get; set; } = string.Empty;
        public double Value { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class LineChartDataDto
    {
        public string Label { get; set; } = string.Empty;
        public List<double> Data { get; set; } = new List<double>();
        public string BorderColor { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
    }

    public class ScatterChartDataDto
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
