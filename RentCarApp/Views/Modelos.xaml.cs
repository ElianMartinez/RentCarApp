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
    /// Lógica de interacción para Modelos.xaml
    /// </summary>
    public partial class Modelos : UserControl
    {
        bool _IsEditing = false;
        int _ID = 0;
        public Modelos()
        {
            InitializeComponent();
            Refresh();
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
        private void Refresh()
        {
            cbxmarcas.ItemsSource = null;
            using (var db = new Models.rentcarEntities())
            {
                var marcas = db.MARCAS.Where(a => a.ESTADO == "A").ToList();
                var rows = db.MODELOS.ToList();
                datagrid.ItemsSource = rows;
                cbxmarcas.ItemsSource = marcas;
                cbxmarcas.SelectedValuePath = "ID_MARCA";
                cbxmarcas.DisplayMemberPath = "DESCRIPCION";


            }
        }

        private void cbxmarcas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void BtnGuardad_Click(object sender, RoutedEventArgs e)
        {
            if (_IsEditing)
            {
                if (txtnombre.Text.Length > 0 && cbxestado.SelectedValue != null && cbxmarcas.SelectedValue != null)
                {
                    using (var db = new Models.rentcarEntities())
                    {
                        var result = db.MODELOS.First(a => a.ID_MODELO == _ID);
                        result.ESTADO = cbxestado.SelectedValue.ToString();
                        result.DESCRIPCION = txtnombre.Text;
                        result.ID_MARCA = (int)cbxmarcas.SelectedValue;
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
                        Models.MODELOS newModelo = new Models.MODELOS();
                        newModelo.DESCRIPCION = txtnombre.Text;
                        newModelo.ESTADO = cbxestado.SelectedValue.ToString();
                        newModelo.ID_MARCA = (int)cbxmarcas.SelectedValue;
                        db.MODELOS.Add(newModelo);
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

        private void datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (datagrid.SelectedItem != null)
            {
                Models.MODELOS da = (Models.MODELOS)datagrid.SelectedItem;
                txtnombre.Text = da.DESCRIPCION;
                cbxestado.SelectedValue = da.ESTADO.ToString();
                cbxmarcas.SelectedValue = da.ID_MARCA;
                _ID = da.ID_MODELO;
                _IsEditing = true;
                BtnGuardad.Content = "Modificar";
                BtnGuardad.Background = new SolidColorBrush(Colors.Orange);
                BtnCancelar.Visibility = Visibility.Visible;
                BtnBorrar.Visibility = Visibility.Visible;
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Cancell();
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
                        var result = db.MODELOS.First(a => a.ID_MODELO == _ID);
                        db.MODELOS.Remove(result);
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
    }
}
