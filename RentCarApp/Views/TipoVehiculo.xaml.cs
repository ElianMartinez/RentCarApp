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
    /// Lógica de interacción para TipoVehiculo.xaml
    /// </summary>
    public partial class TipoVehiculo : UserControl
    {
        private int _ID = 0;
        private bool _IsEditing = false;
        public TipoVehiculo()
        {
            InitializeComponent();
            Refresh();
            Cancell();
        }

        private void Refresh()
        {
            using (Models.rentcarEntities db = new Models.rentcarEntities())
            {
                var data = db.TIPOS_VEHICULOS.ToList();
                datagrid.ItemsSource = data;
                BtnCancelar.Visibility = Visibility.Hidden;
                BtnBorrar.Visibility = Visibility.Hidden;
            }
        }

        private void Cancell()
        {
            _ID = 0;
            _IsEditing = false;
            BtnCancelar.Visibility = Visibility.Hidden;
            BtnBorrar.Visibility = Visibility.Hidden;
            txtnombre.Text = "";
            cbxestado.SelectedValue = "";
            BtnGuardad.Background = new SolidColorBrush(Colors.Green);
            BtnGuardad.Content = "Guardar";
        }

        private void BtnGuardad_Click(object sender, RoutedEventArgs e)
        {
            if (_IsEditing)
            {
                if (txtnombre.Text.Length > 0 && cbxestado.SelectedValue != null)
                {
                    using (var db = new Models.rentcarEntities())
                    {

                        var result = db.TIPOS_VEHICULOS.First(a => a.ID_TIPO_VEHICULO == _ID);
                        result.ESTADO = cbxestado.SelectedValue.ToString();
                        result.DESCRIPCION = txtnombre.Text;
                        db.SaveChanges();
                    }
                    Cancell();
                    Refresh();

                }
                else
                {
                    MessageBox.Show("Los campos están mal...");
                }
            }
            else
            {
                if (txtnombre.Text.Length > 0 && cbxestado.SelectedValue != null)
                {

                    using (var db = new Models.rentcarEntities())
                    {
                        Models.TIPOS_VEHICULOS newMarca = new Models.TIPOS_VEHICULOS();
                        newMarca.DESCRIPCION = txtnombre.Text;
                        newMarca.ESTADO = cbxestado.SelectedValue.ToString();
                        db.TIPOS_VEHICULOS.Add(newMarca);
                        db.SaveChanges();
                    }
                    Cancell();
                    Refresh();
                }
                else
                {
                    MessageBox.Show("Los campos están mal...");
                }
            }
        }

        private void BtnBorrar_Click(object sender, RoutedEventArgs e)
        {


            MessageBoxResult result1 = MessageBox.Show("Está seguro de borrar este registro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result1 == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new Models.rentcarEntities())
                    {
                        var result = db.TIPOS_VEHICULOS.First(a => a.ID_TIPO_VEHICULO == _ID);
                        db.TIPOS_VEHICULOS.Remove(result);
                        db.SaveChanges();
                    }
                    Cancell();
                    Refresh();
                }
                catch (Exception err)
                {
                    if (err.HResult == -2146233087)
                    {
                        MessageBox.Show("No se puede borrar este registro", "Registro tiene referencias", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Cancell();
        }

        private void datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (datagrid.SelectedItem != null)
            {
                Models.TIPOS_VEHICULOS da = (Models.TIPOS_VEHICULOS)datagrid.SelectedItem;
                txtnombre.Text = da.DESCRIPCION;
                cbxestado.SelectedValue = da.ESTADO.ToString();
                _ID = da.ID_TIPO_VEHICULO;
                _IsEditing = true;
                BtnGuardad.Content = "Modificar";
                BtnGuardad.Background = new SolidColorBrush(Colors.Orange);
                BtnCancelar.Visibility = Visibility.Visible;
                BtnBorrar.Visibility = Visibility.Visible;
            }
        }
    }
}
