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
using System.Windows.Shapes;

namespace MotoGP
{
    /// <summary>
    /// Interaction logic for AddOrModify.xaml
    /// </summary>
    public partial class AddOrModify : Window
    {
        MotoGPTeam modified;
        bool adding; //True if adding new value, false for modifying

        public AddOrModify(MotoGPTeam nmodified, bool nameReadOnly)
        {
            InitializeComponent();
            modified = nmodified;
            adding = !nameReadOnly;
            Name_tb.IsReadOnly = nameReadOnly;
            if(modified.Name!=null)
            {
                Name_tb.Text = modified.Name;
                Established_tb.Text = modified.Established.ToString();
                Trophies_tb.Text = modified.Trophies.ToString();
                Registered_cb.IsChecked = modified.Registered;
            }
        }

        private void Cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OK_btn_Click(object sender, RoutedEventArgs e)
        {
            //Validate everything
            bool isEverythingOK = true;
            Name_err_tb.Text = "";
            Established_err_tb.Text = "";
            Trophies_err_tb.Text = "";

            if(Name_tb.Text==null || Name_tb.Text=="")
            {
                isEverythingOK = false;
                Name_err_tb.Text = "Field must not be empty";
            }
            else if(Name_tb.Text!=null && Name_tb.Text.Length>80)
            {
                isEverythingOK = false;
                Name_err_tb.Text = "Maximum length is 80 character";
            }
            else
            {
                using (MotoGPContext db = new MotoGPContext())
                {
                    MotoGPTeam team = db.Teams.Find(Name_tb.Text);
                    if(team!=null && adding)
                    {
                        isEverythingOK = false;
                        Name_err_tb.Text = "Name already exists";
                    }
                }
            }

            if(Established_tb.Text!=null && Established_tb.Text!="")
            {
                int value = -1;
                if(!int.TryParse(Established_tb.Text,out value))
                {
                    isEverythingOK = false;
                    Established_err_tb.Text = "Field must contain an integer";
                }
                else if (value<1)
                {
                    isEverythingOK = false;
                    Established_err_tb.Text = "Field must contain a positive number";
                }
                else if (value>DateTime.Now.Year)
                {
                    isEverythingOK = false;
                    Established_err_tb.Text = "Field must not contain a year from future";
                }
            }
            else
            {
                isEverythingOK = false;
                Established_err_tb.Text = "Field must not be empty";
            }

            if(Trophies_tb.Text!=null && Trophies_tb.Text!="")
            {
                int value = -1;
                if (!int.TryParse(Trophies_tb.Text, out value))
                {
                    isEverythingOK = false;
                    Trophies_err_tb.Text = "Field must contain an integer";
                }
                else if (value < 0)
                {
                    isEverythingOK = false;
                    Trophies_err_tb.Text = "Field must contain a non negative number";
                }
            }
            else
            {
                isEverythingOK = false;
                Trophies_err_tb.Text = "Field must not be empty";
            }


            if(isEverythingOK)
            {
                modified.Name = Name_tb.Text;
                modified.Established = int.Parse(Established_tb.Text);
                modified.Trophies = int.Parse(Trophies_tb.Text);
                modified.Registered = Registered_cb.IsChecked==true;
                DialogResult = true;
                Close();
            }
        }
    }
}
