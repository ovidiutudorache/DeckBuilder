using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class DealDamageToEnemy : BaseAbility
    {
        public int Damage { get; set; }

        public DealDamageToEnemy(int damage)
        {
            Damage = damage;
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            player.Enemy.Health -= Damage;

            if (player.Depth == 0)
                Console.WriteLine("Dealt {0} to enemy.", Damage);

            return null;
        }

        public override BaseAbility Clone()
        {
            return new DealDamageToEnemy(Damage);
        }
    }
}