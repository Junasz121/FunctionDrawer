using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NCalc;
using System.Text.RegularExpressions;

namespace FunkctionGraphDrawer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
/// 


public partial class MainWindow : Window
{
    private void FunctionInput_GotFocus(object sender, RoutedEventArgs e)
    {
        if (FunctionInput.Text == "Enter function (e.g., x*y + 2)")
        {
            FunctionInput.Text = "";
        }
    }

    private void FunctionInput_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FunctionInput.Text))
        {
            FunctionInput.Text = "Enter function (e.g., x*y + 2)";
        }
    }

    public MainWindow()
    {
        InitializeComponent();
    }

    private double CalculateFunction(string function, double x, double y)
    {
        try
        {
            // Replace ^ with Pow()
            function = Regex.Replace(function, @"(\w+)\^(\w+)", "Pow($1, $2)");

            NCalc.Expression expression = new NCalc.Expression(function);
            expression.Parameters["x"] = x;
            expression.Parameters["y"] = y;
            return Convert.ToDouble(expression.Evaluate());
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
            double x = double.Parse(XInput.Text);
            double y = double.Parse(YInput.Text);
            double result = CalculateFunction(function, x, y);
            ResultLabel.Content = $"Result: {result}";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Invalid input! " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}


