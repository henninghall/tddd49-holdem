using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using tddd49_holdem;
using tddd49_holdem.actions;

namespace tddd49_holdem_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public LogBox logBox;

        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Player p1 = new Player("Barbastark");
            Player p2 = new Player("Skalman");
            PlayerSlot1.DataContext = p1;
            PlayerSlot2.DataContext = p2;

            Table table = new Table();
            table.AttachPlayer(p1);
            table.AttachPlayer(p2);
            Window.DataContext = table;

            logBox = new LogBox();
            LogBox.DataContext = logBox;

            table.StartGuiGame();
        }

        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.MakeMove(new Fold(activePlayer));
            logBox.Log(activePlayer.Name + " folded!");
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.MakeMove(new Check(activePlayer));
            logBox.Log(activePlayer.Name + " checked!");
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.MakeMove(new Call(activePlayer));
            logBox.Log(activePlayer.Name + " called!");
        }

        private void RaiseButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.MakeMove(new Raise(activePlayer));
            logBox.Log(activePlayer.Name + " raised!");
        }
    }
}
