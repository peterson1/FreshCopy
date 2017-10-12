using Autofac;
using CommonTools.Lib.ns11.SignalRClients;
using System.Windows;

namespace FreshCopy.ServerControl.WPF.ConfigEditors
{
    public partial class CfgEditorWindow1 : Window
    {
        public CfgEditorWindow1()
        {
            InitializeComponent();
        }


        internal static void Show(HubClientSession session)
        {
            var win = new CfgEditorWindow1();
            var vm  = Components.Scope.Resolve<ConfigEditorVM>();
            win.DataContext = vm;
            vm.Load(session);
            win.Show();
        }
    }
}
