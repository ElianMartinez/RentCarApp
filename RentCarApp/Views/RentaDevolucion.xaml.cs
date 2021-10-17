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
    /// Lógica de interacción para RentaDevolucion.xaml
    /// </summary>
    public partial class RentaDevolucion : UserControl
    {
        MainWindow _mw;
        int ID = 0;
        public RentaDevolucion(MainWindow mw)
        {
            InitializeComponent();
            _mw = mw;
            Refresh();
        }

        private void btnBuscar_Copy_Click(object sender, RoutedEventArgs e)
        {
            var a = new Rentar(_mw,0);
            _mw.ChangeContext(a);
        }

        private async void Refresh()
        {
            using(var db = new Models.rentcarEntities())
            {
                List<Models.CLIENTES> clientes = await db.CLIENTES.Where(o => o.ESTADO == "A").ToListAsync();
                List<Models.VEHICULOS> vehiculos = await db.VEHICULOS.ToListAsync();
                cbxvehiculos.DisplayMemberPath = "Text";
                cbxvehiculos.SelectedValuePath = "Value";
                cbxcliente.DisplayMemberPath = "Text";
                cbxcliente.SelectedValuePath = "Value";
                cbxestado.DisplayMemberPath = "Text";
                cbxestado.SelectedValuePath = "Value";


                foreach (Models.VEHICULOS i in vehiculos)
                {
                    cbxvehiculos.Items.Add(new { Text = i.MARCAS.DESCRIPCION + " " + i.MODELOS.DESCRIPCION + " " + i.DESCRIPCION, Value = i.ID_VEHICULO });
                }
                foreach (Models.CLIENTES i in clientes)
                {
                    cbxcliente.Items.Add(new
                    { Text = i.NOMBRE + " " + i.CEDULA, Value = i.ID_CLIENTE });
                }


                var rs = await (from r in db.RENTA
                                       join v in db.VEHICULOS on r.ID_VEHICULO equals v.ID_VEHICULO
                                       join marca in db.MARCAS on v.ID_MARCA equals marca.ID_MARCA
                                       join mod in db.MODELOS on v.ID_MODELO equals mod.ID_MODELO
                                       join cli in db.CLIENTES on r.ID_CLIENTE equals cli.ID_CLIENTE
                                       join emple in db.EMPLEADO on r.ID_EMPLEADO equals emple.ID_EMPLEADO
                                       select new TempRent()
                                       {
                                         CLIENTE = cli.NOMBRE + " "+ cli.CEDULA,
                                         DESCRIPCION = v.DESCRIPCION,
                                         MARCA = marca.DESCRIPCION,
                                         MODELO = mod.DESCRIPCION,
                                         ESTADO = r.ESTADO == "A" ? "ACTIVA" : r.ESTADO == "F" ? "FINALIZADA" : "CANCELADA",
                                         FECHA = r.FECHA_RENTA,
                                           EMPLEADO = emple.NOMBRE,
                                           FECHA_D = (DateTime)r.FEHCA_DEVOLUCION,
                                           NO_RENTA = r.NO_RENTA,
                                           ID_CLIENTE = cli.ID_CLIENTE,
                                           ID_VEHICULO = v.ID_VEHICULO
                                       }
                              ).ToListAsync();

                 datagrid.ItemsSource = rs;

                cbxvehiculos.Items.Add(new { Text = "Seleccionar", Value = 0 });
                cbxcliente.Items.Add(new { Text = "Seleccionar", Value = 0 });

                cbxestado.Items.Add(new { Text = "Seleccionar", Value = "" });
                cbxestado.Items.Add(new { Text = "ACTIVA", Value = "ACTIVA" });
                cbxestado.Items.Add(new { Text = "FINALIZADA", Value = "FINALIZADA" });
                cbxestado.Items.Add(new { Text = "CANCELADA", Value = "CANCELADA" });
            }
        }

        private void btnBuscar_Copy1_Click(object sender, RoutedEventArgs e)
        {
            var a = new Rentar(_mw, ID);
            _mw.ChangeContext(a);
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(datagrid.SelectedItem != null)
            {
                btnDevolver.Visibility = Visibility.Visible;
                TempRent ren = (TempRent)datagrid.SelectedItem;
                ID = ren.NO_RENTA;
            }
            else
            {
                ID = 0;
                btnDevolver.Visibility = Visibility.Hidden;
            }
        }


       async void Filter()
        {
            using (var db = new Models.rentcarEntities())
            {
                
                List<TempRent> rs = await (from r in db.RENTA
                          join v in db.VEHICULOS on r.ID_VEHICULO equals v.ID_VEHICULO
                          join marca in db.MARCAS on v.ID_MARCA equals marca.ID_MARCA
                          join mod in db.MODELOS on v.ID_MODELO equals mod.ID_MODELO
                          join cli in db.CLIENTES on r.ID_CLIENTE equals cli.ID_CLIENTE
                          join emple in db.EMPLEADO on r.ID_EMPLEADO equals emple.ID_EMPLEADO
                          select new TempRent()
                          {
                              CLIENTE = cli.NOMBRE + " " + cli.CEDULA,
                              DESCRIPCION = v.DESCRIPCION,
                              MARCA = marca.DESCRIPCION,
                              MODELO = mod.DESCRIPCION,
                              ESTADO = r.ESTADO == "A" ? "ACTIVA" : r.ESTADO == "F" ? "FINALIZADA" : "CANCELADA",
                              FECHA = r.FECHA_RENTA,
                              FECHA_D = (DateTime)r.FEHCA_DEVOLUCION,
                              NO_RENTA = r.NO_RENTA,
                              EMPLEADO = emple.NOMBRE,
                              ID_CLIENTE = cli.ID_CLIENTE,
                              ID_VEHICULO = v.ID_VEHICULO
                          }
                                  ).ToListAsync();
                List<TempRent> lts = null;
                if (cbxcliente.SelectedValue != null && (int)cbxcliente.SelectedValue > 0)
                {
                    lts = rs.Where(o => o.ID_CLIENTE == (int)cbxcliente.SelectedValue).ToList();
                }
                if (cbxvehiculos.SelectedValue != null && (int)cbxvehiculos.SelectedValue > 0)
                {
                    lts = lts == null ? rs.Where(o => o.ID_VEHICULO == (int)cbxvehiculos.SelectedValue).ToList() : lts.Where(o => o.ID_VEHICULO == (int)cbxvehiculos.SelectedValue).ToList();
                }

                if(iniciodp.SelectedDate != null && findp.SelectedDate != null)
                {
                    lts = lts == null ? rs.Where(o => o.FECHA >= iniciodp.SelectedDate && o.FECHA <= findp.SelectedDate).ToList() : lts.Where(o => o.FECHA >= iniciodp.SelectedDate && o.FECHA <= findp.SelectedDate).ToList();
                }

                if(cbxestado.SelectedValue != null && cbxestado.SelectedValue != "")
                {
                    lts = lts == null ? rs.Where(o => o.ESTADO == cbxestado.SelectedValue.ToString()).ToList() : lts.Where(o => o.ESTADO == cbxestado.SelectedValue.ToString()).ToList();
                }

                datagrid.ItemsSource = null;
                datagrid.ItemsSource = lts == null ? rs : lts;
            }
        }
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            Filter();
        }
    }

    class TempRent
    {
        public int NO_RENTA { get; set; }
        public string MARCA { get; set; }
        public string MODELO { get; set; }
        public string DESCRIPCION { get; set; }
        public string CLIENTE { get; set; }
        public string EMPLEADO { get; set; }
        public DateTime FECHA { get; set; }
        public DateTime FECHA_D { get; set; }
        public string ESTADO { get; set; }
        public int ID_VEHICULO { get; set; }
        public int ID_CLIENTE { get; set; }

        
}

}
