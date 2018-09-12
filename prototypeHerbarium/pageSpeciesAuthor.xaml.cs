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
    /// Interaction logic for pageSpeciesAuthor.xaml
    /// </summary>
    public partial class pageSpeciesAuthor : Page
    {
        List<SpeciesAuthor> SpeciesAuthors = new List<SpeciesAuthor>();

        public pageSpeciesAuthor()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfAuthorID.Text is null || txfAuthorID.Text == "")
                    addAuthor();
                else
                    editAuthor();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfAuthorID.Clear();
            txfAuthorName.Clear();
            txfAuthorSuffix.Clear();
            btnSave.Content = "Save";

            msgAuthorName.Visibility = Visibility.Collapsed;
            msgAuthorSuffix.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Content = "Update";
            SpeciesAuthor selectedAuthor = dgrAuthorsTable.SelectedValue as SpeciesAuthor;
            
            var result = from author in SpeciesAuthors
                         where author.AuthorID == selectedAuthor.AuthorID
                         select author;

            if (pnlAddSpeciesAuthor.Visibility == Visibility.Collapsed)
                btnAddSpeciesAuthor_Click(btnAddSpeciesAuthor, null);

            foreach (SpeciesAuthor data in result)
            {
                txfAuthorID.Text = data.AuthorID.ToString();
                txfAuthorName.Text = data.AuthorName;
                txfAuthorSuffix.Text = data.AuthorSuffix;
            }
        }

        private void btnAddSpeciesAuthor_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddSpeciesAuthor.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddSpeciesAuthor.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddSpeciesAuthor.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddSpeciesAuthor.Content = (state) ? "Close Panel" : "Add Author";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in SpeciesAuthors
                         where record.AuthorName.ToUpper().Contains(input) ||
                                record.AuthorSuffix.ToUpper().Contains(input)
                         select record;

            dgrAuthorsTable.ItemsSource = result;
        }

        private void resetForm()
        {
            pnlAddSpeciesAuthor.Visibility = Visibility.Collapsed;
            sprAddSpeciesAuthor.Visibility = Visibility.Collapsed;
            btnAddSpeciesAuthor.Content = "Add Author";
            btnClear_Click(btnClear, null);

            getAuthorTable();
        }

        private void getAuthorTable()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<SpeciesAuthor> authors = new List<SpeciesAuthor>();

            btnClear_Click(btnClear, null);

            connection.setQuery("SELECT intAuthorID, strAuthorsName, strSpeciesSuffix " +
                                "FROM viewSpeciesAuthor " +
                                "ORDER BY strAuthorsName");
            SqlDataReader sqlData = connection.executeResult();

            while (sqlData.Read())
            {
                authors.Add(new SpeciesAuthor()
                {
                    AuthorID = Convert.ToInt32(sqlData[0]),
                    AuthorName = sqlData[1].ToString(),
                    AuthorSuffix = sqlData[2].ToString()
                });
            }
            connection.closeResult();

            dgrAuthorsTable.ItemsSource = authors;
            SpeciesAuthors = authors;
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgAuthorName.Visibility = Visibility.Collapsed;
            msgAuthorSuffix.Visibility = Visibility.Collapsed;

            if (txfAuthorName.Text == "")
            {
                msgAuthorName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfAuthorSuffix.Text == "")
            {
                msgAuthorSuffix.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addAuthor()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertSpeciesAuthor");
            connection.addSprocParameter("@author", SqlDbType.VarChar, txfAuthorName.Text);
            connection.addSprocParameter("@speciesSuffix", SqlDbType.VarChar, txfAuthorSuffix.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Species Author Added to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getAuthorTable();
        }

        private void editAuthor()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateSpeciesAuthor");
            connection.addSprocParameter("@author", SqlDbType.VarChar, txfAuthorName.Text);
            connection.addSprocParameter("@speciesSuffix", SqlDbType.VarChar, txfAuthorSuffix.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Species Author Updated to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getAuthorTable();
        }
    }
}

public class SpeciesAuthor
{
    public int AuthorID { get; set; }
    public string AuthorName { get; set; }
    public string AuthorSuffix { get; set; }
}