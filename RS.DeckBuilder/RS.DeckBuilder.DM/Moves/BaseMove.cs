using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM.Moves
{
    public abstract class BaseMove
    {
        public Player Player { get; set; }

        public BaseMove(Player player)
        {
            Player = player;
        }

        public abstract List<BaseMove> Play(bool log = false);
        public abstract BaseMove TransformInGame(Game game);
    }
}