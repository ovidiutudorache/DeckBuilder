using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM.Abilities
{
    public abstract class BaseAbility
    {
        public abstract AbilityResult Apply(Player player, Card card, Card cardPlayed);
        public abstract BaseAbility Clone();
    }
}
