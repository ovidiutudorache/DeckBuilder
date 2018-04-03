using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class PlayCardGainAttack : BaseAbility
    {
        public Races Race { get; set; }
        public int Attack { get; set; }

        public PlayCardGainAttack(Races race, int attack)
        {
            Race = race;
            Attack = attack;
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            if (card.Id != cardPlayed.Id && cardPlayed.Race == Race)
            {
                card.Attack += Attack;

                if (player.Depth == 0)
                    Console.WriteLine("{0} gained {1} attack", card.Name, Attack);
            }

            return null;
        }

        public override BaseAbility Clone()
        {
            return new PlayCardGainAttack(Race, Attack);
        }
    }
}