using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM.Moves
{
    public class AttackCardWithCardMove : BaseCardMove
    {
        public Card EnemyCard { get; set; }

        public AttackCardWithCardMove(Player player, Card card, Card enemyCard) : base(player, card)
        {
            EnemyCard = enemyCard;
        }

        public override List<BaseMove> Play(bool log = false)
        {
            if (!EnemyCard.HasDivineShield)
                EnemyCard.ApplyDamage(Player, Card.Attack, Player.Enemy);
            else EnemyCard.StartTurnAbilities.RemoveAll(el => el is Abilities.DivineShield);

            if (!Card.HasDivineShield)
                Card.ApplyDamage(Player.Enemy, EnemyCard.Attack, Player);
            else Card.StartTurnAbilities.RemoveAll(el => el is Abilities.DivineShield);

            Card.NoActionsAvailable--;

            if (Card.HasStealth)
                Card.StartTurnAbilities.RemoveAll(el => el is Abilities.Stealth);

            if (log)
                Console.WriteLine("{0} attacks {1} for {2} damage.", Card.Name, EnemyCard.Name, Card.Attack);

            return null;
        }

        public override BaseMove TransformInGame(Game game)
        {
            var player = game.GetPlayerById(Player.Id);

            return new AttackCardWithCardMove(player, player.Board.GetCardById(Card.Id), player.Enemy.Board.GetCardById(EnemyCard.Id));
        }
    }
}