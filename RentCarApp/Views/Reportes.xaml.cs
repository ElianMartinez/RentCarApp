using ClosedXML.Excel;
using Syncfusion.XlsIO;
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
    /// Lógica de interacción para Reportes.xaml
    /// </summary>
    public partial class Reportes : UserControl
    {
        public Reportes()
        {
            InitializeComponent();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var db = new Models.rentcarEntities();
            
                List<TempRent> rs = (from r in db.RENTA
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
                                   EMPLEADO = emple.NOMBRE,
                                   FECHA_D = r.FEHCA_DEVOLUCION == null ? DateTime.MinValue : (DateTime)r.FEHCA_DEVOLUCION,
                                   NO_RENTA = r.NO_RENTA,
                                   ID_CLIENTE = cli.ID_CLIENTE,
                                   ID_VEHICULO = v.ID_VEHICULO
                               }
                             ).ToList();
           

                      var workbook = new XLWorkbook();
            
                var ws = workbook.Worksheets.Add("Reporte de Rentas");

                int currentRow = 1;
                ws.Cell(currentRow, 1).Value = "NO RENTA";
                ws.Cell(currentRow, 2).Value = "MARCA";
                ws.Cell(currentRow, 3).Value = "MODELO";
                ws.Cell(currentRow, 4).Value = "DESCRIPCIÓN";
                ws.Cell(currentRow, 5).Value = "FECHA RENTA";
                ws.Cell(currentRow, 6).Value = "FECHA DEVOLUCIÓN";
                ws.Cell(currentRow, 7).Value = "CLIENTE";
                ws.Cell(currentRow, 8).Value = "EMPLEADO";
                ws.Cell(currentRow, 9).Value = "ESTADO";


                foreach(var renta in rs)
                {
                currentRow++;
                ws.Cell(currentRow, 1).Value = renta.NO_RENTA;
                ws.Cell(currentRow, 2).Value = renta.MARCA;
                ws.Cell(currentRow, 3).Value = renta.MODELO;
                ws.Cell(currentRow, 4).Value = renta.DESCRIPCION;
                ws.Cell(currentRow, 5).Value = renta.FECHA;
                ws.Cell(currentRow, 6).Value = renta.FECHA_D;
                ws.Cell(currentRow, 7).Value = renta.CLIENTE;
                ws.Cell(currentRow, 8).Value = renta.EMPLEADO;
                ws.Cell(currentRow, 9).Value = renta.ESTADO;

            }

                workbook.SaveAs(@"C:\Users\Public\reporte-rentas.xlsx");
            
        }
    }
}
