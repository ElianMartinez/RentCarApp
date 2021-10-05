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
    /// Lógica de interacción para TipoCombustible.xaml
    /// </summary>
    public partial class TipoCombustible : UserControl
    {
        bool _IsEditing = false;
        int _ID = 0;
        public TipoCombustible()
        {
            InitializeComponent();
            Cancell();
            Refresh();
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

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Cancell();
        }

        private void Refresh()
        {
            using (Models.rentcarEntities db = new Models.rentcarEntities())
            {
                var data = db.TIPOS_COMBUSTIBLES.ToList();
                datagrid.ItemsSource = data;
                BtnCancelar.Visibility = Visibility.Hidden;
                BtnBorrar.Visibility = Visibility.Hidden;
            }
        }

        private void BtnGuardad_Click(object sender, RoutedEventArgs e)
        {
            if (_IsEditing)
            {
                if (txtnombre.Text.Length > 0 && cbxestado.SelectedValue != null)
                {
                    using (var db = new Models.rentcarEntities())
                    {
                        var result = db.TIPOS_COMBUSTIBLES.First(a => a.ID_TIPO_COMBUSTIBLE == _ID);
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
                        Models.TIPOS_COMBUSTIBLES newMarca = new Models.TIPOS_COMBUSTIBLES();
                        newMarca.DESCRIPCION = txtnombre.Text;
                        newMarca.ESTADO = cbxestado.SelectedValue.ToString();
                        db.TIPOS_COMBUSTIBLES.Add(newMarca);
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
                        var result = db.TIPOS_COMBUSTIBLES.First(a => a.ID_TIPO_COMBUSTIBLE == _ID);
                        db.TIPOS_COMBUSTIBLES.Remove(result);
                        db.SaveChanges();
                    }
                    Cancell();
                    Refresh();
                }
                catch (Exception err)
                {
                    if (err.HResult == -2146233087)
                    {
                        MessageBox.Show("No se puede borrar este registro.", "Registro tiene referencias", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (datagrid.SelectedItem != null)
            {
                Models.TIPOS_COMBUSTIBLES da = (Models.TIPOS_COMBUSTIBLES)datagrid.SelectedItem;
                txtnombre.Text = da.DESCRIPCION;
                cbxestado.SelectedValue = da.ESTADO.ToString();
                _ID = da.ID_TIPO_COMBUSTIBLE;
                _IsEditing = true;
                BtnGuardad.Content = "Modificar";
                BtnGuardad.Background = new SolidColorBrush(Colors.Orange);
                BtnCancelar.Visibility = Visibility.Visible;
                BtnBorrar.Visibility = Visibility.Visible;
            }
        }
    }


}
