using System;
using System.Globalization;
using System.Windows.Data;
using tddd49_holdem;

namespace tddd49_holdem_gui
{
    public class IsActivePlayerConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture) {
            Player player = value as Player;
        
            // TODO: Change player name to player
            if (player != null && player.Name.Equals(player.Table.ActivePlayer.Name)) {
                return true;
            }
            return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter,
                    System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}