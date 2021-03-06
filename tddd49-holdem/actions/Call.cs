﻿using System;
using tddd49_holdem.Players;

namespace tddd49_holdem.actions
{
    public class Call : PlayerAction
    {
        public Call(Player player) : base(player) { }

        public override bool IsValid()
        {
            // not valid if player has highest bet at the table already
            if (Player.Table.Players.GetHighestBetters().Contains(Player)) return false;
            // must have enough chips
            return Player.Table.Players.GetHighestBet() <= (Player.CurrentBet + Player.ChipsOnHand);
        }

        public override void Execute()
        {
            Player.Bet(Player.Table.Players.GetHighestBet() - Player.CurrentBet);
            //Player.Table.AfterMove.Enqueue(Player.Table.BeforeMove.Dequeue());
            Player.Table.LogBox.Log(Player.Name + " called!");
            Player.Table.ReactOnActionExecution();
        }
    }
}