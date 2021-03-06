﻿using tddd49_holdem.Players;

namespace tddd49_holdem.actions
{
    public class Raise : PlayerAction
    {
        public Raise(Player player) : base(player) { }

        public override bool IsValid()
        {
            // must have enough chips
            return (Player.CurrentBet + Player.ChipsOnHand) > Player.Table.Players.GetHighestBet();
        }

        public override void Execute()
        {
            int bet = 10;
            // bet = raise + diff to max bet. 
            bet += (Player.Table.Players.GetHighestBet() - Player.CurrentBet);
            Player.Bet(bet);

            // forces every active player to move again...
            Player.Table.Players.SetNoBet();
            Player.Table.LogBox.Log(Player.Name + " raised!");
            Player.Table.ReactOnActionExecution();
        }
    }
}