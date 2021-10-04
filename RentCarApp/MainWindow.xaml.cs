using RentCarApp.Views;
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

namespace RentCarApp
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public  partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            using (Models.rentcarEntities db = new Models.rentcarEntities())
            {
                int cantidadVehiculos = db.VEHICULOS.Count();
            }
        }

        public void ChangeContext(object valor)
        {
            DataContext = valor;
        }

        private void TBShow(object sender, RoutedEventArgs e)
        {
            GridContent.Opacity = 0.5;


        }

        private void TBHide(object sender, RoutedEventArgs e)
        {
            GridContent.Opacity = 1;

        }

        private void PreviewMouseBtnDown(object sender, MouseButtonEventArgs e)
        {
            BtnShowHide.IsChecked = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void home_click(object sender, RoutedEventArgs e)
        {
            var a = new Home();
            ChangeContext(a);
        }

        private void BtnMant_Click(object sender, RoutedEventArgs e)
        {
            var a = new Mantenimiento(this);
            ChangeContext(a);
        }
    }
}
