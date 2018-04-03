using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class ReduceCardCost : BaseAbility
    {
        public CardTypes CardType { get; set; }
        public int Reduction { get; set; }
        public int MinimumCost { get; set; }

        public ReduceCardCost(CardTypes cardType, int reduction, int minimumCost = 1)
        {
            CardType = cardType;
            Reduction = reduction;
            MinimumCost = minimumCost;
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            return null;
        }

        public override BaseAbility Clone()
        {
            return new ReduceCardCost(CardType, Reduction, MinimumCost);
        }
    }
}