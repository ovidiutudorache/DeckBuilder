using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class Enrage : BaseAbility
    {
        public int Attack { get; private set; }
        public bool IsApplied { get; private set; }

        public Enrage(int attack)
        {
            Attack = attack;
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            if (IsApplied || card.Health >= card.BaseHealth)
                return null;

            card.Attack += Attack;
            IsApplied = true;

            if (player.Depth == 0)
                Console.WriteLine("Applied enrage with {0} attack to {1}", Attack, card.Name);

            return null;
        }

        public override BaseAbility Clone()
        {
            return new Enrage(Attack)
            {
                IsApplied = IsApplied
            };
        }
    }
}