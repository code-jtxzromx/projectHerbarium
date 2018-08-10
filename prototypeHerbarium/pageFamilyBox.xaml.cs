﻿using System;
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
    /// Interaction logic for pageFamilyBox.xaml
    /// </summary>
    public partial class pageFamilyBox : Page
    {
        List<FamilyBox> HerbariumBoxes = new List<FamilyBox>();

        public pageFamilyBox()
        {
            InitializeComponent();

            getFamilyBoxTable();
            getFamilyList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfBoxNumber.Text is null || txfBoxNumber.Text == "")
                    addFamilyBox();
                else
                    editFamilyBox();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfBoxNumber.Clear();
            cbxFamily.SelectedIndex = -1;
            txfBoxLimit.Clear();

            msgFamily.Visibility = Visibility.Collapsed;
            msgBoxLimit.Visibility = Visibility.Collapsed;
        }

        private void btnAddBox_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddBox.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddBox.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddBox.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddBox.Content = (state) ? "Close Panel" : "Add Family Box";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in HerbariumBoxes
                         where record.BoxNumber.ToUpper().Contains(input) ||
                            record.BoxNumber.ToUpper().Contains(input)
                         select record;

            dgrBoxTable.ItemsSource = result;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            FamilyBox SelectedBox = dgrBoxTable.SelectedValue as FamilyBox;

            var result = from box in HerbariumBoxes
                         where box.BoxNumber == SelectedBox.BoxNumber
                         select box;

            if (pnlAddBox.Visibility == Visibility.Collapsed)
                btnAddBox_Click(btnAddBox, null);

            foreach (FamilyBox data in result)
            {
                txfBoxNumber.Text = data.BoxNumber;
                cbxFamily.SelectedItem = data.Family;
                txfBoxLimit.Text = data.BoxLimit.ToString();
            }
        }

        public void getFamilyBoxTable()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<FamilyBox> familyBoxes = new List<FamilyBox>();

            btnClear_Click(btnClear, null);

            // Query Command Setting
            connection.setQuery("SELECT strBoxNumber, strFamilyName, intBoxLimit FROM viewFamilyBox");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                familyBoxes.Add(new FamilyBox()
                {
                    BoxNumber = sqlData[0].ToString(),
                    Family = sqlData[1].ToString(),
                    BoxLimit = Convert.ToInt32(sqlData[2])
                });
            }
            connection.closeResult();

            dgrBoxTable.ItemsSource = familyBoxes;
            HerbariumBoxes = familyBoxes;
        }

        public void getFamilyList()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();

            cbxFamily.Items.Clear();

            // Query Command Setting
            connection.setQuery("SELECT strFamilyName FROM tblFamily");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                cbxFamily.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgFamily.Visibility = Visibility.Collapsed;
            msgBoxLimit.Visibility = Visibility.Collapsed;

            if (cbxFamily.SelectedIndex == -1)
            {
                msgFamily.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfBoxLimit.Text == "")
            {
                msgBoxLimit.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        public void addFamilyBox()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertFamilyBox");
            connection.addSprocParameter("@familyName", SqlDbType.VarChar, cbxFamily.SelectedItem);
            connection.addSprocParameter("@boxLimit", SqlDbType.Int, txfBoxLimit.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Family Box Added to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getFamilyBoxTable();
        }

        public void editFamilyBox()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateFamilyBox");
            connection.addSprocParameter("@boxNumber", SqlDbType.VarChar, txfBoxNumber.Text);
            connection.addSprocParameter("@familyName", SqlDbType.VarChar, cbxFamily.SelectedItem);
            connection.addSprocParameter("@boxLimit", SqlDbType.Int, txfBoxLimit.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Family Box Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getFamilyBoxTable();
        }
    }
}

public class FamilyBox
{
    public string BoxNumber { get; set; }
    public string Family { get; set; }
    public int BoxLimit { get; set; }
}