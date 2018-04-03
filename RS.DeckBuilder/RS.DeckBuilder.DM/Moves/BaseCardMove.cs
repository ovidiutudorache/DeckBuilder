using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM.Moves
{
    public abstract class BaseCardMove: BaseMove
    {
        public Card Card { get; set; }

        public BaseCardMove(Player player, Card card): base(player)
        {
            Card = card;
        }

        public abstract override List<BaseMove> Play(bool log = false);

        public abstract override BaseMove TransformInGame(Game game);
    }
}