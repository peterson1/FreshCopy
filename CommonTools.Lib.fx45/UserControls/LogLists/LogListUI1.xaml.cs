using System.Windows;
using System.Windows.Controls;


namespace CommonTools.Lib.fx45.UserControls.LogLists
{
    public partial class LogListUI1 : UserControl
    {
        public LogListUI1()
        {
            InitializeComponent();
        }


        private void dg_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (var col in dg.Columns)
                col.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
        }
    }
}
