using System.Windows;
using System.Windows.Input;
using tddd49_holdem;

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
            Player p1 = new Player("Player 1");
            Player p2 = new Player("Player 2");
           
            tddd49_holdem.Table table = new tddd49_holdem.Table();
            table.AttachPlayer(p1);
            table.AttachPlayer(p2);

            this.DataContext = table;
            Slot1.DataContext = p1;
            //Slot2.DataContext = p2;

            p1.Name = "New p1 name";
            p2.Name = "New p2 name";
        }

        private void PnlMainGrid_OnMouseDown(object sender, MouseButtonEventArgs e) {
            MessageBox.Show("You clicked me at down " + e.GetPosition(this).ToString());
        }

        private void pnlMainGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("You clicked me at up " + e.GetPosition(this).ToString());
        }
    }
}
