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
    /// Interaction logic for pagePlantType.xaml
    /// </summary>
    public partial class pagePlantType : Page
    {
        List<PlantType> PlantTypes = new List<PlantType>();

        public pagePlantType()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfPlantTypeID.Text is null || txfPlantTypeID.Text == "")
                    addPlantType();
                else
                    editPlantType();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lblPlantTypeForm.Text = "Add Plant Type";
            txfPlantTypeID.Clear();
            txfPlantCode.Clear();
            txfPlantType.Clear();
            btnSave.Content = "Save";

            msgPlantCode.Visibility = Visibility.Collapsed;
            msgPlantType.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Content = "Update";
            lblPlantTypeForm.Text = "Edit Plant Type";
            PlantType selectedType = dgrPlantTypeTable.SelectedValue as PlantType;

            var result = from type in PlantTypes
                         where type.PlantTypeID == selectedType.PlantTypeID
                         select type;

            if (pnlAddPlantType.Visibility == Visibility.Collapsed)
                btnAddPlantType_Click(btnAddPlantType, null);

            foreach (PlantType data in result)
            {
                txfPlantTypeID.Text = data.PlantTypeID.ToString();
                txfPlantCode.Text = data.Code;
                txfPlantType.Text = data.Type;
            }
        }

        private void btnAddPlantType_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddPlantType.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddPlantType.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddPlantType.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddPlantType.Content = (state) ? "Close Panel" : "Add Plant Type";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in PlantTypes
                         where record.Code.ToUpper().Contains(input) ||
                                record.Type.ToUpper().Contains(input)
                         select record;

            dgrPlantTypeTable.ItemsSource = result;
        }

        private void resetForm()
        {
            lblPlantTypeForm.Text = "Add Plant Type";
            pnlAddPlantType.Visibility = Visibility.Collapsed;
            sprAddPlantType.Visibility = Visibility.Collapsed;
            btnAddPlantType.Content = "Add Plant Type";
            btnClear_Click(btnClear, null);

            getPlantTypeTable();
        }

        private void getPlantTypeTable()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<PlantType> types = new List<PlantType>();

            btnClear_Click(btnClear, null);

            connection.setQuery("SELECT intPlantTypeID, strPlantTypeCode, strPlantTypeName " +
                                "FROM tblPlantType " +
                                "ORDER BY strPlantTypeCode");
            SqlDataReader sqlData = connection.executeResult();

            while (sqlData.Read())
            {
                types.Add(new PlantType()
                {
                    PlantTypeID = Convert.ToInt32(sqlData[0]),
                    Code = sqlData[1].ToString(),
                    Type = sqlData[2].ToString()
                });
            }
            connection.closeResult();

            dgrPlantTypeTable.ItemsSource = types;
            PlantTypes = types;

        }

        private bool validateForm()
        {
            bool formOK = true;
            msgPlantCode.Visibility = Visibility.Collapsed;
            msgPlantType.Visibility = Visibility.Collapsed;

            if (txfPlantCode.Text == "")
            {
                msgPlantCode.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfPlantType.Text == "")
            {
                msgPlantType.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addPlantType()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertPlantType");
            connection.addSprocParameter("@plantCode", SqlDbType.VarChar, txfPlantCode.Text);
            connection.addSprocParameter("@plantType", SqlDbType.VarChar, txfPlantType.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Plant Type Added to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getPlantTypeTable();
        }

        private void editPlantType()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdatePlantType");
            connection.addSprocParameter("@plantTypeID", SqlDbType.Int, txfPlantTypeID.Text);
            connection.addSprocParameter("@plantCode", SqlDbType.VarChar, txfPlantCode.Text);
            connection.addSprocParameter("@plantType", SqlDbType.VarChar, txfPlantType.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Plant Type Updated to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getPlantTypeTable();
        }
    }
}

public class PlantType
{
    public int PlantTypeID { get; set; }
    public string Code { get; set; }
    public string Type { get; set; }
}