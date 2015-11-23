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
             
            Player p1 = new Player("Barbastark");
            Player p2 = new Player("Barbafin");
           
            Table table = new Table();
            table.AttachPlayer(p1);
            table.AttachPlayer(p2);

            PlayerSlot1.DataContext = p1;
            PlayerSlot2.DataContext = p2;
            

           // table.StartGame();
        }

    
    }
}
