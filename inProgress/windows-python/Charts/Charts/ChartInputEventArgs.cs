namespace Charts
{
    public class ChartInputEventArgs : EventArgs
    {
        public double[] XValues { get; }
        public double[] YValues { get; }

        public ChartInputEventArgs(double[] x, double[] y)
        {
            XValues = x;
            YValues = y;
        }
    }
}
