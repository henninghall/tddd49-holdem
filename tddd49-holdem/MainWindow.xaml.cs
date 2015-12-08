using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;
using tddd49_holdem.Players;
using System.Linq;
using System.Net;
using tddd49_holdem.actions;

namespace tddd49_holdem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public static HoldemContext db = new HoldemContext();
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //  SQLiteConnection sqlConnection = new SQLiteConnection("Data Source=HoldemDatabase.sqlite;Version=3;New=True;Compress=True");
            //sqlConnection.Open();
            //SQLiteCommand sqlCommand = sqlConnection.CreateCommand();
            // sqlCommand.CommandText = "CREATE TABLE test (id integer primary key, text varchar(100));";
            // sqlCommand.ExecuteNonQuery();
            PlayerSlot1.DataContext = db.Tables.First().AllPlayers.First();
            PlayerSlot2.DataContext = db.Tables.First().AllPlayers[1];

            Table table = db.Tables.First();
            Window.DataContext = table;
            LogBoxControl.DataContext = table.LogBox;


            table.StartRound();





        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            db.Dispose();
        }

        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();

            Player activePlayer = (Player)((Button)sender).Tag;
            new Fold(activePlayer).Execute();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();

            Player activePlayer = (Player)((Button)sender).Tag;
            new Check(activePlayer).Execute();
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();

            Player activePlayer = (Player)((Button)sender).Tag;
            new Call(activePlayer).Execute();
        }

        private void RaiseButton_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();

            Player activePlayer = (Player)((Button)sender).Tag;
            new Raise(activePlayer).Execute();
        }
    }
}
