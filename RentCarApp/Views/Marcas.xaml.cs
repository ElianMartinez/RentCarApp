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

namespace RentCarApp.Views
{
    /// <summary>
    /// Lógica de interacción para Marcas.xaml
    /// </summary>
    public partial class Marcas : UserControl
    {

        private int _ID = 0;
        private bool _IsEditing = false;
        public Marcas()
        {
            InitializeComponent();
            Refresh();
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Refresh()
        {
            using (Models.rentcarEntities db = new Models.rentcarEntities())
            {
                var data = db.MARCAS.ToList();
                datagrid.ItemsSource = data;
                BtnCancelar.Visibility = Visibility.Hidden;
                BtnBorrar.Visibility = Visibility.Hidden;

            }
        }

        private void datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(datagrid.SelectedItem != null)
            {
                Models.MARCAS da = (Models.MARCAS)datagrid.SelectedItem;
                txtnombre.Text = da.DESCRIPCION;
                cbxestado.SelectedValue = da.ESTADO.ToString();
                _ID = da.ID_MARCA;
                _IsEditing = true;
                BtnGuardad.Content = "Modificar";
                BtnGuardad.Background = new SolidColorBrush(Colors.Orange);
                BtnCancelar.Visibility = Visibility.Visible;
                BtnBorrar.Visibility = Visibility.Visible;

            }


        }

        private void cbxestado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
  
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Cancell();


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

                        var result = db.MARCAS.First(a => a.ID_MARCA == _ID);
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
                if(txtnombre.Text.Length > 0 && cbxestado.SelectedValue != null)
                {
                   
                    using (var db = new Models.rentcarEntities())
                    {
                        Models.MARCAS newMarca = new Models.MARCAS();
                        newMarca.DESCRIPCION = txtnombre.Text;
                        newMarca.ESTADO = cbxestado.SelectedValue.ToString();
                        db.MARCAS.Add(newMarca);
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
            using (var db = new Models.rentcarEntities())
            {
                var result = db.MARCAS.First(a => a.ID_MARCA == _ID);
                db.MARCAS.Remove(result);
                db.SaveChanges();
            }
            Cancell();
            Refresh();
        }
    }
}
