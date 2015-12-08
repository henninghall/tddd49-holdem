using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Windows;
using System.Windows.Controls;
using tddd49_holdem.Players;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using tddd49_holdem.actions;

namespace tddd49_holdem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private HoldemContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new HoldemContext();


            Table table = _context.Tables.First();
            List<Player> allPlayers = table.AllPlayers;

            MainPanel.DataContext = table;
            LogBoxControl.DataContext = table.LogBox;
            SetPlayersDataContext(allPlayers);

            table.StartNewRound();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            new Fold(activePlayer).Execute();
            UpdateUI();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            new Check(activePlayer).Execute();
            UpdateUI();

        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            new Call(activePlayer).Execute();
            UpdateUI();

        }

        private void RaiseButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            new Raise(activePlayer).Execute();
            UpdateUI();

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

        private async void UpdateUI() {
            _context.SaveChanges();

            RefreshContext();
            /*
            await _context.SaveChangesAsync();
            /*_context.Dispose();
            _context = new HoldemContext();

            Table table = _context.Tables.First();
            
            List<Player> allPlayers = table.AllPlayers;
            MainPanel.DataContext = table;
            LogBoxControl.DataContext = table.LogBox;
            SetPlayersDataContext(allPlayers);
            // 
    */
        }


        /// <summary>
        /// Refreshing context to detect database changes outside the program
        /// From: http://stackoverflow.com/questions/18169970/how-do-i-refresh-dbcontext
        /// </summary>
        private void RefreshContext()
        {
            var context = ((IObjectContextAdapter)_context).ObjectContext;
            var refreshableObjects = (from entry in context.ObjectStateManager.GetObjectStateEntries(
                                                        EntityState.Added
                                                       | EntityState.Deleted
                                                       | EntityState.Modified
                                                       | EntityState.Unchanged)
                                      where entry.EntityKey != null
                                      select entry.Entity).ToList();

            List<object> refreshableObjects2 = refreshableObjects.Where(refreshableObject => refreshableObject != null).ToList();

            context.Refresh(RefreshMode.StoreWins, refreshableObjects2);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            _context.Dispose();
        }

        private void Seed()
        {

            Table table1 = new Table();
            Player p1 = new HumanPlayer("Bamse");
            Player p2 = new HumanPlayer("Skalman");
            table1.AttachPlayer(p1);
            table1.AttachPlayer(p2);
            _context.Tables.Add(table1);
            _context.SaveChanges();

        }
    }
}
