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
    /// Interaction logic for pageTaxonClass.xaml
    /// </summary>
    public partial class pageTaxonClass : Page
    {
        List<TaxonClass> TaxonomicClasses = new List<TaxonClass>();

        public pageTaxonClass()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfClassID.Text is null || txfClassID.Text == "")
                    addClass();
                else
                    editClass();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lblClassForm.Text = "Add Class";
            txfClassID.Clear();
            cbxPhylumName.SelectedIndex = -1;
            txfClassName.Clear();
            btnSave.Content = "Save";

            msgPhylumName.Visibility = Visibility.Collapsed;
            msgClassName.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Content = "Update";
            lblClassForm.Text = "Edit Class";
            TaxonClass selectedClass = dgrClassTable.SelectedValue as TaxonClass;

            var result = from sclass in TaxonomicClasses
                         where sclass.ClassID == selectedClass.ClassID
                         select sclass;

            if (pnlAddClass.Visibility == Visibility.Collapsed)
                btnAddClass_Click(btnAddClass, null);

            foreach (TaxonClass data in result)
            {
                txfClassID.Text = data.ClassID;
                cbxPhylumName.SelectedItem = data.PhylumName;
                txfClassName.Text = data.ClassName;
            }
        }

        private void btnAddClass_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddClass.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddClass.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddClass.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddClass.Content = (state) ? "Close Panel" : "Add Class";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in TaxonomicClasses
                         where record.ClassID.ToUpper().Contains(input) ||
                                record.PhylumName.ToUpper().Contains(input) ||
                                record.ClassName.ToUpper().Contains(input)
                         select record;

            dgrClassTable.ItemsSource = result;
        }

        private void resetForm()
        {
            lblClassForm.Text = "Add Class";
            pnlAddClass.Visibility = Visibility.Collapsed;
            sprAddClass.Visibility = Visibility.Collapsed;
            btnAddClass.Content = "Add Class";
            btnClear_Click(btnClear, null);

            getClassTable();
            getPhylumList();
        }

        private void getClassTable()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<TaxonClass> classes = new List<TaxonClass>();

            btnClear_Click(btnClear, null);

            connection.setQuery("SELECT strClassNo, strPhylumName, strClassName " +
                                "FROM viewTaxonClass " +
                                "ORDER BY strPhylumName, strClassName");
            SqlDataReader sqlData = connection.executeResult();

            while (sqlData.Read())
            {
                classes.Add(new TaxonClass()
                {
                    ClassID = sqlData[0].ToString(),
                    PhylumName = sqlData[1].ToString(),
                    ClassName = sqlData[2].ToString()
                });
            }
            connection.closeResult();

            dgrClassTable.ItemsSource = classes;
            TaxonomicClasses = classes;
        }

        private void getPhylumList()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();

            cbxPhylumName.Items.Clear();

            // Query Command Setting
            connection.setQuery("SELECT strPhylumName FROM tblPhylum ORDER BY strPhylumName");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                cbxPhylumName.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgPhylumName.Visibility = Visibility.Collapsed;
            msgClassName.Visibility = Visibility.Collapsed;

            if (cbxPhylumName.SelectedIndex == -1)
            {
                msgPhylumName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfClassName.Text == "")
            {
                msgClassName.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addClass()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertClass");
            connection.addSprocParameter("@phylumName", SqlDbType.VarChar, cbxPhylumName.SelectedItem.ToString());
            connection.addSprocParameter("@className", SqlDbType.VarChar, txfClassName.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Class Added to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getClassTable();
        }

        private void editClass()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateClass");
            connection.addSprocParameter("@classNo", SqlDbType.VarChar, txfClassID.Text);
            connection.addSprocParameter("@phylumName", SqlDbType.VarChar, cbxPhylumName.SelectedItem.ToString());
            connection.addSprocParameter("@className", SqlDbType.VarChar, txfClassName.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Class Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getClassTable();
        }
    }
}

public class TaxonClass
{
    public string ClassID { get; set; }
    public string PhylumName { get; set; }
    public string ClassName { get; set; }
}