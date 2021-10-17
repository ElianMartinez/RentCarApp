using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    /// Lógica de interacción para Clientes.xaml
    /// </summary>
    public partial class Clientes : UserControl
    {
        bool _IsEditing = false;
        int _ID = 0;
        public Clientes()
        {
            InitializeComponent();
            Refresh();
            Cancell();
        }

        private void Refresh()
        {
            using (var db = new Models.rentcarEntities())
            {
                var rows = db.CLIENTES.ToList();
                datagrid.ItemsSource = rows;
            }
        }

        private void Cancell()
        {
            _ID = 0;
            _IsEditing = false;
            btnCancelar.Visibility = Visibility.Hidden;
            btnBorrar.Visibility = Visibility.Hidden;
            txtnombre.Text = "";
            cbxestado.SelectedValue = "";
            txtcedula.Text = "";
            cbxTipoPersona.SelectedValue = null;
            txtNoTarjeta.Text = "";
            txtLimiteC.Text = null;
            btnGuardar.Background = new SolidColorBrush(Colors.Green);
            btnGuardar.Content = "Guardar";
        }

        private void datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (datagrid.SelectedItem != null)
            {
                Models.CLIENTES da = (Models.CLIENTES)datagrid.SelectedItem;
                txtnombre.Text = da.NOMBRE;
                txtcedula.Text = da.CEDULA;
                cbxestado.SelectedValue = da.ESTADO.ToString();
                cbxTipoPersona.Text = da.TIPO_PERSONA;
                cbxestado.SelectedValue = da.ESTADO;
                txtLimiteC.Text = da.LIMITE_CREDITO.ToString();
                txtNoTarjeta.Text = da.NO_TARJETA_CR;
                _ID = da.ID_CLIENTE;
                _IsEditing = true;
                btnGuardar.Content = "Modificar";
                btnGuardar.Background = new SolidColorBrush(Colors.Orange);
                btnCancelar.Visibility = Visibility.Visible;
                btnBorrar.Visibility = Visibility.Visible;
            }

        }

        private void btnborrar_click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = MessageBox.Show("Está seguro de borrar este registro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result1 == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new Models.rentcarEntities())
                    {
                        var result = db.CLIENTES.First(a => a.ID_CLIENTE == _ID);
                        db.CLIENTES.Remove(result);
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

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Cancell();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_IsEditing)
            {
                if (txtnombre.Text.Length > 0 &&
                    cbxestado.SelectedValue != null &&
                    txtcedula.Text.Length > 0 &&
                    txtLimiteC.Text.Length > 0 &&
                    txtcedula.Text.Length == 11 &&
                    txtNoTarjeta.Text != null &&
                    cbxTipoPersona.SelectedValue != null)
                {
                    using (var db = new Models.rentcarEntities())
                    {
                        var result = db.CLIENTES.First(a => a.ID_CLIENTE == _ID);
                        result.NOMBRE = txtnombre.Text;
                        result.CEDULA = txtcedula.Text;
                        result.ESTADO = cbxestado.SelectedValue.ToString();
                        result.LIMITE_CREDITO = decimal.Parse(txtLimiteC.Text);
                        result.NO_TARJETA_CR = txtNoTarjeta.Text;
                        result.TIPO_PERSONA = cbxTipoPersona.Text;
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
                if (txtnombre.Text.Length > 0 &&
                    cbxestado.SelectedValue != null &&
                    txtcedula.Text.Length > 0 &&
                    txtcedula.Text.Length == 11 &&
                    txtLimiteC.Text.Length > 0 &&
                    txtNoTarjeta.Text != null &&
                  decimal.Parse(txtLimiteC.Text) > 0 &&
                    cbxTipoPersona.SelectedValue != null)
                {

                    using (var db = new Models.rentcarEntities())
                    {
                        try
                        {
                            Models.CLIENTES newMarca = new Models.CLIENTES
                            {
                                NOMBRE = txtnombre.Text,
                                CEDULA = txtcedula.Text,
                                ESTADO = cbxestado.SelectedValue.ToString(),
                                LIMITE_CREDITO = decimal.Parse(txtLimiteC.Text),
                                NO_TARJETA_CR = txtNoTarjeta.Text,
                                TIPO_PERSONA = cbxTipoPersona.Text
                            };
                            db.CLIENTES.Add(newMarca);
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException err)
                        {
                            foreach (var eve in err.EntityValidationErrors)
                            {
                                MessageBox.Show("Entity of type " + eve.Entry.Entity.GetType().Name + "in state " + eve.Entry.State + " has the following validation errors:");
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    MessageBox.Show("- Property: " + ve.PropertyName + " , Error: " + ve.ErrorMessage);
                                }
                            }
                        }
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
    }
}
