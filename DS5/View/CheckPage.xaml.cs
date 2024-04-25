using System;
using System.Collections.Generic;
using System.Data;
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
using DS5.DataSet1TableAdapters;
using DS5.ViewModel;

namespace DS5
{
    /// <summary>
    /// Логика взаимодействия для CheckPage.xaml
    /// </summary>
    public partial class CheckPage : Page
    {
        private int OrderId { get; set; }
        private int ClientId { get; set; }
        OrdersTableAdapter orders = new OrdersTableAdapter();
        public CheckPage(int orderID, int clientID)
        {
            InitializeComponent();
            this.OrderId = orderID;
            this.ClientId = clientID;
            var a = checks.ItemsSource;
            a = orders.getall(this.ClientId);
            DataContext = new CheckViewModel(orderID, clientID);
        }
       
    }
}
