using DS5.DataSet1TableAdapters;
using DS5.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DS5.ViewModel
{
    internal class ProductOrderViewModel : Helper
    {
        private int OrderId { get; set; }
        private int ClientId { get; set; }
       

        private DS5.DataSet1.Product_in_checkDataTable product_In_CheckRows;
        public DS5.DataSet1.Product_in_checkDataTable Product_In_CheckRows
        {
            get { return product_In_CheckRows; }
            set { product_In_CheckRows = value;
                OnPropertyChanged();
            }
        }
        private DS5.DataSet1.ProductsDataTable produuct;
        public DS5.DataSet1.ProductsDataTable Produuct
        {
            get { return produuct; }
            set
            {
                produuct = value;
                OnPropertyChanged();
            }
        }

        private decimal many;

        public decimal Many
        {
            get { return many; }
            set { many = value; }
        }
        private string change;

        public string Change
        {
            get { return change; }
            set { change = value; }
        }
        private DataRowView selected;

        public DataRowView Selected
        {
            get { return selected; }
            set { selected = value;
                OnPropertyChanged();
            }
        }

        public BindableCommand InsertCommand { get; set; }
        public BindableCommand DeleteCommand { get; set; }
        public BindableCommand GoCommand { get; set; }

        Product_in_checkTableAdapter pincheck = new Product_in_checkTableAdapter();
        ProductsTableAdapter product = new ProductsTableAdapter();
        OrdersTableAdapter order = new OrdersTableAdapter();
        public ProductOrderViewModel(int orderID, int clientID)
        {
            this.OrderId = orderID;
            this.ClientId = clientID;
            produuct = product.GetProduct();
            product_In_CheckRows = pincheck.ok(this.OrderId);
            CheckOrderStatus(this.OrderId);
            many = CalculateOrderTotal(this.OrderId);
            InsertCommand = new BindableCommand(_ => Insert_Click());
            DeleteCommand = new BindableCommand(_=> Delete_Click());
            GoCommand = new BindableCommand(_ => Go_Click());
        }
       
        
        private void Insert_Click()
        {
            if (Selected == null)
            {
                MessageBox.Show("Выберите продукт для добавления.");
                return;
            }

            var selectedProduct = Selected.Row[0];

            if (selectedProduct != DBNull.Value)
            {
                pincheck.InsertQuery(this.OrderId, Convert.ToInt32(selectedProduct));
                product_In_CheckRows = pincheck.ok(this.OrderId);
                many = CalculateOrderTotal(this.OrderId);
            }
        }
        private void Delete_Click()
        {
            if (Selected == null)
            {
                MessageBox.Show("Выберите продукт для удаления.");
                return;
            }
            var selectedProduct = Selected.Row[0];
            pincheck.DeleteQuery(Convert.ToInt32(selectedProduct));
            Product_In_CheckRows = pincheck.ok(this.OrderId);
            many = CalculateOrderTotal(this.OrderId);
        }
        private bool CheckOrderStatus(int orderID)
        {
            var orders = order.GetClientId(orderID);
            if (orders != null && orders.Count > 0)
            {
                return orders[0].status_order;
            }
            return false;
        }
        private void Check()
        {
            if (!CheckOrderStatus(this.ClientId))
            {
                MessageBox.Show("Ошибка: заказ не найден или он уже обработан.");
                return;
            }

            if (!decimal.TryParse(Change, out decimal payment) || payment < 0)
            {
                MessageBox.Show("Внесенная сумма должна быть правильным числом.");
               
                return;
            }

            decimal summ = CalculateOrderTotal(this.OrderId);
            if (payment < summ)
            {
                MessageBox.Show("Внесенная сумма меньше итоговой суммы заказа.");
                
                return;
            }
            if (CheckOrderStatus(this.ClientId))
            {
                if (!decimal.TryParse(Change, out decimal payments) || payments <= 0)
                {
                    MessageBox.Show("Введите валидную сумму оплаты.");
                    
                    return;
                }

                decimal summa = CalculateOrderTotal(this.OrderId);
                if (payments < summa)
                {
                    MessageBox.Show("Оплаченная сумма меньше суммы заказа.");
                    return;
                }
                decimal changeAmount = payments - summa;
                order.UpdateQueryReady(DateTime.Now.ToString(), summa, this.OrderId);

                var productInCheck = pincheck.GetDataByOrderId(this.OrderId);
                var productsDetails = new StringBuilder();

                foreach (var item in productInCheck)
                {
                    var productRows = product.GetDataById(item.product_ID);
                    if (productRows.Count > 0)
                    {
                        var productRow = productRows[0];
                        productsDetails.AppendLine($"{productRow.name_product}\t-\t{productRow.price}");
                    }
                }
                string receiptText = $@"
MacDonald's
Кассовый чек №{this.OrderId}

{productsDetails}

Итого к оплате: {summa}
Внесено: {payments}
Сдача: {changeAmount}
";

                string receiptPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"Чек_Номер_{this.OrderId}.txt");
                File.WriteAllText(receiptPath, receiptText, Encoding.UTF8);
                MessageBox.Show("Ваш заказ приготовлен, чек можете найти во вкладке 'Чеки'");
            }
            else
            {
                MessageBox.Show("Ошибка: заказ не найден или он уже обработан.");
            }
        }
        decimal CalculateOrderTotal(int orderId)
        {
            var productsInOrder = pincheck.GetDataByOrderId(orderId);
            decimal orderTotal = 0;
            foreach (var productInCheck in productsInOrder)
            {
                var products = product.GetDataById(productInCheck.product_ID).First();
                orderTotal += products.price;
            }
            return orderTotal;
        }
        private void Go_Click()
        {
            if (CheckOrderStatus(this.ClientId))
            {
                Check();
            }
            else
            {
                MessageBox.Show("Ваш заказ готовится, чек сможете получить после приготовления");
            }


        }
    }
}
