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
    /// Lógica de interacción para Empleado.xaml
    /// </summary>
    public partial class Empleado : UserControl
    {
         int _ID = 0;
         bool _IsEditing = false;
        public Empleado()
        {
            InitializeComponent();
            Cancell();
            Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_IsEditing)
            {
                if (txtnombre.Text.Length > 0 && 
                    cbxestado.SelectedValue != null && 
                    txtcedula.Text.Length > 0 && 
                    txtcomision.Text.Length > 0 && 
                    cbxtandal.SelectedValue != null &&
                    dpkFecha.SelectedDate != null)
                {
                    using (var db = new Models.rentcarEntities())
                    {
                        var result = db.EMPLEADO.First(a => a.ID_EMPLEADO == _ID);
                        result.NOMBRE = txtnombre.Text;
                        result.PORCIENTO_COMISION = double.Parse(txtcomision.Text);
                        result.TANDA_LABORAR = cbxtandal.Text;
                        result.CEDULA = txtcedula.Text;
                        result.FECHA_INGRESO = (DateTime)dpkFecha.SelectedDate;
                        result.ESTADO = cbxestado.SelectedValue.ToString();
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
                    txtcomision.Text.Length > 0 &&
                    cbxtandal.SelectedValue != null
                    && dpkFecha.SelectedDate != null)
                {

                    using (var db = new Models.rentcarEntities())
                    {
                      
                        try
                        {
                            Models.EMPLEADO newMarca = new Models.EMPLEADO();
                            newMarca.NOMBRE = txtnombre.Text;
                            newMarca.PORCIENTO_COMISION = double.Parse(txtcomision.Text);
                            newMarca.TANDA_LABORAR = cbxtandal.Text;
                            newMarca.CEDULA = txtcedula.Text;
                            newMarca.FECHA_INGRESO = (DateTime)dpkFecha.SelectedDate;
                            newMarca.ESTADO = cbxestado.SelectedValue.ToString();
                            db.EMPLEADO.Add(newMarca);
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException err)
                        {
                            foreach (var eve in err.EntityValidationErrors)
                            {
                                MessageBox.Show("Entity of type "+eve.Entry.Entity.GetType().Name +"in state " + eve.Entry.State + " has the following validation errors:" );
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    MessageBox.Show("- Property: "+ ve.PropertyName + " , Error: "+ ve.ErrorMessage );
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

        private void Refresh()
        {
            using(var db = new Models.rentcarEntities())
            {
                var rows = db.EMPLEADO.ToList();
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
            txtcomision.Text = "";
            dpkFecha.SelectedDate = null;
            btnGuardar.Background = new SolidColorBrush(Colors.Green);
            btnGuardar.Content = "Guardar";
        }

        private void datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (datagrid.SelectedItem != null)
            {
                Models.EMPLEADO da = (Models.EMPLEADO)datagrid.SelectedItem;

                txtnombre.Text = da.NOMBRE;
                txtcedula.Text = da.CEDULA;
                txtcomision.Text = da.PORCIENTO_COMISION.ToString();
                cbxestado.SelectedValue = da.ESTADO.ToString();
                dpkFecha.SelectedDate = da.FECHA_INGRESO;
                cbxestado.SelectedValue = da.ESTADO;
                cbxtandal.Text = da.TANDA_LABORAR;
                _ID = da.ID_EMPLEADO;
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
                        var result = db.EMPLEADO.First(a => a.ID_EMPLEADO == _ID);
                        db.EMPLEADO.Remove(result);
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
    }
}
