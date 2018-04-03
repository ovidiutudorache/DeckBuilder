using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM.Moves
{
    public class PlayCardMove : BaseCardMove
    {
        public bool HasManaCost { get; set; }
        public PlayCardMove(Player player, Card card) : base(player, card)
        {
            HasManaCost = true;
        }

        public override List<BaseMove> Play(bool log = false)
        {
            if (Player.Board.Cards == null)
                Player.Board.Cards = new List<Card>();

            if (Player.Board.Cards.Count == Player.Board.MaxSize)
                return null;

            Player.Board.Cards.Add(Card);
            Player.Hand.Cards.Remove(Card);

            if (HasManaCost)
                Player.Mana -= Card.GetCost(Player);

            if (log)
                Console.WriteLine("Player plays {0} to board and has {1} mana left.", Card.Name, Player.Mana);

            List<BaseMove> moves = null;

            if (Card.BirthAbilities != null && Card.BirthAbilities.Any())
            {
                foreach (var ability in Card.BirthAbilities)
                {
                    Abilities.AbilityResult result = ability.Apply(Player, Card, Card);
                    if (result != null && result.Moves != null && result.Moves.Any())
                    {
                        if (moves == null)
                            moves = new List<BaseMove>();

                        moves.AddRange(result.Moves);
                    }
                }
            }

            foreach (var card in Player.Board.Cards)
            {
                if (card.PlayCardAbilities != null)
                {
                    foreach (var ability in card.PlayCardAbilities)
                    {
                        ability.Apply(Player, card, Card);
                    }
                }
            }

            return moves;
        }

        public override BaseMove TransformInGame(Game game)
        {
            var player = game.GetPlayerById(Player.Id);

            var move = new PlayCardMove(player, player.Hand.GetCardById(Card.Id));
            move.HasManaCost = HasManaCost;

            return move;
        }
    }
}