using BaseView.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BaseView.Controls
{
    /// <summary>
    /// Логика взаимодействия для AnalysisUserControl.xaml
    /// </summary>
    public partial class AnalysisUserControl : UserControl
    {
        private readonly AnalysisViewModel _context;

        private double result;
        public AnalysisUserControl()
        {
            InitializeComponent();
            _context = new AnalysisViewModel((Application.Current as App)?.Context!);
            DataContext = _context;                               
            if(analysis.Visibility == Visibility.Visible)
            {
                result = _context.CalculateCorrelation();
                tb.Text = result.ToString();
            }
        }
    }
}
