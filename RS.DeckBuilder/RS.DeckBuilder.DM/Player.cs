using Newtonsoft.Json;
using RS.DeckBuilder.DM.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM
{
    public class Player
    {
        public Guid Id { get; set; }
        public Deck Deck { get; set; }
        public int Health { get; set; }
        public Hand Hand { get; set; }
        public int Mana { get; set; }
        public PlayerBoard Board { get; set; }
        public Player Enemy { get; set; }

        internal int healthPenalty = 0;

        public const int MaxHealth = 30;
        public int Depth { get; private set; }
        public int MaxMana { get; set; }

        public Player(Deck deck, Hand hand = null)
        {
            Id = Guid.NewGuid();
            Deck = deck;
            Health = MaxHealth;
            Hand = hand;
        }

        public void DrawCard()
        {
            if (Deck == null || Deck.Cards == null || Deck.Cards.Count == 0)
            {
                healthPenalty++;
                Health -= healthPenalty;

                return;
            }

            if (Hand == null)
                Hand = new Hand();

            if (Hand.Cards == null)
                Hand.Cards = new List<Card>();

            var card = Deck.Cards[0];
            Deck.Cards.RemoveAt(0);

            Hand.Cards.Add(card);
        }

        public void PlayRound(Game game, int playerIndex, bool isChild = false, int depth = 0, bool isStart = false, List<BaseMove> additionalMoves = null)
        {
            List<BaseMove> moves = GetAvailableMoves();

            if (additionalMoves != null && additionalMoves.Any())
            {
                if (moves == null)
                    moves = new List<BaseMove>();

                moves.AddRange(additionalMoves);
            }

            int bestScore = int.MinValue;
            BaseMove bestMove = null;
            Player player = playerIndex == 0 ? game.Player1 : game.Player2;
            player.Depth = depth;

            if (isStart)
                player.Board.PlayStartTurnAbilities(player);

            if (moves != null && moves.Any())
            {
                var index = 0;
                foreach (var move in moves)
                {
                    index++;

                    Game temporaryGame = game.Clone();
                    BaseMove transformedMove = move.TransformInGame(temporaryGame);

                    if (playerIndex == 0)
                    {
                        List<BaseMove> resultedMoves = temporaryGame.Player1.MakeMove(transformedMove);
                        temporaryGame.Player1.PlayRound(temporaryGame, playerIndex, true, depth + 1, additionalMoves: resultedMoves);
                        int playerScore = temporaryGame.Player1.GetScore();
                        int enemyScore = temporaryGame.Player1.Enemy.GetScore();
                        int score = playerScore - enemyScore;
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = move;
                        }
                    }
                    else
                    {
                        List<BaseMove> resultedMoves = temporaryGame.Player2.MakeMove(transformedMove);
                        temporaryGame.Player2.PlayRound(temporaryGame, playerIndex, true, depth + 1, additionalMoves: resultedMoves);
                        int playerScore = temporaryGame.Player2.GetScore();
                        int enemyScore = temporaryGame.Player2.Enemy.GetScore();
                        int score = playerScore - enemyScore;
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = move;
                        }
                    }
                }
            }

            if (bestMove != null)
            {
                List<BaseMove> resultedMoves = MakeMove(bestMove, !isChild);
                PlayRound(game, playerIndex, isChild, additionalMoves: resultedMoves);
            }

            if (moves == null || !moves.Any())
            {
                player.Board.PlayEndTurnAbilities(player);
                foreach (var card in player.Board.Cards)
                {
                    card.Attack -= card.TemporaryAttack;
                    card.TemporaryAttack = 0;

                    card.ApplyDamage(player, card.TemporaryHealth, player);
                    card.TemporaryHealth = 0;
                }
            }
        }

        public int GetScore()
        {
            int score = 0;
            score += Health;
            score += MaxHealth - Enemy.Health;

            if (Board.Cards != null)
            {
                foreach (var item in Board.Cards)
                    score += item.GetScore(true);
            }

            if (Enemy.Board.Cards != null)
            {
                foreach (var item in Enemy.Board.Cards)
                    score -= item.GetScore(true);
            }

            if (Hand != null && Hand.Cards != null)
            {
                foreach (var item in Hand.Cards)
                    score += item.GetScore(false);
            }

            return score;
        }

        private List<BaseMove> MakeMove(BaseMove move, bool log = false)
        {
            if (move == null)
                return null;

            return move.Play(log);
        }

        private List<BaseMove> GetAvailableMoves()
        {
            var moves = new List<BaseMove>();

            var m1 = GetAvailableCardsToPlayOnBoard();
            if (m1 != null && m1.Any())
                moves.AddRange(m1);

            var m2 = GetAvailableCardsToAttackEnemyCards();
            if (m2 != null && m2.Any())
                moves.AddRange(m2);

            var m3 = GetAvailableCardsToAttackEnemy();
            if (m3 != null && m3.Any())
                moves.AddRange(m3);

            return moves;
        }

        private List<PlayCardMove> GetAvailableCardsToPlayOnBoard()
        {
            if (Hand == null || Hand.Cards == null || !Hand.Cards.Any())
                return null;

            List<PlayCardMove> list = null;

            foreach (var item in Hand.Cards)
            {
                if (!item.CanPlayOnBoard(this, Board))
                    continue;

                if (list == null)
                    list = new List<PlayCardMove>();

                list.Add(new PlayCardMove(this, item));
            }

            return list;
        }

        private List<AttackCardWithCardMove> GetAvailableCardsToAttackEnemyCards()
        {
            if (Board == null || Board.Cards == null || !Board.Cards.Any() || Enemy.Board == null || Enemy.Board.Cards == null || !Enemy.Board.Cards.Any())
                return null;

            List<AttackCardWithCardMove> list = null;

            foreach (var myCard in Board.Cards)
            {
                foreach (var enemyCard in Enemy.Board.Cards)
                {
                    if (!myCard.CanAttackCard(this, myCard, enemyCard))
                        continue;

                    if (list == null)
                        list = new List<AttackCardWithCardMove>();

                    list.Add(new AttackCardWithCardMove(this, myCard, enemyCard));
                }
            }

            return list;
        }

        private List<AttackEnemyWithCardMove> GetAvailableCardsToAttackEnemy()
        {
            if (Board == null || Board.Cards == null || !Board.Cards.Any())
                return null;

            List<AttackEnemyWithCardMove> list = null;

            foreach (var myCard in Board.Cards)
            {
                if (!myCard.CanAttackEnemy(this, myCard, Enemy, Enemy.Board))
                    continue;

                if (list == null)
                    list = new List<AttackEnemyWithCardMove>();

                list.Add(new AttackEnemyWithCardMove(this, myCard));
            }

            return list;
        }

        public Player Clone()
        {
            var p = new Player(Deck.Clone());
            p.Id = Id;
            p.Health = Health;
            p.Hand = Hand.FromDeck(Hand);
            p.Mana = Mana;
            p.Board = PlayerBoard.FromDeck(Board);
            p.Depth = Depth;

            p.healthPenalty = healthPenalty;

            return p;
        }
    }
}