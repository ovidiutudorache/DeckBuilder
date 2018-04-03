using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM
{
    public class PlayerBoard: Deck
    {
        public int MaxSize = 7;

        public static PlayerBoard FromDeck(Deck deck)
        {
            var pb = new PlayerBoard();

            if (deck != null && deck.Cards != null && deck.Cards.Any())
                pb.Cards.AddRange(deck.Clone().Cards);

            return pb;
        }

        public void PlayStartTurnAbilities(Player player)
        {
            foreach (var card in Cards)
            {
                if (card.StartTurnAbilities != null)
                {
                    foreach (var ability in card.StartTurnAbilities)
                        ability.Apply(player, card, card);
                }
            }
        }

        public void PlayEndTurnAbilities(Player player)
        {
            foreach (var card in Cards)
            {
                if (card.EndTurnAbilities != null)
                {
                    foreach (var ability in card.EndTurnAbilities)
                        ability.Apply(player, card, card);
                }
            }
        }
    }
}