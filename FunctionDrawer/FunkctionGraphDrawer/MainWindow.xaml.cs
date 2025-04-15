using System;
using System.Text.RegularExpressions;
using System.Windows;
using NCalc;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace FunkctionGraphDrawer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FunctionInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FunctionInput.Text == "Enter function (e.g., x + 2)")
            {
                FunctionInput.Text = "";
            }
        }

        private void FunctionInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FunctionInput.Text))
            {
                FunctionInput.Text = "Enter function (e.g., x + 2)";
            }
        }

        private double EvaluateFunction(string function, double x, double y)
{
    try
    {
        // Replace '^' with Pow
        function = Regex.Replace(function, @"(\b[\w.]+)\s*\^\s*([\w.]+)", "Pow($1, $2)");

        // Normalize trigonometric functions to NCalc format (capitalized)
        function = Regex.Replace(function, @"\bsin\b", "Sin", RegexOptions.IgnoreCase);
        function = Regex.Replace(function, @"\bcos\b", "Cos", RegexOptions.IgnoreCase);
        function = Regex.Replace(function, @"\btan\b", "Tan", RegexOptions.IgnoreCase);
        function = Regex.Replace(function, @"\bsqrt\b", "Sqrt", RegexOptions.IgnoreCase);
        function = Regex.Replace(function, @"\bln\b", "Log", RegexOptions.IgnoreCase);
        function = Regex.Replace(function, @"\blog\b", "Log10", RegexOptions.IgnoreCase);

        NCalc.Expression expr = new NCalc.Expression(function);
        expr.Parameters["x"] = x;
        expr.Parameters["y"] = y;

        return Convert.ToDouble(expr.Evaluate());
    }
    catch
    {
        throw new Exception("Invalid function format.");
    }
}

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string function = FunctionInput.Text;
                double y = 0;

                // Try parsing Y, if provided
                double.TryParse(YInput.Text, out y);

                // Evaluate result for display
                double testResult = EvaluateFunction(function, 1, y); // example x=1
                ResultLabel.Content = $"Sample Result (x=1): {testResult}";

                PlotFunction(function, y);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid input! " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PlotFunction(string function, double y)
        {
            var model = new PlotModel { Title = $"Graph of: {function}" };

            // X axis
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "x",
                Minimum = -10,
                Maximum = 10,
                PositionAtZeroCrossing = true,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Black
            });

            // Y axis
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "f(x)",
                Minimum = -10,
                Maximum = 10,
                PositionAtZeroCrossing = true,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Black
            });

            var series = new LineSeries
            {
                Title = "f(x)",
                StrokeThickness = 2,
                MarkerSize = 0,
                Color = OxyColors.SkyBlue
            };

            for (double x = -10; x <= 10; x += 0.1)
            {
                try
                {
                    double result = EvaluateFunction(function, x, y);
                    if (double.IsFinite(result))
                    {
                        series.Points.Add(new DataPoint(x, result));
                    }
                }
                catch
                {
                    // skip invalid points
                }
            }

            model.Series.Add(series);
            PlotView.Model = model;




        }

    }
}


