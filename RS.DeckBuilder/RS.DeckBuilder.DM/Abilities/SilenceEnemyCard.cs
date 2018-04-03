using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class SilenceEnemyCard : BaseAbility
    {

        public SilenceEnemyCard()
        {
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            AbilityResult result = null;

            foreach (var item in player.Enemy.Board.Cards)
            {
                if (!item.HasAbilities)
                    continue;

                if (result == null)
                    result = new AbilityResult()
                    {
                        Moves = new List<Moves.BaseMove>()
                    };

                result.Moves.Add(new Moves.SilenceCardMove(player.Enemy, item));
            }

            return result;
        }

        public override BaseAbility Clone()
        {
            return new SilenceEnemyCard();
        }
    }
}