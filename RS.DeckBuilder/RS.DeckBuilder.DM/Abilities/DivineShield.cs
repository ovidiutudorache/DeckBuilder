using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class DivineShield : BaseAbility
    {
        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            return null;
        }

        public override BaseAbility Clone()
        {
            return new DivineShield();
        }
    }
}