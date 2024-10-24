using BaseViewModel;
using BaseViewModel.DatasDTO;
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
        private readonly SelectPointYearsViewModel _context;
        public MainWindow()
        {
            InitializeComponent();
            _context = new SelectPointYearsViewModel((Application.Current as App)?.Context!);
            DataContext = _context;
            _context.FirstValue = "Hello";
        }
        private void CityCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox box || box.SelectedItem == null) return;
            _context.SelectStationID?.Execute((string)CityCB.SelectedItem);
        }

        private void PointCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox box || box.SelectedItem == null) return;
            _context.SelectPointID?.Execute((int)PointCB.SelectedItem);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _context.UpdateSelection();
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }
    }
}