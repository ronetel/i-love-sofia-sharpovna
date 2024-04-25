using DS5.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DS5.ViewModel.Helpers
{
    internal class MainViewModel : Helper
    {
        private string loginText;
        //propfull
        public string LoginText
        {
            get { return loginText; }
            set { loginText = value; }
        }
        private string passwordText;

        public string PasswordText
        {
            get { return passwordText; }
            set { passwordText = value; }
        }

        //prop

        public BindableCommand AddCommand { get; set; }
        public AccountsTableAdapter account = new AccountsTableAdapter();
        public ClientsTableAdapter clients = new ClientsTableAdapter();
        public EmployeesTableAdapter employees = new EmployeesTableAdapter();
        
        public void Button_Click()
        {
            var curWindow = Application.Current.MainWindow;

            if (string.IsNullOrWhiteSpace(LoginText))
            {
                MessageBox.Show("Введите логин.");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordText))
            {
                MessageBox.Show("Введите пароль.");
                return;
            }

            var log = account.GetData().Where(a => a.login_account == LoginText && a.passord_account == PasswordText).FirstOrDefault();

            if (log != null)
            {
                int roleID = log.role_ID;
                switch (roleID)
                {
                   
                    case 3:
                        var emp = employees.GetData().Where(em => em.account_ID == log.ID_account).FirstOrDefault();
                        if (emp != null)
                        {
                            int employeeId = emp.ID_employees;
                            Application.Current.MainWindow = new EmploeeysWindow(employeeId);
                            Application.Current.MainWindow.Show();
                            curWindow.Close();
                        }
                        else
                        {
                            MessageBox.Show("Сотрудника не найдено.");
                        }
                        break;

                    case 4:
                        var cli = clients.GetData().Where(c => c.account_ID == log.ID_account).FirstOrDefault();
                        if (cli != null)
                        {
                            int clientId = cli.ID_client;
                            Application.Current.MainWindow = new ClientWindows(clientId);
                            Application.Current.MainWindow.Show();
                            curWindow.Close();
                        }
                        else
                        {
                            MessageBox.Show("Клиента не найдено.");
                        }
                        break;
                    default:
                        MessageBox.Show("Роль не определена или нет доступа.");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Неверные учетные данные.");
            }
        }
        public MainViewModel()
        {
            AddCommand = new BindableCommand(_ => Button_Click());
        }
    }
}
