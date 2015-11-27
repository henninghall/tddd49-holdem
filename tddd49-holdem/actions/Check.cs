using System;
using System.Windows.Input;
using tddd49_holdem.Players;

namespace tddd49_holdem.actions
{ 
    public class Check : PlayerAction
    {
       
        public Check(Player player) : base(player) { }


        public override bool IsValid()
        {
            return (Player.Table.GetHighestBetPlayers().Contains(Player));
        }

        public override void Execute()
        {
            Player.Table.AfterMove.Enqueue(Player.Table.BeforeMove.Dequeue());

            Player.Table.LogBox.Log(Player.Name + " checked!");
            Player.Table.ReactOnActionExecution();
        }
    }



}