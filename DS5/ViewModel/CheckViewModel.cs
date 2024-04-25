using DS5.DataSet1TableAdapters;
using DS5.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DS5.ViewModel
{
    internal class CheckViewModel : Helper
    {
        private int OrderId { get; set; }
        private int ClientId { get; set; }
        private DataSet1.Product_in_checkDataTable grid_check;

        public DataSet1.Product_in_checkDataTable Grid_check
        {
            get { return grid_check; }
            set { grid_check = value; }
        }
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }


        Product_in_checkTableAdapter pincheck = new Product_in_checkTableAdapter();
        ProductsTableAdapter product = new ProductsTableAdapter();

        OrdersTableAdapter orders = new OrdersTableAdapter();
        public CheckViewModel(int orderID, int clientID)
        {
            this.OrderId = orderID;
            this.ClientId = clientID;
            grid_check = pincheck.ok(this.OrderId);

            checks.ItemsSource = orders.getall(this.ClientId);
            checks.DisplayMemberPath = "ID_order";

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
        private void checks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var selectedOrder = checks.SelectedItem as DataRowView;

            if (selectedOrder != null)
            {
                int selectedOrderId = Convert.ToInt32(selectedOrder["ID_order"]);
                int selectedEmployeeId = Convert.ToInt32(selectedOrder[0]);
                grid_check = pincheck.ok(selectedOrderId);

                var employeeInfo = orders.getIDemp(selectedEmployeeId);
                if (employeeInfo.Count > 0)
                {
                    var employeeRow = employeeInfo[0];
                    emp.Text = $"{employeeRow["name_emp"]} {employeeRow["surname_emp"]} {employeeRow["midlname_emp"]}";
                }

                date.Text = Convert.ToDateTime(selectedOrder["date_check"]).ToString("dd/MM/yyyy");
                summ.Text = $"{CalculateOrderTotal(selectedOrderId):C2}";
            }
        }
    }
}
