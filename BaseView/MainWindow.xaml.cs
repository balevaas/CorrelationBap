
using BaseView.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace BaseView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CorrelationViewModel _context;
        
        public MainWindow()
        {
            InitializeComponent();
            _context = new CorrelationViewModel((Application.Current as App)?.Context!);
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
            _context.SelectPointID?.Execute((int)PointCB.SelectedItem);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _context.UpdateSelection();
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void AnalysisBtn_Click(object sender, RoutedEventArgs e)
        {
            _context.SelectPollution();
        }
    }
}