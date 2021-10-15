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
    /// Lógica de interacción para Inspecciones.xaml
    /// </summary>
    public partial class Inspecciones : UserControl
    {
        int ID = 0;
        MainWindow _mainWindow;
        public Inspecciones(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            Refresh();
        }

        private async void Refresh()
        {
            using(var db = new Models.rentcarEntities())
            {
                IQueryable<Models.INSPECCIONES> lst =
                    from d in db.INSPECCIONES
                    select d;

                 datagrid.ItemsSource = await lst.ToListAsync();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var a = new EditarInspecciones(_mainWindow, ID, false);
            _mainWindow.ChangeContext(a);
        }
    }


}
