using RS.DeckBuilder.DM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM
{
    public class Game
    {
        public Player Player1 = null;
        public Player Player2 = null;
        public Board Board = null;

        const int maxMana = 10;

        private int round = 0;

        public Game(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;

            Board = new Board(player1, player2, player1.Board, player2.Board);

            Player1.Enemy = player2;
            Player2.Enemy = player1;
        }

        public void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                Player1.DrawCard();
                Player2.DrawCard();
            }

            while (GetResult() == 0)
            {
                PlayNextRound();
            }

            Console.WriteLine(GetResult());
            Console.ReadKey();
        }

        private int GetResult()
        {
            if (Player1.Health <= 0)
                return 2;

            if (Player2.Health <= 0)
                return 1;

            return 0;
        }

        public void PlayNextRound(int? startMana = null, bool drawCards = true)
        {
            round++;

            Player1.MaxMana++;
            Player2.MaxMana++;

            Player1.Mana = startMana.GetValueOrDefault(Math.Min(Player1.MaxMana, maxMana));
            Player2.Mana = startMana.GetValueOrDefault(Math.Min(Player2.MaxMana, maxMana));

            if (drawCards)
            {
                Player1.DrawCard();
                Player2.DrawCard();
            }

            WriteGame();

            if (!Debugger.IsAttached)
                Console.ReadKey();

            Player1.Board.Cards.ForEach(el => el.NoActionsAvailable = el.MaxActionsAvailable);
            Player1.PlayRound(this, 0, isStart: true);
            if (!Debugger.IsAttached)
                Console.ReadKey();

            WriteGame();
            if (!Debugger.IsAttached)
                Console.ReadKey();

            if (GetResult() != 0)
                return;

            Player2.Board.Cards.ForEach(el => el.NoActionsAvailable = el.MaxActionsAvailable);
            Player2.PlayRound(this, 1, isStart: true);
            if (!Debugger.IsAttached)
                Console.ReadKey();
        }

        private void WriteGame()
        {
            Console.Clear();
            Console.WriteLine("Player 1: {0} health and {1} mana - {2}", Player1.Health, Player1.Mana, Player1.Id);
            Console.WriteLine();
            Console.WriteLine(Player1.Hand);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(Player1.Board);
            Console.WriteLine();
            Console.WriteLine(Player2.Board);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(Player2.Hand);
            Console.WriteLine("Player 2: {0} health and {1} mana - {2}", Player2.Health, Player2.Mana, Player2.Id);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        public Player GetPlayerById(Guid id)
        {
            if (Player1.Id == id)
                return Player1;

            if (Player2.Id == id)
                return Player2;

            return null;
        }

        public Game Clone()
        {
            var p1 = Player1.Clone();
            var p2 = Player2.Clone();

            p1.Enemy = p2;
            p2.Enemy = p1;

            var g = new Game(p1, p2);
            g.Board = new Board(p1, p2, p1.Board, p2.Board);

            return g;
        }
    }
}