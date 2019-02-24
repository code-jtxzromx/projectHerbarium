using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace prototypeHerbarium
{
    /// <summary>
    /// Interaction logic for frmLogin.xaml
    /// </summary>
    public partial class frmLogin : Window
    {
        string DefaultUsername = "admin";
        string DefaultPassword = "admin";
        string DefaultStaff = "Temporary Admin";
        string DefaultRole = "SUPER-ADMINISTRATOR";
        bool noRecords;

        public frmLogin()
        {
            InitializeComponent();

            checkDBConnect();
            checkRecords();
        }
        
        private void KeyBoardEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                btnLogin_Click(btnLogin, null);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            txfError.Visibility = Visibility.Collapsed;
            txfSuccess.Visibility = Visibility.Collapsed;

            if (noRecords)
            {
                bool matchUsername = (txfUsername.Text == DefaultUsername);
                bool matchPassword = (txfPassword.Password == DefaultPassword);

                if (matchUsername && matchPassword)
                {
                    txfSuccess.Visibility = Visibility.Visible;
                    StaticData.staffname = DefaultStaff;
                    StaticData.username = DefaultUsername;
                    StaticData.role = DefaultRole;

                    loginSuccess();
                }
                else
                {
                    txfError.Visibility = Visibility.Visible;
                    txfPassword.Clear();
                }
            }
            else
            {
                DatabaseConnection connection = new DatabaseConnection();
                connection.setQuery("SELECT strFullName, strUsername, strRole " +
                                    "FROM viewAccounts " +
                                    "WHERE strUsername = @username AND strPassword = @password");
                connection.addParameter("@username", System.Data.SqlDbType.VarChar, txfUsername.Text);
                connection.addParameter("@password", System.Data.SqlDbType.VarChar, txfPassword.Password);

                SqlDataReader sqlData = connection.executeResult();
                if (sqlData.HasRows)
                {
                    txfSuccess.Visibility = Visibility.Visible;
                    while (sqlData.Read())
                    {
                        StaticData.staffname = sqlData[0].ToString();
                        StaticData.username = sqlData[1].ToString();
                        StaticData.role = sqlData[2].ToString();

                        loginSuccess();
                    }
                }
                else
                {
                    txfError.Visibility = Visibility.Visible;
                    txfPassword.Clear();
                }
            }
        }

        private void checkDBConnect()
        {
            DatabaseConnection connection = new DatabaseConnection();

            if (!connection.testConnection())
            {
                this.Close();
            }
        }

        private void loginSuccess()
        {           
            frmMain MainForm = new frmMain();
            
            MainForm.Show();
            this.Close();
        }

        private void checkRecords()
        {
            DatabaseConnection connection = new DatabaseConnection();

            connection.setQuery("SELECT intStaffID FROM tblAccounts");

            SqlDataReader sqlData = connection.executeResult();
            noRecords = (sqlData.HasRows) ? false : true;

            connection.closeResult();
        }
    }
}
