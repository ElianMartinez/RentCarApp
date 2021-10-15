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
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        
        public Home()
        {
            InitializeComponent();
            Refresh();

        }

        private void Refresh()
        {
            using (Models.rentcarEntities db = new Models.rentcarEntities())
            {
                int cantidadVehiculos = db.VEHICULOS.Count();    
                int cantidadEmpleados = db.EMPLEADO.Count();
                int cantidadRENTAS = db.RENTA.Where(o => o.ESTADO == "A").Count();
               
                valorEmpleados.Text = cantidadEmpleados.ToString();
                valorRentas.Text = cantidadRENTAS.ToString();
                valorVehiculos.Text = cantidadVehiculos.ToString();
            }
        }

       
    }
}
