using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FirstTest
{
    /// <summary>
    /// Interaction logic for BarcodeInvestigating.xaml
    /// </summary>
    public partial class BarcodeInvestigating : Window
    {
        public BarcodeInvestigating()
        {
            InitializeComponent();
        }
        //Attempted to add a Scandit NuGet Package however this resulted in markup issues for every other window.
    }
}
