using System;
using System.ComponentModel;
using System.Threading;

namespace tddd49_holdem.Players
{
    public class AiPlayer : Player
    {
        public AiPlayer(string name) : base(name) {}
        public AiPlayer() {}
        public override bool IsUsingGui { get; } = false;
        
        public override void RequestActionExcecution() {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
          Thread.Sleep(2000);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {

            // The net draw type is calculated to make draws only from table cards insignificant. 
            int handDrawTypeValue = (int) Table.Rules.GetDrawType(GetAllCards());
            int tableDrawTypeValue = (int) Table.Rules.GetDrawType(Table.CardsOnTable);
            int netDrawType = handDrawTypeValue - tableDrawTypeValue;

            // In cases where a check is possible lays the decision between checking or raising.
            // Probaility of doing a raise increases along with better draw types. 
            if (Check.IsValid()) {
                double raiseProbability = 0.2;
                if (netDrawType > (int) DrawType.HighCards) raiseProbability += 0.4;
                else if (netDrawType > (int) DrawType.OnePair) raiseProbability += 0.6;
                if (new Random().NextDouble() < raiseProbability) Raise.Execute();
                else Check.Execute();
            }

            // In cases where a check is NOT possible lays the decision between folding, calling or raising.
            // Probaility of doing a raise and call increases along with better draw types. 
            else if (!Check.IsValid())
            {
                double raiseProbability = 0.1;
                double callProbability = 0.3;
                if (netDrawType > (int) DrawType.HighCards) {
                    raiseProbability += 0.2;
                    callProbability += 0.5;
                }
                else if (netDrawType > (int) DrawType.OnePair) {
                    raiseProbability += 0.5;
                    callProbability += 0.7;
                }
                if (new Random().NextDouble() < raiseProbability) Raise.Execute();
                else if (new Random().NextDouble() < callProbability) Call.Execute();
                else Fold.Execute();
            }
            
            throw new InvalidActionException("AI-Player couldn't find any action to excecute.");
        }


    }
}