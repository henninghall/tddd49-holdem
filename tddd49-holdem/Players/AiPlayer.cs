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

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Check.IsValid()) Check.Execute();
            else if (Fold.IsValid()) Fold.Execute();
            else throw new InvalidActionException("AI-Player couldn't find any action to excecute.");
        }


    }
}