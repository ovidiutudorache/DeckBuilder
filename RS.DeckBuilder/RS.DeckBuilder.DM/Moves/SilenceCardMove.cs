using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM.Moves
{
    public class SilenceCardMove : BaseCardMove
    {
        public SilenceCardMove(Player player, Card card) : base(player, card)
        {
        }

        public override List<BaseMove> Play(bool log = false)
        {
            Card.Silence();

            return null;
        }

        public override BaseMove TransformInGame(Game game)
        {
            var player = game.GetPlayerById(Player.Id);

            var move = new SilenceCardMove(player, player.Board.GetCardById(Card.Id));

            return move;
        }
    }
}