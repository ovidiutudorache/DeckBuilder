using RS.DeckBuilder.DM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder
{
    public class Game
    {
        const int deckNumberOfCards = 30;

        private Player player1 = null;
        private Player player2 = null;

        public Game()
        {
            //player1 = new Player(new CardsRepository().GetStandardDeck(deckNumberOfCards));
            //player2 = new Player(new CardsRepository().GetStandardDeck(deckNumberOfCards));

            player1 = new Player(new CardsUnivers().GetRandomDeck(deckNumberOfCards));
            player2 = new Player(new CardsUnivers().GetRandomDeck(deckNumberOfCards));

        }

        public void Start()
        {
            var game = new DM.Game(player1, player2);
            game.Start();
        }
    }
}