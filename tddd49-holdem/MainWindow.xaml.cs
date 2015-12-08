using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using tddd49_holdem.Players;
using System.Linq;
using tddd49_holdem.actions;

namespace tddd49_holdem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static HoldemContext Db = new HoldemContext();
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Table table = Db.Tables.First();
            DataBindTableToWindow(table);
            table.ContinueRound();
        }

        private void DataBindTableToWindow(Table table) {
            MainPanel.DataContext = table;
            LogBoxControl.DataContext = table.LogBox;
            SetPlayersDataContext(table.AllPlayers);
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

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            Db.Dispose();
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

        private void Seed()
        {
            Table table1 = new Table();
            Player p1 = new HumanPlayer("Bamse");
            Player p2 = new HumanPlayer("Skalman");
            table1.AttachPlayer(p1);
            table1.AttachPlayer(p2);
            Db.Tables.Add(table1);
            Db.SaveChanges();
        }
    }
}
