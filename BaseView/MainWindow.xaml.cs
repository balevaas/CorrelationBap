
using BaseView.ViewModel;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
            
        }

        private void AnalysisBtn_Click(object sender, RoutedEventArgs e)
        {
            _context.SelectPollution();
        }

        private void YearsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _context.UpdateSelection();
            if(_context.SelectedCount == 1) PanelMonth.Visibility = Visibility.Visible;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (plotView.Model == null)
            {
                MessageBox.Show("Для выбранных данных график не построен");
                return;
            }
            else SavePlotAsPng($"{_context.NameCity},№{_context.PointID}.png");
            
        }

        private void SavePlotAsPng(string pngFilePath)
        {
            var pngExporter = new PngExporter
            {
                Width = (int)graphGrid.ActualWidth, 
                Height = (int)graphGrid.ActualHeight 
            };

            var bitmap = pngExporter.ExportToBitmap(plotView.Model);

            if (bitmap != null)
            {
                using (var stream = File.Create(pngFilePath))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    encoder.Save(stream);
                }
            }
            else
            {
                MessageBox.Show("Ошибка экспорта графика в PNG.");
                return; // Прерываем выполнение функции, если bitmap null
            }
            MessageBox.Show($"График сохранен в {pngFilePath}");
        }
    }
}