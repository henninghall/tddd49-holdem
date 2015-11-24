﻿using System;
using System.Collections.ObjectModel;
using System.Net.Mime;

namespace tddd49_holdem
{
    public class LogBox
    {
        public ObservableCollection<Row> TextRows { get; set; }

        public LogBox() {
            TextRows = new ObservableCollection<Row>();
        }

        public void Log(string text) {
           TextRows.Insert(0,new Row(text));
        }
    }

    public class Row
    {
        public string Text { set; get; }
        public string Color { set; get; }
        public string TimeStamp { set; get; }

        public Row(string text) {
            Text = text;
        }

        public Row() { }
    }

}