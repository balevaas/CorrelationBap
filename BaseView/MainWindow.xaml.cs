using BaseViewModel;
using Microsoft.EntityFrameworkCore;
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
        private readonly SelectDataViewModel _context;
        public MainWindow()
        {
            InitializeComponent();
            _context = new SelectDataViewModel((Application.Current as App)?.Context!);
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.DataContext is Season selectedSeason)
            {
                var viewModel = DataContext as SelectDataViewModel;
                viewModel.SelectedSeason = selectedSeason;
            }
        }
    }
}