using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class PlaySpellPlayCard : BaseAbility
    {
        public string CardName { get; set; }
        public int Count { get; set; }

        public PlaySpellPlayCard(string cardName, int count = 1)
        {
            CardName = cardName;
            Count = count;
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            if (card.Type != CardTypes.Ability)
                return null;

            for (int i = 0; i < Count; i++)
            {
                Card cardToPlay = new CardsUnivers().GetCardByName("Violet Apprentice");
                var move = new Moves.PlayCardMove(player, cardToPlay)
                {
                    HasManaCost = false
                };

                move.Play();
            }

            return null;
        }

        public override BaseAbility Clone()
        {
            return new PlaySpellPlayCard(CardName, Count);
        }
    }
}