using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace prototypeHerbarium
{
    /// <summary>
    /// Interaction logic for pageTaxonOrder.xaml
    /// </summary>
    public partial class pageTaxonOrder : Page
    {
        List<TaxonOrder> TaxonomicOrders = new List<TaxonOrder>();

        public pageTaxonOrder()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfOrderID.Text is null || txfOrderID.Text == "")
                    addOrder();
                else
                    editOrder();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfOrderID.Clear();
            cbxClassName.SelectedIndex = -1;
            txfOrderName.Clear();

            msgClassName.Visibility = Visibility.Collapsed;
            msgOrderName.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            TaxonOrder selectedOrder = dgrOrderTable.SelectedValue as TaxonOrder;

            var result = from order in TaxonomicOrders
                         where order.OrderID == selectedOrder.OrderID
                         select order;

            if (pnlAddOrder.Visibility == Visibility.Collapsed)
                btnAddOrder_Click(btnAddOrder, null);

            foreach (TaxonOrder data in result)
            {
                txfOrderID.Text = data.OrderID;
                cbxClassName.SelectedItem = data.ClassName;
                txfOrderName.Text = data.OrderName;
            }
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddOrder.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddOrder.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddOrder.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddOrder.Content = (state) ? "Close Panel" : "Add Class";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in TaxonomicOrders
                         where record.OrderID.ToUpper().Contains(input) ||
                                record.ClassName.ToUpper().Contains(input) ||
                                record.OrderName.ToUpper().Contains(input)
                         select record;

            dgrOrderTable.ItemsSource = result;
        }

        private void resetForm()
        {
            pnlAddOrder.Visibility = Visibility.Collapsed;
            sprAddOrder.Visibility = Visibility.Collapsed;
            btnAddOrder.Content = "Add Order";
            btnClear_Click(btnClear, null);

            getOrderTable();
            getClassList();
        }

        private void getOrderTable()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<TaxonOrder> orders = new List<TaxonOrder>();

            btnClear_Click(btnClear, null);

            connection.setQuery("SELECT strOrderNo, strClassName, strOrderName FROM viewTaxonOrder");
            SqlDataReader sqlData = connection.executeResult();

            while (sqlData.Read())
            {
                orders.Add(new TaxonOrder()
                {
                    OrderID = sqlData[0].ToString(),
                    ClassName = sqlData[1].ToString(),
                    OrderName = sqlData[2].ToString()
                });
            }
            connection.closeResult();

            dgrOrderTable.ItemsSource = orders;
            TaxonomicOrders = orders;
        }

        private void getClassList()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();

            cbxClassName.Items.Clear();

            // Query Command Setting
            connection.setQuery("SELECT strClassName FROM tblClass");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                cbxClassName.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgClassName.Visibility = Visibility.Collapsed;
            msgOrderName.Visibility = Visibility.Collapsed;

            if (cbxClassName.SelectedIndex == -1)
            {
                msgClassName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfOrderName.Text == "")
            {
                msgOrderName.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addOrder()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertOrder");
            connection.addSprocParameter("@className", SqlDbType.VarChar, cbxClassName.SelectedItem.ToString());
            connection.addSprocParameter("@orderName", SqlDbType.VarChar, txfOrderName.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Order Added to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getOrderTable();
        }

        private void editOrder()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateOrder");
            connection.addSprocParameter("@orderNo", SqlDbType.VarChar, txfOrderID.Text);
            connection.addSprocParameter("@className", SqlDbType.VarChar, cbxClassName.SelectedItem.ToString());
            connection.addSprocParameter("@orderName", SqlDbType.VarChar, txfOrderName.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Order Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getOrderTable();
        }
    }
}

public class TaxonOrder
{
    public string OrderID { get; set; }
    public string ClassName { get; set; }
    public string OrderName { get; set; }
}