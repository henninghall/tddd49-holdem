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
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Player p1 = new Player("Barbastark");
            Player p2 = new Player("Barbafin");

            Table table = new Table();
            table.AttachPlayer(p1);
            table.AttachPlayer(p2);

            PlayerSlot1.DataContext = p1;
            PlayerSlot2.DataContext = p2;

            Window.DataContext = table;

            table.StartGuiGame();
        }

        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.MakeMove(new Fold(activePlayer.Table, activePlayer));
            MessageBox.Show(activePlayer.Name + " checked!");
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            Table table = (Table)((Button)sender).Tag;
            Player activePlayer = table.ActivePlayer;
            activePlayer.MakeMove(new Check(activePlayer.Table, activePlayer));
            MessageBox.Show(activePlayer.Name + " checked!");
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.MakeMove(new Call(activePlayer.Table, activePlayer));
            MessageBox.Show(activePlayer.Name + " called!");
        }

        private void RaiseButton_Click(object sender, RoutedEventArgs e)
        {
            Player activePlayer = (Player)((Button)sender).Tag;
            activePlayer.MakeMove(new Raise(activePlayer.Table, activePlayer));
            MessageBox.Show(activePlayer.Name + " raised!");
        }
    }
}
