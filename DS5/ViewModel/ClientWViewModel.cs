using DS5.DataSet1TableAdapters;
using DS5.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DS5.ViewModel
{
    internal class ClientWViewModel : Helper
    {
        private int ClientId { get; set; }
        private OrdersTableAdapter orders;
        private Page p;

        public Page P 
        {
            get { return p; }
            set { p = value;
                OnPropertyChanged();
            }
        }
        public BindableCommand ExitCommand { get; set; }
        public BindableCommand CheckCommand { get; set; }
        public BindableCommand OrderCommand { get; set; }
        public ClientWViewModel(int clientId)
        {
            this.ClientId = clientId;
            orders = new OrdersTableAdapter();
            p = new ProductOrederPage(GetOrderIdForClient(), this.ClientId);
            ExitCommand = new BindableCommand(_ => Exit_Click());
            CheckCommand = new BindableCommand(_ => Check_Button_Click());
            OrderCommand = new BindableCommand(_ => Order_Button_Click());
            CreateNewOrder();
        }

        private void CreateNewOrder()
        {
            try
            {
                var activeOrder = orders.GetClientId(ClientId)
                                        .FirstOrDefault(o => o.status_order == false);
                if (activeOrder == null)
                {
                    int newOrderId = orders.InsertQuery(ClientId, false);

                    MessageBox.Show("Новый заказ создан с ID: " + newOrderId);
                }
                else
                {
                    MessageBox.Show("Существующий активный заказ с ID: " + activeOrder.ID_order);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при создании заказа: " + ex.Message);
            }
        }
        private int GetOrderIdForClient()
        {
            var order = orders.GetClientId(ClientId).ToList();
            if (order.Any())
            {
                return order.FirstOrDefault(x => !x.status_order)?.ID_order ?? 1;
            }
            return -1;
        }
        private void Check_Button_Click()
        {
            P = new CheckPage(GetOrderIdForClient(), this.ClientId);
        }

        private void Order_Button_Click()
        {
            P = new ProductOrederPage(GetOrderIdForClient(), this.ClientId);
        }

        private void Exit_Click()
        {
            var curWindow = Application.Current.MainWindow;
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
            curWindow.Close();
        }
    }

}
