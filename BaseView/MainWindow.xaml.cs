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
        private readonly PollutionViewModel _context;
        //private readonly AddMonitoringVm _addMonitoringVm;
        public MainWindow()
        {
            InitializeComponent();
            _context = new PollutionViewModel((Application.Current as App)?.Context!);
            DataContext = _context;
        }

        private void CityCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (sender is not ComboBox box || box.SelectedItem == null) return;
            //_context.SelectStationID?.Execute((string)CityCB.SelectedItem);
        }

        private void PointCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (sender is not ComboBox box || box.SelectedItem == null) return;
            //_context.SelectPoint?.Execute((int)PointCB.SelectedItem);            
        }

        private void YearCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AnalizBtn_Click(object sender, RoutedEventArgs e)
        {
            //UserControlPanel.Children.Clear();
            //var analysUC = new AnalysUserControl();
            //UserControlPanel.Children.Add(analysUC);
            //var viewModel = DataContext as PollutionViewModel;
            //var selectedMonths = viewModel?.GetSelectedMonths();
            //var selectedYears = viewModel?.GetSelectedYears();
            //if (selectedMonths != null && selectedMonths.Any())
            //{
            //    MessageBox.Show(string.Join(", ", selectedMonths), "Выбранные месяцы и года");
            //}
            //else MessageBox.Show("Нет выбранных месяцев", "Информация");

            CityNameTB.Text = _context.NameS;
        }

        private void MonthCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //if (sender is RadioButton radioButton && radioButton.DataContext is Season selectedSeason)
            //{
            //    var viewModel = DataContext as PollutionViewModel;
            //    viewModel.SelectedSeason = selectedSeason;
            //}
        }
    }
}