using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class GiveMana : BaseAbility
    {
        public int Mana { get; set; }
        public bool IsPermanent { get; set; }
        public bool ToEnemy { get; set; }

        public GiveMana(int mana, bool isPermanent, bool toEnemy = false)
        {
            Mana = mana;
            IsPermanent = isPermanent;
            ToEnemy = toEnemy;
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            var receiver = ToEnemy ? player.Enemy : player;

            if (IsPermanent)
                receiver.MaxMana += Mana;
            else receiver.Mana += Mana;

            return null;
        }

        public override BaseAbility Clone()
        {
            return new GiveMana(Mana, IsPermanent, ToEnemy);
        }
    }
}