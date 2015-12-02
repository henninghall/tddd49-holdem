using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using tddd49_holdem.Players;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using tddd49_holdem.actions;

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

            /*
            Player p1 = new HumanPlayer("Bamse");
            Player p2 = new AiPlayer("Skalman");
            Player p3 = new AiPlayer("Lille Skutt");
            Player p4 = new AiPlayer("Farmor");
            Player p5 = new AiPlayer("Draken");
            Player p6 = new AiPlayer("Skorpan");
            Player p7 = new AiPlayer("Katla");
            Player p8 = new AiPlayer("Snusmumriken");
            Player p9 = new AiPlayer("Herr Nilsson");
            Player p10 = new AiPlayer("Pikachu");
          */



            Table table = new Table();
            /* table.AttachPlayer(p1);
             table.AttachPlayer(p2);
             table.AttachPlayer(p3);
             */



            /*
          Db.Players.Add(p4);
          Db.Players.Add(p5);
          Db.Players.Add(p3);
          Db.SaveChanges();
          */

            var query = from p in Table.Db.Players
                        orderby p.Name
                        select p;

            table.AttachPlayers(query);

            SetPlayersDataContext(table.AllPlayers);
            MainPanel.DataContext = table;
            LogBoxControl.DataContext = table.LogBox;



            table.StartRound();
        }

        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            new Fold(activePlayer).Execute();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            new Check(activePlayer).Execute();
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            new Call(activePlayer).Execute();
        }

        private void RaiseButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            new Raise(activePlayer).Execute();
        }

        private void SetPlayersDataContext(IReadOnlyList<Player> players)
        {
            List<PlayerPanel> allPlayerPanels = GetLogicalChildCollection<PlayerPanel>(MainPanel);
            List<PlayerPanel> sortedAllPlayerPanels = allPlayerPanels.OrderBy(o => o.Name).ToList();
            for (int i = 0; i < players.Count; i++)
            {
                sortedAllPlayerPanels[i].DataContext = players[i];
            }
        }

        private static List<T> GetLogicalChildCollection<T>(object parent) where T : DependencyObject
        {
            List<T> logicalCollection = new List<T>();
            GetLogicalChildCollection(parent as DependencyObject, logicalCollection);
            return logicalCollection;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }
    }
}
