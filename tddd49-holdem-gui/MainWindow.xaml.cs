using System.Windows;
using System.Windows.Controls;
using tddd49_holdem;
using tddd49_holdem.actions;
using tddd49_holdem.Players;

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

            Player p1 = new HumanPlayer("Bamse");
            Player p2 = new AiPlayer("Skalman");
            Player p3 = new AiPlayer("Lille Skutt");
            PlayerSlot1.DataContext = p1;
            PlayerSlot2.DataContext = p2;
            PlayerSlot3.DataContext = p3;

            Table table = new Table();
            table.AttachPlayer(p1);
            table.AttachPlayer(p2);
            table.AttachPlayer(p3);

            Window.DataContext = table;
            LogBoxControl.DataContext = table.LogBox;

            table.StartRound();

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
