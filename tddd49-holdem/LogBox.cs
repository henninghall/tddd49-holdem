using System;
using System.Collections.ObjectModel;

namespace tddd49_holdem
{
    public class LogBox
    {
        public int LogBoxId { get; set; }
        public virtual ObservableCollection<Row> TextRows { get; set; }

        public LogBox() {
            TextRows = new ObservableCollection<Row>();
        }

        public void Log(string text) {
           TextRows.Insert(0,new Row(text));
        }
    }

    public class Row
    {
        public int RowId { set; get; }
        public string Text { set; get; }
        public string Color { set; get; }
        public string TimeStamp { set; get; }

        public Row(string text) {
            Text = text;
        }

        public Row() { }
    }

}