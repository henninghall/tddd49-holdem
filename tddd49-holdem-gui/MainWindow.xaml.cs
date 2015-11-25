using System.Windows;
using System.Windows.Controls;
using tddd49_holdem;
using tddd49_holdem.actions;

namespace tddd49_holdem_gui
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

            Player p1 = new Player("Bamse");
            Player p2 = new Player("Skalman");
            PlayerSlot1.DataContext = p1;
            PlayerSlot2.DataContext = p2;

            Table table = new Table();
            table.AttachPlayer(p1);
            table.AttachPlayer(p2);

            Window.DataContext = table;
            LogBoxControl.DataContext = table.LogBox;
            
            table.StartGuiGame();
        }

        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.Table.MakeMove(new Fold(activePlayer));
            activePlayer.Table.LogBox.Log(activePlayer.Name + " folded!");
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.Table.MakeMove(new Check(activePlayer));
            activePlayer.Table.LogBox.Log(activePlayer.Name + " checked!");
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.Table.MakeMove(new Call(activePlayer));
            activePlayer.Table.LogBox.Log(activePlayer.Name + " called!");
        }

        private void RaiseButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.Table.MakeMove(new Raise(activePlayer));
            activePlayer.Table.LogBox.Log(activePlayer.Name + " raised!");
        }
    }
}
