using JN = Javonet.Netcore.Sdk;

namespace Charts
{
    public partial class MainForm : Form
    {
        private readonly ChartInputPanel _inputPanel;
        private readonly ChartDisplayPanel _chartDisplay;
        private readonly JN.RuntimeContext _pythonRuntime;

        public MainForm()
        {
            Text = "Python Chart Generator via Javonet";
            Size = new Size(800, 600);

            _inputPanel = new ChartInputPanel { Dock = DockStyle.Left, Width = 250 };
            _chartDisplay = new ChartDisplayPanel { Dock = DockStyle.Fill };

            _inputPanel.GenerateChartClicked += OnGenerateChart;

            Controls.Add(_chartDisplay);
            Controls.Add(_inputPanel);

            JN.Javonet.Activate("n9B5-Km7g-Pp69-j9FE-e9A5");
            _pythonRuntime = JN.Javonet.InMemory().Python();

            var path = @"C:\coding\articles\inProgress\windows-python\python";

            _pythonRuntime.LoadLibrary(path);

            InitializeComponent();
        }

        private void OnGenerateChart(object sender, ChartInputEventArgs e)
        {
            try
            {
                if (e.XValues.Length != e.YValues.Length)
                {
                    MessageBox.Show("X and Y must have the same number of elements.");
                    return;
                }

                var instance = _pythonRuntime.GetType("charts.Charts").CreateInstance();

                var x = e.XValues;
                var y = e.YValues;
                
                var svg = instance.InvokeInstanceMethod("generate_line_chart_svg", "Demo Chart", x, y).Execute();
                var result = (string)svg.GetValue();
                
                _chartDisplay.DisplayImage(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
