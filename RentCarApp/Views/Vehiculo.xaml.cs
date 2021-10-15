using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            Cancell();
        }

        private void txtcomision_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async Task Refresh()
        {
            using (var db = new Models.rentcarEntities())
            {
                var qs = await (from c in db.VEHICULOS
                                join m in db.MARCAS on c.ID_MARCA equals m.ID_MARCA
                                join mo in db.MODELOS on c.ID_MODELO equals mo.ID_MODELO
                                join ticom in db.TIPOS_COMBUSTIBLES on c.ID_TIPO_COMNUSTIBLE equals ticom.ID_TIPO_COMBUSTIBLE
                                join tipovehi in db.TIPOS_VEHICULOS on c.ID_TIPO_VEHICULO equals tipovehi.ID_TIPO_VEHICULO
                                select new VEHICULOSDOT()
                                {
                                    DESCRIPCION = c.DESCRIPCION,
                                    ESTADO = c.ESTADO,
                                    MARCAS = m,
                                    ID_MODELO = c.ID_MODELO,
                                    MODELOS = mo,
                                    ID_TIPO_COMNUSTIBLE = c.ID_TIPO_COMNUSTIBLE,
                                    TIPOS_COMBUSTIBLES = ticom,
                                    ID_TIPO_VEHICULO = c.ID_TIPO_VEHICULO,
                                    TIPOS_VEHICULOS = tipovehi,
                                    ID_MARCA = c.ID_MARCA,
                                    NO_CHASIS = c.NO_CHASIS,
                                    NO_MOTOR = c.NO_MOTOR,
                                    NO_PLACA = c.NO_PLACA,
                                    ID_VEHICULO = c.ID_VEHICULO,

                                }
                              ).ToListAsync();

                datagrid.ItemsSource = qs;

                var marcas = db.MARCAS.Where(o => o.ESTADO == "A").ToList();
                cbxmarca.ItemsSource = marcas;
                cbxmarca.SelectedValuePath = "ID_MARCA";
                cbxmarca.DisplayMemberPath = "DESCRIPCION";
                var tipoVehiculos = db.TIPOS_VEHICULOS.Where(o => o.ESTADO == "A").ToList();
                cbxtipoVehi.ItemsSource = tipoVehiculos;
                cbxtipoVehi.SelectedValuePath = "ID_TIPO_VEHICULO";
                cbxtipoVehi.DisplayMemberPath = "DESCRIPCION";
                cbxtipoVehi.SelectedIndex = 0;

                cbxtipocomb.ItemsSource = null;
                var tipoCom = db.TIPOS_COMBUSTIBLES.Where(o => o.ESTADO == "A").ToList();
                cbxtipocomb.ItemsSource = tipoCom;
                cbxtipocomb.SelectedValuePath = "ID_TIPO_COMBUSTIBLE";
                cbxtipocomb.DisplayMemberPath = "DESCRIPCION";
                cbxtipocomb.SelectedIndex = 0;

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
            txtmatricula.Text = "";
            cbxModelos.SelectedValue = null;
            btnGuardar.Background = new SolidColorBrush(Colors.Green);
            btnGuardar.Content = "Guardar";
        }


        private void cbxmarca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbxmarca.SelectedValue != null)
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
           
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Cancell();
        }

        private async void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = MessageBox.Show("Está seguro de borrar este registro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result1 == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new Models.rentcarEntities())
                    {
                        var result = db.VEHICULOS.First(a => a.ID_VEHICULO == _ID);
                        db.VEHICULOS.Remove(result);
                        db.SaveChanges();
                    }
                    Cancell();
                   await Refresh();
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

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
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
                        var newMarca = db.VEHICULOS.First(a => a.ID_VEHICULO == _ID);
                        newMarca.ESTADO = cbxestado.SelectedValue.ToString();
                        newMarca.DESCRIPCION = txtDescripcion.Text;
                        newMarca.ID_MARCA = (int)cbxmarca.SelectedValue;
                        newMarca.ID_MODELO = (int)cbxModelos.SelectedValue;
                        newMarca.ID_TIPO_COMNUSTIBLE = (int)cbxtipocomb.SelectedValue;
                        newMarca.ID_TIPO_VEHICULO = (int)cbxtipoVehi.SelectedValue;
                        newMarca.NO_CHASIS = txtchasis.Text;
                        newMarca.NO_MOTOR = txtnoMotor.Text;
                        newMarca.NO_PLACA = txtmatricula.Text;
                        
                        db.SaveChanges();
                    }
                    Cancell();
                   await Refresh();
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
                            Models.VEHICULOS newMarca = new Models.VEHICULOS();
                            newMarca.ESTADO = cbxestado.SelectedValue.ToString();
                            newMarca.DESCRIPCION = txtDescripcion.Text;
                            newMarca.ID_MARCA = (int)cbxmarca.SelectedValue;
                            newMarca.ID_MODELO = (int)cbxModelos.SelectedValue;
                            newMarca.ID_TIPO_COMNUSTIBLE = (int)cbxtipocomb.SelectedValue;
                            newMarca.ID_TIPO_VEHICULO = (int)cbxtipoVehi.SelectedValue;
                            newMarca.NO_CHASIS = txtchasis.Text;
                            newMarca.NO_MOTOR = txtnoMotor.Text;
                            newMarca.NO_PLACA = txtmatricula.Text;
                            db.VEHICULOS.Add(newMarca);
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
                   await Refresh();
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
                Models.VEHICULOS da = (Models.VEHICULOS)datagrid.SelectedItem;

                txtDescripcion.Text = da.DESCRIPCION;
                txtmatricula.Text = da.NO_PLACA;
                txtnoMotor.Text = da.NO_MOTOR;
                txtchasis.Text = da.NO_CHASIS;
                cbxmarca.SelectedValue = da.ID_MARCA;
                cbxModelos.SelectedValue = da.ID_MODELO;
                cbxtipocomb.SelectedValue = da.ID_TIPO_COMNUSTIBLE;
                cbxtipoVehi.SelectedValue = da.ID_TIPO_VEHICULO;
                cbxestado.SelectedValue = da.ESTADO;
                _ID = da.ID_VEHICULO;
                _IsEditing = true;
                btnGuardar.Content = "Modificar";
                btnGuardar.Background = new SolidColorBrush(Colors.Orange);
                btnCancelar.Visibility = Visibility.Visible;
                btnBorrar.Visibility = Visibility.Visible;
            }
        }


        private async void Txtchasis_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            string valor = txtchasis_Copy.Text;
            
            if (valor != String.Empty)
            {
                using (var db = new Models.rentcarEntities())
                {
                    var qs = await (from c in db.VEHICULOS
                              join m in db.MARCAS on c.ID_MARCA equals m.ID_MARCA
                              join mo in db.MODELOS on c.ID_MODELO equals mo.ID_MODELO
                              join ticom in db.TIPOS_COMBUSTIBLES on c.ID_TIPO_COMNUSTIBLE equals ticom.ID_TIPO_COMBUSTIBLE
                              join tipovehi in db.TIPOS_VEHICULOS on c.ID_TIPO_VEHICULO equals tipovehi.ID_TIPO_VEHICULO
                              where c.DESCRIPCION.Contains(valor) || m.DESCRIPCION.Contains(valor) || mo.DESCRIPCION.Contains(valor) || c.ESTADO.Contains(valor) ||
                              tipovehi.DESCRIPCION.Contains(valor)
                              select new VEHICULOSDOT()
                              {
                                  DESCRIPCION = c.DESCRIPCION,
                                  ESTADO = c.ESTADO,
                                  MARCAS = m,
                                  ID_MODELO = c.ID_MODELO,
                                  MODELOS = mo,
                                  ID_TIPO_COMNUSTIBLE = c.ID_TIPO_COMNUSTIBLE,
                                  TIPOS_COMBUSTIBLES = ticom,
                                  ID_TIPO_VEHICULO = c.ID_TIPO_VEHICULO,
                                  TIPOS_VEHICULOS = tipovehi,
                                  ID_MARCA = c.ID_MARCA,
                                  NO_CHASIS = c.NO_CHASIS,
                                  NO_MOTOR = c.NO_MOTOR,
                                  NO_PLACA = c.NO_PLACA,
                                  ID_VEHICULO = c.ID_VEHICULO,
                              
                              }
                              ).ToListAsync();

                    datagrid.ItemsSource = qs;
                }
            }
            else
            {
                await Refresh();
            }
        }

       

      
    }
    public class VEHICULOSDOT : Models.VEHICULOS { }


}
