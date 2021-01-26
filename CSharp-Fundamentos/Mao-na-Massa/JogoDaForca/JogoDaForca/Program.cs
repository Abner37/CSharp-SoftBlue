using System;

namespace JogoDaForca
{
    class Program
    {
        static void Main()
        {
            // Inicia o jogo
            var game = new Game(3);
            game.Play();
        }
    }
}
