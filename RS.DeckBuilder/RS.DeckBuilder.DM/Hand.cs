using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM
{
    public class Hand : Deck
    {

        public static Hand FromDeck(Deck deck)
        {
            var h = new Hand();
            if (deck != null && deck.Cards != null && deck.Cards.Any())
                h.Cards.AddRange(deck.Clone().Cards);

            return h;
        }
    }
}