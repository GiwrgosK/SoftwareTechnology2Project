using MySiberianWarfarePOC1.Components;
using MySiberianWarfarePOC1.GameObjects;
using MySiberianWarfarePOC1.Interfaces;

namespace MySiberianWarfarePOC1 {
    public class Program {
        public static void Main() {
            SWGameState gameState = SWGameState.Instance;
            gameState.InitializeGame();
            Console.WriteLine("Done with the initialization!");
        }
    }
}