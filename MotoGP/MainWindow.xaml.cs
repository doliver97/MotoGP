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
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;

namespace MotoGP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<MotoGPTeam> teams;
        
        public MainWindow()
        {
            InitializeComponent();

            //LINQ-able
            using (MotoGPContext db = new MotoGPContext())
            {
                teams = db.Teams.ToList();
            }


            Teams_dgw.ItemsSource = teams; //Changes dynamically with the list
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            MotoGPTeam modified = new MotoGPTeam();

            AddOrModify addOrModify = new AddOrModify(modified, false);
            addOrModify.ShowDialog();

            //Clicked OK (validated)
            if (addOrModify.DialogResult.HasValue && addOrModify.DialogResult.Value)
            {
                //Upserting value
                using (MotoGPContext db = new MotoGPContext())
                {
                    db.Teams.AddOrUpdate(modified);
                    db.SaveChanges();
                    teams = db.Teams.ToList();
                    Teams_dgw.ItemsSource = teams;
                }
            }
        }

        private void Modify_btn_Click(object sender, RoutedEventArgs e)
        {
            MotoGPTeam modified = (MotoGPTeam)Teams_dgw.SelectedItem;

            //if nothing selected, do not respond
            if(modified == null)
            {
                return;
            }

            AddOrModify addOrModify = new AddOrModify(modified, true);
            addOrModify.ShowDialog();

            //Clicked OK (validated)
            if (addOrModify.DialogResult.HasValue && addOrModify.DialogResult.Value)
            {
                //Upserting value
                using (MotoGPContext db = new MotoGPContext())
                {
                    db.Teams.AddOrUpdate(modified);
                    db.SaveChanges();
                    teams = db.Teams.ToList();
                    Teams_dgw.ItemsSource = teams;
                }
            }
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            MotoGPTeam modified = (MotoGPTeam)Teams_dgw.SelectedItem;

            //if nothing selected, do not respond
            if (modified == null)
            {
                return;
            }

            Delete delete = new Delete();
            delete.ShowDialog();

            //Clicked OK
            if (delete.DialogResult.HasValue && delete.DialogResult.Value)
            {
                //Upserting value
                using (MotoGPContext db = new MotoGPContext())
                {
                    db.Teams.Remove(db.Teams.Find(modified.Name));
                    db.SaveChanges();
                    teams = db.Teams.ToList();
                    Teams_dgw.ItemsSource = teams;
                }
            }
        }

        private void News_btn_Click(object sender, RoutedEventArgs e)
        {
            News news = new News();
            news.ShowDialog();
        }
    }
}
