using System.Windows;

namespace FreshCopy.Client.Lib45.ProblemReporters
{
    public partial class ProblemReportWindow1 : Window
    {
        public ProblemReportWindow1()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                VM.CloseRequested += (c, d) => this.Close();
            };
        }

        private ProblemReporter1VM VM => DataContext as ProblemReporter1VM;
    }
}
