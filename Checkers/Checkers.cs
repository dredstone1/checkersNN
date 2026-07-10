using Checkers.models;

namespace Checkers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool load = false,
                save = false,
                train = false;

            if (args.Contains("-t"))
                train = true;
            if (args.Contains("-s"))
                save = true;
            if (args.Contains("-l"))
                load = true;

            Game game = new Game();
            game.Start(load, save, train);
        }
    }
}
