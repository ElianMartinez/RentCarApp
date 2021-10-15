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
        bool IsEditing = false;
        public EditarInspecciones(MainWindow m, int id, bool isEditing)
        {
            InitializeComponent();
            mw = m;
            ID = id;
            IsEditing = isEditing;
            if (isEditing)
            {
            GetData();
            }
        }


       private async void GetData()
        {
            using (var db = new Models.rentcarEntities())
            { 
                Models.INSPECCIONES row = await db.INSPECCIONES.Where(o => o.ID_INSPECCION == ID).FirstAsync(); 
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var a = new Inspecciones(mw);
            mw.ChangeContext(a);
        }
    }
}
