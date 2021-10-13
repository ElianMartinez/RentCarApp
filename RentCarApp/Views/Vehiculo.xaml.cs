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
    /// Lógica de interacción para Vehiculo.xaml
    /// </summary>
    public partial class Vehiculo : UserControl
    {
        int _ID = 0;
        bool _IsEditing = false;
        public Vehiculo()
        {
            InitializeComponent();
            Refresh();
        }

        private void txtcomision_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Refresh()
        {
            using (var db = new Models.rentcarEntities())
            {
                var rows = db.VEHICULOS.ToList();
                datagrid.ItemsSource = rows; 

                var marcas = db.MARCAS.Where(o => o.ESTADO == "A").ToList();
                cbxmarca.ItemsSource = marcas;
                cbxmarca.SelectedValuePath = "ID_MARCA";
                cbxmarca.DisplayMemberPath = "DESCRIPCION";

                var tipoVehiculos = db.TIPOS_VEHICULOS.Where(o => o.ESTADO == "A").ToList();
                cbxtipoVehi.ItemsSource = tipoVehiculos;
                cbxtipoVehi.SelectedValuePath = "ID_TIPO_VEHICULO";
                cbxtipoVehi.DisplayMemberPath = "DESCRIPCION";
                cbxtipoVehi.SelectedIndex = 0;

            }
        }

        private void Cancell()
        {
            _ID = 0;
            _IsEditing = false;
            btnCancelar.Visibility = Visibility.Hidden;
            btnBorrar.Visibility = Visibility.Hidden;
            txtDescripcion.Text = "";
            cbxestado.SelectedValue = "";
            txtnoMotor.Text = "";
            txtchasis.Text = "";
            cbxtipoVehi.SelectedValue = null;
            cbxmarca.SelectedValue = null;
            txtmatricula.Text = "";
            cbxModelos.SelectedValue = null;
            btnGuardar.Background = new SolidColorBrush(Colors.Green);
            btnGuardar.Content = "Guardar";
        }


        private void cbxmarca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var db = new Models.rentcarEntities())
            {

                var modelos = db.MODELOS.Where(item => item.ID_MARCA == (int)cbxmarca.SelectedValue && item.ESTADO == "A").ToList();
                cbxModelos.ItemsSource = modelos;
                cbxModelos.SelectedValuePath = "ID_MODELO";
                cbxModelos.DisplayMemberPath = "DESCRIPCION";
                cbxModelos.SelectedIndex = 0;

            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Cancell();
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
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
                        MessageBox.Show("No se puede borrar este registro.", "Registro tiene referencias", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (_IsEditing)
            {
                if (txtDescripcion.Text.Length > 0 &&
                    cbxestado.SelectedValue != null &&
                    txtmatricula.Text.Length > 0 &&
                    txtnoMotor.Text.Length > 0 &&
                    txtchasis.Text.Length > 0 &&
                    cbxmarca.SelectedValue != null &&
                    cbxModelos.SelectedValue != null &&
                    cbxtipoVehi.SelectedValue != null &&
                    cbxtipocomb.SelectedValue != null
                    )
                {
                    using (var db = new Models.rentcarEntities())
                    {
                        var result = db.VEHICULOS.First(a => a.ID_TIPO_VEHICULO == _ID);
                        result.DESCRIPCION = txtDescripcion.Text;
                        result.ID_MARCA = (int)cbxmarca.SelectedValue;
                        result.ID_MODELO = (int)cbxModelos.SelectedValue;
                        result.ESTADO = cbxestado.SelectedValue.ToString();
                        result.ID_TIPO_COMNUSTIBLE = (int)cbxtipocomb.SelectedValue;
                        result.ID_TIPO_VEHICULO = (int)cbxtipoVehi.SelectedValue;
                        
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
                if (txtDescripcion.Text.Length > 0 &&
                    cbxestado.SelectedValue != null &&
                    txtmatricula.Text.Length > 0 &&
                    txtnoMotor.Text.Length > 0 &&
                    txtchasis.Text.Length > 0 &&
                    cbxmarca.SelectedValue != null &&
                    cbxModelos.SelectedValue != null &&
                    cbxtipoVehi.SelectedValue != null &&
                    cbxtipocomb.SelectedValue != null
                    )
                {
                    using (var db = new Models.rentcarEntities())
                    {
                        try
                        {
                            Models.EMPLEADO newMarca = new Models.EMPLEADO();
                            //newMarca.NOMBRE = txtnombre.Text;/
                            //newMarca.PORCIENTO_COMISION = double.Parse(txtcomision.Text);
                            //newMarca.TANDA_LABORAR = cbxtandal.Text;
                            ////newMarca.CEDULA = txtcedula.Text;
                            //newMarca.FECHA_INGRESO = (DateTime)dpkFecha.SelectedDate;
                            newMarca.ESTADO = cbxestado.SelectedValue.ToString();
                            db.EMPLEADO.Add(newMarca);
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
