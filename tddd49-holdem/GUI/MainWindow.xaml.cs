using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Windows;
using System.Windows.Controls;
using tddd49_holdem.Players;
using System.Linq;
using tddd49_holdem.actions;

namespace tddd49_holdem.GUI
{
    public partial class MainWindow
    {
        public static HoldemContext Db = new HoldemContext();

        public MainWindow()
        {
            InitializeComponent();

            // Gets the first table from database
            // This could be inproved to support multiple tables 
            Table table = Db.Tables.First();
            DataBindTableToWindow(table);

            table.StartNewRound();
            //table.ContinueRound();
        }

        /// <summary>
        /// 1. Saves the current object modifications to the database.
        /// 2. Updates the current state with data from the database if 
        /// it has been changed from outside the application.
        /// </summary>
        public static void SyncState()
        {
            Db.SaveChanges();

            ObjectContext context = ((IObjectContextAdapter)Db).ObjectContext;
            List<object> refreshableObjects = Db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
            context.Refresh(RefreshMode.StoreWins, refreshableObjects);

        }

        /// <summary>
        /// Databinds the table and all its components to the window.
        /// </summary>
        /// <param name="table"></param>
        private void DataBindTableToWindow(Table table)
        {
            MainPanel.DataContext = table;
            LogBoxControl.DataContext = table.LogBox;
            SetPlayersDataContext(table.Players);
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

        private static void GetLogicalChildCollection<T>(DependencyObject parent, ICollection<T> logicalCollection) where T : DependencyObject
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
            Player p2 = new AiPlayer("Skalman");
            table1.AttachPlayer(p1);
            table1.AttachPlayer(p2);
            Db.Tables.Add(table1);
            Db.SaveChanges();
        }
    }
}
