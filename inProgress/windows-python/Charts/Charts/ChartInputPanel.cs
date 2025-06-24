namespace Charts
{
    public class ChartInputPanel : Panel
    {
        private readonly TextBox inputX;
        private readonly TextBox inputY;
        private readonly Button generateButton;

        public event EventHandler<ChartInputEventArgs> GenerateChartClicked;

        public ChartInputPanel()
        {
            Padding = new Padding(10);

            generateButton = new Button { Text = "Generate Chart", Dock = DockStyle.Top, Height = 40 };
            generateButton.Click += (s, e) => OnGenerateChartClicked();
            Controls.Add(generateButton);

            Controls.Add(new Label { Text = "Values Y (e.g. 10,20,30)", Dock = DockStyle.Top });
            inputY = new TextBox { Dock = DockStyle.Top };
            Controls.Add(inputY);

            Controls.Add(new Label { Text = "Values X (e.g. 1,2,3)", Dock = DockStyle.Top });
            inputX = new TextBox { Dock = DockStyle.Top };
            Controls.Add(inputX);
        }

        private void OnGenerateChartClicked()
        {
            try
            {
                string[] xStr = inputX.Text.Split(',');
                string[] yStr = inputY.Text.Split(',');

                double[] x = Array.ConvertAll(xStr, double.Parse);
                double[] y = Array.ConvertAll(yStr, double.Parse);

                GenerateChartClicked?.Invoke(this, new ChartInputEventArgs(x, y));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Input data error: " + ex.Message);
            }
        }
    }
}
