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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RentCarApp.Views
{
    /// <summary>
    /// Lógica de interacción para Mantenimiento.xaml
    /// </summary>
    public partial class Mantenimiento : UserControl
    {
        private MainWindow _mainWindow = null;
        public Mantenimiento(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void BtnMarcas_Click(object sender, RoutedEventArgs e)
        {
            var a = new Marcas();
            _mainWindow.ChangeContext(a);
        }

        private void BtnModelos_Click(object sender, RoutedEventArgs e)
        {
            var a = new Modelos();
            _mainWindow.ChangeContext(a);
        }

        private void BtnTipoC_Click(object sender, RoutedEventArgs e)
        {

            var a = new TipoCombustible();
            _mainWindow.ChangeContext(a);
        }

        private void BtnTipoV_Click(object sender, RoutedEventArgs e)
        {
            var a = new TipoVehiculo();
            _mainWindow.ChangeContext(a);
        }
    }
}
