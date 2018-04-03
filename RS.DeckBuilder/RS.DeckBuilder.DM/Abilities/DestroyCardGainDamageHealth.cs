using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class DestroyCardGainDamageHealth : BaseAbility
    {
        public Races RaceToDestroy { get; private set; }
        public int Attack { get; private set; }
        public int Health { get; private set; }

        public DestroyCardGainDamageHealth(Races raceToDestroy, int attack, int health)
        {
            RaceToDestroy = raceToDestroy;
            Attack = attack;
            Health = health;
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            var cards = new List<Card>();

            List<Card> otherCards = player.Board.Cards.Where(el => el.Id != card.Id && el.Race == RaceToDestroy).ToList();
            if (otherCards.Any())
                cards.AddRange(otherCards);

            List<Card> enemyCards = player.Enemy.Board.Cards.Where(el => el.Race == RaceToDestroy).ToList();
            if (enemyCards.Any())
                cards.AddRange(enemyCards);

            cards.Shuffle();

            Card cardToDestroy = cards.FirstOrDefault();
            if (cardToDestroy == null)
                return null;

            var playersCard = player.Board.GetCardById(cardToDestroy.Id) != null ? player : player.Enemy;

            cardToDestroy.Destroy(player.Enemy, playersCard);

            card.Attack += Attack;
            card.Health += Health;

            if (player.Depth == 0)
                Console.WriteLine("Destroyed {0} and gained {1} attack and {2} health", cardToDestroy.Name, Attack, Health);

            return null;
        }

        public override BaseAbility Clone()
        {
            return new DestroyCardGainDamageHealth(RaceToDestroy, Attack, Health);
        }
    }
}