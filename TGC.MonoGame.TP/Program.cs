using TGC.MonoGame.TP.Menu;
using System;

namespace TGC.MonoGame.TP
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MenuScreen())
                game.Run();
        }
    }
}
