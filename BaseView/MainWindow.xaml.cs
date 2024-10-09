using BaseViewModel;
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

namespace BaseView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PollutionVm _context;
        //private readonly AddMonitoringVm _addMonitoringVm;
        public MainWindow()
        {
            InitializeComponent();
            _context = new PollutionVm((Application.Current as App)?.Context!);
            DataContext = _context;
        }

        private void CityCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox box || box.SelectedItem == null) return;
            _context.SelectStationID?.Execute((string)CityCB.SelectedItem);
        }

        private void PointCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox box || box.SelectedItem == null) return;
            _context.SelectPoint?.Execute((int)PointCB.SelectedItem);
        }

        private void YearCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AnalizBtn_Click(object sender, RoutedEventArgs e)
        {
            UserControlPanel.Children.Clear();
            var analysUC = new AnalysUserControl();
            UserControlPanel.Children.Add(analysUC);
        }
    }
}