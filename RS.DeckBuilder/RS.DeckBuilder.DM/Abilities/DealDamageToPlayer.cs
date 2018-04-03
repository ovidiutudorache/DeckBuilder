using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class DealDamageToPlayer : BaseAbility
    {
        public int Damage { get; set; }

        public DealDamageToPlayer(int damage)
        {
            Damage = damage;
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            player.Health -= Damage;

            if (player.Depth == 0)
                Console.WriteLine("Dealt {0} to player.", Damage);

            return null;
        }

        public override BaseAbility Clone()
        {
            return new DealDamageToPlayer(Damage);
        }
    }
}