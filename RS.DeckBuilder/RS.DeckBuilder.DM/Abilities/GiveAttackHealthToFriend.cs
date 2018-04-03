using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class GiveAttackHealthToFriend : BaseAbility
    {
        public int Attack { get; set; }
        public int Health { get; set; }
        public bool IsTemporary { get; set; }

        public GiveAttackHealthToFriend(int attack, int health, bool isTemporary = false)
        {
            Attack = attack;
            Health = health;
            IsTemporary = isTemporary;
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            List<Card> otherCards = player.Board.Cards.Where(el => el.Id != card.Id).ToList();
            otherCards.Shuffle();

            var cardToImprove = otherCards.FirstOrDefault();
            if (cardToImprove == null)
                return null;

            cardToImprove.Attack += Attack;
            cardToImprove.Health += Health;

            if (IsTemporary)
            {
                cardToImprove.TemporaryAttack += Attack;
                cardToImprove.TemporaryHealth += Health;
            }

            if (player.Depth == 0)
                Console.WriteLine("Given {0} attack and {1} health to {2}", Attack, Health, card.Name);

            return null;
        }

        public override BaseAbility Clone()
        {
            return new GiveAttackHealthToFriend(Attack, Health);
        }
    }
}