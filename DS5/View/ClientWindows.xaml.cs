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
using System.Windows.Shapes;
using DS5.DataSet1TableAdapters;
using DS5.ViewModel;

namespace DS5
{
    public partial class ClientWindows : Window
    {
        private int ClientId { get; set; }
        public ClientWindows(int clientId)
        {
            this.ClientId = clientId;
            InitializeComponent();
            DataContext = new ClientWViewModel(clientId);
        }
    }
}
