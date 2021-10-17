using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Lógica de interacción para EditarInspecciones.xaml
    /// </summary>
    public partial class EditarInspecciones : UserControl
    {
  
        MainWindow mw;
        int ID = 0;
        public EditarInspecciones(MainWindow m, int id)
        {
            InitializeComponent();
            mw = m;
            ID = id;
            Refresh();
            if (ID > 0)
            {
                GetData();
            }
        }

        private async void Refresh()
        {
            using (var db = new Models.rentcarEntities())
            {
                List<Models.VEHICULOS> vehiculos = await db.VEHICULOS.Where(o => o.ESTADO == "A").ToListAsync();
                List<Models.CLIENTES> clientes = await db.CLIENTES.Where(o => o.ESTADO == "A").ToListAsync();
                List<Models.EMPLEADO> empleados = await db.EMPLEADO.Where(o => o.ESTADO == "A").ToListAsync();
                cbxvehiculos.DisplayMemberPath = "Text";
                cbxvehiculos.SelectedValuePath = "Value";
                cbxcliente.DisplayMemberPath = "Text";
                cbxcliente.SelectedValuePath = "Value";
                cbxempleado.DisplayMemberPath = "Text";
                cbxempleado.SelectedValuePath = "Value";

                foreach (Models.VEHICULOS i in vehiculos)
                {
                    cbxvehiculos.Items.Add(new { Text = i.MARCAS.DESCRIPCION + " " + i.MODELOS.DESCRIPCION + " " + i.DESCRIPCION, Value = i.ID_VEHICULO });
                }

                
                foreach (Models.CLIENTES i in clientes)
                {
                    cbxcliente.Items.Add(new
                    { Text = i.NOMBRE + " " + i.CEDULA, Value = i.ID_CLIENTE });
                }

                foreach (Models.EMPLEADO i in empleados)
                {
                    cbxempleado.Items.Add(new { Text = i.NOMBRE + " " + i.CEDULA, Value = i.ID_EMPLEADO });
                }

                var items1 = new[] {
            new {Text = "ACTIVO", Value = "A" },
            new { Text = "INACTIVO", Value = "I"},
                 };

                cbxestado.DisplayMemberPath = "Text";
                cbxestado.SelectedValuePath = "Value";
                cbxestado.ItemsSource = items1;

                cbxcombus.DisplayMemberPath = "Text";
                cbxcombus.SelectedValuePath = "Value";
                cbxcombus.Items.Add(new { Text = "1/4", Value = 0.25f });
                cbxcombus.Items.Add(new { Text = "1/2", Value = 0.50f });
                cbxcombus.Items.Add(new { Text = "3/4", Value = 0.75f });
                cbxcombus.Items.Add(new { Text = "LLENO", Value = 1.0f });




            }
        }

        private async void GetData()
        {
            using(var db = new Models.rentcarEntities())
            {
                Models.INSPECCIONES inspeccion = await db.INSPECCIONES.Where(o => o.ID_INSPECCION == ID).FirstAsync();
                cbxcliente.SelectedValue = inspeccion.ID_CLIENTE;
                cbxempleado.SelectedValue = inspeccion.ID_EMPLEADO;
                cbxvehiculos.SelectedValue = inspeccion.ID_VEHICULO;
                cbxcombus.SelectedValue = inspeccion.CANTIDAD_COMBUSTIBLE;
                cbxestado.SelectedValue = inspeccion.ESTADO;
                checg1.IsChecked = inspeccion.E_G1;
                checkg2.IsChecked = inspeccion.E_G2;
                checkg3.IsChecked = inspeccion.E_G3;
                checkg4.IsChecked = inspeccion.E_G4;
                datepicker.SelectedDate = inspeccion.FECHA;
                checkgato.IsChecked = inspeccion.GATO;
                gomarepuesto.IsChecked = inspeccion.GOMA_RESPUESTA;
                checkcristalroto.IsChecked = inspeccion.ROTURAS_CRISTAL;
                rayaduras.IsChecked = inspeccion.RALLADURAS;
            }
        }

        void ClosePanel()
        {
            var a = new Inspecciones(mw);
            mw.ChangeContext(a);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClosePanel();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {

        }



        private async void guardar_click(object sender, RoutedEventArgs e)
        {
            if (ID > 0)
            {
                if (cbxcliente.SelectedValue != null && cbxcombus.SelectedValue != null && cbxempleado.SelectedValue != null && cbxestado.SelectedValue != null && cbxvehiculos.SelectedValue != null)
                {
                    using (var db = new Models.rentcarEntities())
                    {
                        var row = await db.INSPECCIONES.Where(o => o.ID_INSPECCION == ID).FirstAsync();
                        row.ID_CLIENTE = (int)cbxcliente.SelectedValue;
                        row.ID_EMPLEADO = (int)cbxempleado.SelectedValue;
                        row.ID_VEHICULO = (int)cbxvehiculos.SelectedValue;
                        row.CANTIDAD_COMBUSTIBLE = (float)cbxcombus.SelectedValue;
                        row.ESTADO = cbxestado.SelectedValue.ToString();
                        row.E_G1 = (bool)checg1.IsChecked;
                        row.E_G2 = (bool)checkg2.IsChecked;
                        row.E_G3 = (bool)checkg3.IsChecked;
                        row.E_G4 = (bool)checkg4.IsChecked;
                        row.FECHA = (DateTime)datepicker.SelectedDate;
                        row.GATO = (bool)checkgato.IsChecked;
                        row.GOMA_RESPUESTA = (bool)gomarepuesto.IsChecked;
                        row.ROTURAS_CRISTAL = (bool)checkcristalroto.IsChecked;
                        row.RALLADURAS = (bool)rayaduras.IsChecked;
                        await db.SaveChangesAsync();
                        ClosePanel();
                    }
                }
                else
                {
                    MessageBox.Show("Los campos están mal.");
                }
            }
            else
            {
                if (cbxcliente.SelectedValue != null && cbxcombus.SelectedValue != null && cbxempleado.SelectedValue != null && cbxestado.SelectedValue != null && cbxvehiculos.SelectedValue != null)
                {
                    Models.INSPECCIONES row = new Models.INSPECCIONES();

                    row.ID_CLIENTE = (int)cbxcliente.SelectedValue;
                    row.ID_EMPLEADO = (int)cbxempleado.SelectedValue;
                    row.ID_VEHICULO = (int)cbxvehiculos.SelectedValue;
                    row.CANTIDAD_COMBUSTIBLE = (float)cbxcombus.SelectedValue;
                    row.ESTADO = cbxestado.SelectedValue.ToString();
                    row.E_G1 = (bool)checg1.IsChecked;
                    row.E_G2 = (bool)checkg2.IsChecked;
                    row.E_G3 = (bool)checkg3.IsChecked;
                    row.E_G4 = (bool)checkg4.IsChecked;
                    row.FECHA = (DateTime)datepicker.SelectedDate;
                    row.GATO = (bool)checkgato.IsChecked;
                    row.GOMA_RESPUESTA = (bool)gomarepuesto.IsChecked;
                    row.ROTURAS_CRISTAL = (bool)checkcristalroto.IsChecked;
                    row.RALLADURAS = (bool)rayaduras.IsChecked;

                    using (var db = new Models.rentcarEntities())
                    {
                        db.INSPECCIONES.Add(row);
                        await db.SaveChangesAsync();
                        ClosePanel();
                    }

                }
                else
                {
                    MessageBox.Show("Los campos están mal.");
                }
            }
        }


    }

  

}
