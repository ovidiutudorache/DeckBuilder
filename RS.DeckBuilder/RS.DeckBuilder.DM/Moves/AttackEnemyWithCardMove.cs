using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM.Moves
{
    public class AttackEnemyWithCardMove : BaseCardMove
    {
        public AttackEnemyWithCardMove(Player player, Card card) : base(player, card) { }

        public override List<BaseMove> Play(bool log = false)
        {
            Player.Enemy.Health -= Card.Attack;
            Card.NoActionsAvailable--;

            if (log)
                Console.WriteLine("{0} attack enemy hero for {1} damage.", Card.Name, Card.Attack);

            return null;
        }

        public override BaseMove TransformInGame(Game game)
        {
            var player = game.GetPlayerById(Player.Id);

            return new AttackEnemyWithCardMove(player, player.Board.GetCardById(Card.Id));
        }
    }
}