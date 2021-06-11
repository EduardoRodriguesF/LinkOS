using LinkOS.Scenes;
using Pirita;
using System;

namespace LinkOS {
    public static class Program {
        [STAThread]
        static void Main() {
            using (var game = new PiritaGame(1024, 576, new GameplayScene(), true))
                game.Run();
        }
    }
}
