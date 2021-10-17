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
    /// Lógica de interacción para Rentar.xaml
    /// </summary>
    public partial class Rentar : UserControl
    {
        MainWindow _mw;
        int ID = 0;
        public Rentar(MainWindow mw, int id)
        {
            InitializeComponent();
            _mw = mw;
            ID = id;
            Refresh(); 
            if (ID > 0)
            {
                txtTitulo.Text = "Devolción";
                datapicker2.IsEnabled = true;

                cbxcliente.IsEnabled = false;
                cbxempleado.IsEnabled = false;
                cbxestado.IsEnabled = true;
                cbxvehiculos.IsEnabled = false;
                txtcantidaddias.IsEnabled = false;
                txtporDias.IsEnabled = false;
                datepicker1.IsEnabled = false;
                GetData();
            }
            else
            {
                txtTitulo.Text = "Rentar";
                cbxestado.IsEnabled = false;
                datapicker2.IsEnabled = false;

            }
        }
        void ClosePanel()
        {
            var a = new RentaDevolucion(_mw);
            _mw.ChangeContext(a);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClosePanel();
        }

        private async void guardar_click(object sender, RoutedEventArgs e)
        {
            if (ID > 0)
            {
               await devolverAuto();
            }
            else
            {
              await rentarAuto();
            }
        }

      async Task devolverAuto()
        {
            using(var db = new Models.rentcarEntities())
            {

                Models.VEHICULOS vh = await db.VEHICULOS.Where(o => o.ID_VEHICULO == (int)cbxvehiculos.SelectedValue).FirstOrDefaultAsync();
                if (datapicker2.SelectedDate != null)
                { 
                vh.ESTADO = "A";
                }
                await db.SaveChangesAsync();

                Models.RENTA renta = await db.RENTA.Where(o => o.NO_RENTA == ID).FirstOrDefaultAsync();
                if(datapicker2.SelectedDate != null)
                {
                    if (cbxestado.SelectedValue.ToString() == "A")
                    {
                        renta.ESTADO = "F";
                    }
                    else
                    {
                        renta.ESTADO = cbxestado.SelectedValue.ToString();
                    }
                }
                else
                {
                    renta.ESTADO = cbxestado.SelectedValue.ToString();
                }
                renta.FEHCA_DEVOLUCION = (DateTime)datapicker2.SelectedDate;
                renta.COMENTARIO = txtcomentario.Text;
                await db.SaveChangesAsync();
                ClosePanel();
            }
        }
      async Task rentarAuto()
        {
            if (cbxcliente.SelectedValue != null && cbxempleado.SelectedValue != null  && cbxvehiculos.SelectedValue != null && int.Parse(txtcantidaddias.Text) > 0 && int.Parse(txtporDias.Text) > 0  && txtcantidaddias.Text != String.Empty && txtporDias.Text != String.Empty )
            {
                using(var db = new Models.rentcarEntities())
                {
                    Models.RENTA renta = new Models.RENTA
                    {
                        ID_CLIENTE = (int)cbxcliente.SelectedValue,
                        ID_EMPLEADO = (int)cbxempleado.SelectedValue,
                        ID_VEHICULO = (int)cbxvehiculos.SelectedValue,
                        ESTADO = "A",
                        CANTIDAD_DIAS = int.Parse(txtcantidaddias.Text),
                        MONTO_DIA = decimal.Parse(txtporDias.Text),
                        FECHA_RENTA = (DateTime)datepicker1.SelectedDate,
                        COMENTARIO = txtcomentario.Text
                    };

                   db.RENTA.Add(renta);
                   await db.SaveChangesAsync();

                   var vehiculo = await db.VEHICULOS.Where(o => o.ID_VEHICULO == renta.ID_VEHICULO).FirstAsync();
                   vehiculo.ESTADO = "R";
                   await db.SaveChangesAsync();
                    ClosePanel();
                }
            }
            else
            {
                MessageBox.Show("Los campos están mal.");
            }
        }

        private async void GetData()
        {
            using (var db = new Models.rentcarEntities())
            {
                Models.RENTA ren = await db.RENTA.Where(o => o.NO_RENTA == ID).FirstAsync();
                cbxempleado.SelectedValue = ren.ID_EMPLEADO;
                cbxvehiculos.SelectedValue = ren.ID_VEHICULO;
                cbxestado.SelectedValue = ren.ESTADO;
                cbxcliente.SelectedValue = ren.ID_CLIENTE;
                txtcantidaddias.Text = ren.CANTIDAD_DIAS.ToString();
                txtporDias.Text = ren.MONTO_DIA.ToString();
                datepicker1.SelectedDate = ren.FECHA_RENTA;
                txtcomentario.Text = ren.COMENTARIO;
                datapicker2.SelectedDate = ren.FEHCA_DEVOLUCION;
            }
        }

        private async void Refresh()
        {
            using (var db = new Models.rentcarEntities())
            {
                List<Models.VEHICULOS> vehiculos;
                if (ID > 0)
                {
                     vehiculos = await db.VEHICULOS.Where(o => o.ESTADO != "I").ToListAsync();
                }
                else
                {
                     vehiculos = await db.VEHICULOS.Where(o => o.ESTADO == "A").ToListAsync();
                }
                
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
            new {Text = "ACTIVA", Value = "A" },
            new { Text = "FINALIZADA", Value = "F"},
            new {Text ="CANCELADA", Value ="C"}
                 };

                cbxestado.DisplayMemberPath = "Text";
                cbxestado.SelectedValuePath = "Value";
                cbxestado.ItemsSource = items1;
            }
        }

        private void txtporDias_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtporDias.Text, "[^0-9]"))
            {
                txtporDias.Text = txtporDias.Text.Remove(txtporDias.Text.Length - 1);
            }
        }

        private void txtcantidaddias_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtcantidaddias.Text, "[^0-9]"))
            {
                txtcantidaddias.Text = txtcantidaddias.Text.Remove(txtcantidaddias.Text.Length - 1);
            }
        }
    }
}
