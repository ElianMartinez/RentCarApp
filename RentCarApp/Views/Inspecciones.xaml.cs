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
            var a = new EditarInspecciones(_mainWindow, ID);
            _mainWindow.ChangeContext(a);
        }

        private void datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (datagrid.SelectedItem != null)
            {
                var a = new EditarInspecciones(_mainWindow, ID);
            _mainWindow.ChangeContext(a);
            }
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(datagrid.SelectedItem != null)
            {
                Models.INSPECCIONES dato = (Models.INSPECCIONES)datagrid.SelectedItem;
                ID = dato.ID_INSPECCION;
                btnBorrar.Visibility = Visibility.Visible;
            }
            else
            {
                btnBorrar.Visibility = Visibility.Hidden;

            }
        }

        private void borrar_click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result1 = MessageBox.Show("Está seguro de borrar este registro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result1 == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new Models.rentcarEntities())
                    {
                        var result = db.INSPECCIONES.First(a => a.ID_INSPECCION == ID);
                        db.INSPECCIONES.Remove(result);
                        db.SaveChanges();
                        Refresh();
                    }
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
    }


}
