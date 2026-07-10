using Checkers.models;

namespace Checkers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool load = false,
                save = false,
                train = false,
                AIB = false,
                AIW = false;

            if (args.Contains("-t"))
                train = true;
            if (args.Contains("-s"))
                save = true;
            if (args.Contains("-l"))
                load = true;
            if (args.Contains("-b"))
                AIB = true;
            if (args.Contains("-w"))
                AIW = true;

            Game game = new Game();
            game.Start(AIB, AIW, load, save, train);
        }
    }
}
