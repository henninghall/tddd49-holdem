using System.Windows;
using System.Windows.Controls;
using tddd49_holdem.Players;
using System.Linq;

namespace tddd49_holdem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {


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

            using (HoldemContext db = new HoldemContext())
            {
                Player p1 = new HumanPlayer("Bamse 2");
                Player p2 = new AiPlayer("Skalman 2");
                Player p3 = new AiPlayer("Lille Skutt 2");
                PlayerSlot1.DataContext = p1;
                PlayerSlot2.DataContext = p2;
                PlayerSlot3.DataContext = p3;

                Table table = new Table();
                table.AttachPlayer(p1);
                table.AttachPlayer(p2);
                table.AttachPlayer(p3);

                Window.DataContext = table;
                LogBoxControl.DataContext = table.LogBox;




                db.Players.Add(p1);
                db.Players.Add(p2);
                db.Players.Add(p3);
                db.SaveChanges();

                // Display all Blogs from the database 
                var query = from p in db.Players
                            orderby p.Name
                            select p;

                foreach (var item in query)
                {
                    table.LogBox.Log("Player DATABASE: " + item.Name);
                }



                table.StartRound();

            }



        }

        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.Fold.Execute();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.Check.Execute();
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.Call.Execute();
        }

        private void RaiseButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.Raise.Execute();
        }
    }
}
