using BaseViewModel;
using System.Windows;
using System.Windows.Controls;

namespace BaseView.Controls
{
    /// <summary>
    /// Логика взаимодействия для AnalysisUserControl.xaml
    /// </summary>
    public partial class AnalysisUserControl : UserControl
    {
        private readonly AnalysisViewModel _context;
        public AnalysisUserControl()
        {
            InitializeComponent();
            _context = new AnalysisViewModel((Application.Current as App)?.Context!);
            DataContext = _context;
            //test.Text = _context.CityName;
        }

        public static readonly DependencyProperty FirstReceivedValueProperty =
        DependencyProperty.Register("FirstReceivedValue", typeof(string), typeof(AnalysisUserControl), new PropertyMetadata(string.Empty));

        public string FirstReceivedValue
        {
            get => (string)GetValue(FirstReceivedValueProperty);
            set => SetValue(FirstReceivedValueProperty, value);
        }
    }
}
