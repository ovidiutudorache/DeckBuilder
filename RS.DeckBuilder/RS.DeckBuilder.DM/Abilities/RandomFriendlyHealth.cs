using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class RandomFriendlyHealth : BaseAbility
    {
        public int Health { get; private set; }

        public RandomFriendlyHealth(int health)
        {
            Health = health;
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            List<Card> otherCards = player.Board.Cards.Where(el => el.Id != card.Id).ToList();
            otherCards.Shuffle();

            Card destinationCard = otherCards.FirstOrDefault();
            if (destinationCard == null)
                return null;

            destinationCard.Health++;

            if (player.Depth == 0)
                Console.WriteLine("{0} gained {1} health", destinationCard.Name, Health);

            return null;
        }

        public override BaseAbility Clone()
        {
            return new RandomFriendlyHealth(Health);
        }
    }
}