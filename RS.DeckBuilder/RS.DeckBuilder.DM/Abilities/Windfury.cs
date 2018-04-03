using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class Windfury : BaseAbility
    {
        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            card.MaxActionsAvailable = 2;
            if (player.Depth == 0)
                card.NoActionsAvailable = 2;

            return null;
        }

        public override BaseAbility Clone()
        {
            return new Windfury();
        }
    }
}