using System;
using System.Collections.Generic;
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
    /// Interaction logic for ctrlDateField.xaml
    /// </summary>
    public partial class ctrlDateField : UserControl
    {
        public ctrlDateField()
        {
            InitializeComponent();
        }

        public string Label
        {
            get { return lblFieldLabel.Text; }
            set { lblFieldLabel.Text = value; }
        }

        public bool RequiredField
        {
            get { return (lblRequired.Visibility == Visibility.Visible); }
            set { lblRequired.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public string DateContent
        {
            get { return dpkDatePicker.Text; }
            set { dpkDatePicker.Text = value; }
        }

        public bool ErrorMessage
        {
            set { lblErrorMessage.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public void Clear()
        {
            dpkDatePicker.Text = "";
        }
    }
}
