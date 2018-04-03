using RS.DeckBuilder.DM.Abilities;
using RS.DeckBuilder.DM.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CardTypes Type { get; set; }
        public int Cost { get; set; }
        public int BaseCost { get; set; }
        public int Attack { get; set; }
        public int BaseAttack { get; set; }
        public int Health { get; set; }
        public int BaseHealth { get; set; }
        public int NoActionsAvailable { get; set; }
        public int MaxActionsAvailable { get; set; }
        public bool HasStealth { get { return StartTurnAbilities != null && StartTurnAbilities.Any(el => el is Abilities.Stealth); } }
        public bool HasTaunt { get { return StartTurnAbilities != null && StartTurnAbilities.Any(el => el is Abilities.Taunt); } }
        public bool HasDivineShield { get { return StartTurnAbilities != null && StartTurnAbilities.Any(el => el is Abilities.DivineShield); } }
        public Races Race { get; set; }
        public int TemporaryAttack { get; set; }
        public int TemporaryHealth { get; set; }

        public List<BaseAbility> StartTurnAbilities { get; set; }
        public List<BaseAbility> EndTurnAbilities { get; set; }
        public List<BaseAbility> PlayCardAbilities { get; set; }
        public List<BaseAbility> DeathAbilities { get; set; }
        public List<BaseAbility> BirthAbilities { get; set; }
        public List<BaseAbility> TakeDamageAbilities { get; set; }

        public bool HasAbilities { get {
                return
                    (StartTurnAbilities != null && StartTurnAbilities.Any())
                    || (EndTurnAbilities != null && EndTurnAbilities.Any())
                    || (PlayCardAbilities != null && PlayCardAbilities.Any())
                    || (DeathAbilities != null && DeathAbilities.Any())
                    || (BirthAbilities != null && BirthAbilities.Any())
                    || (TakeDamageAbilities != null && TakeDamageAbilities.Any())
                    ;
                    } }

        public Card(string name, int baseCost, int baseAttack = 0, int baseHealth = 0, CardTypes type = CardTypes.Minion)
        {
            Id = Guid.NewGuid();
            Name = name;
            Cost = baseCost;
            BaseCost = baseCost;
            Attack = baseAttack;
            BaseAttack = baseAttack;
            Health = baseHealth;
            BaseHealth = baseHealth;
            Type = type;
            MaxActionsAvailable = 1;
            Race = Races.None;
        }

        public int GetCost(Player player)
        {
            var min = Cost;

            foreach (var card in player.Board.Cards)
            {
                if (card.StartTurnAbilities == null || !card.StartTurnAbilities.Any())
                    continue;

                foreach (var ability in card.StartTurnAbilities.OfType<ReduceCardCost>())
                {
                    if (ability.CardType != Type)
                        continue;

                    var cost = Math.Max(BaseCost - ability.Reduction, ability.MinimumCost);
                    if (cost < min)
                        min = cost;
                }
            }

            return min;
        }

        public void Silence()
        {
            StartTurnAbilities = null;
            EndTurnAbilities = null;
            PlayCardAbilities = null;
            BirthAbilities = null;
            DeathAbilities = null;
            TakeDamageAbilities = null;

            if (Health > BaseHealth)
                Health = BaseHealth;

            if (Attack > BaseAttack)
                Attack = BaseAttack;

            if (Cost < BaseCost)
                Cost = BaseCost;
        }

        public void ApplyDamage(Player attacker, int damage, Player cardOwner = null)
        {
            Health -= damage;

            if (Health <= 0)
                Destroy(attacker, cardOwner);

            if (TakeDamageAbilities != null)
                foreach (var ability in TakeDamageAbilities)
                    ability.Apply(cardOwner, this, this);
        }

        public void Destroy(Player attacker, Player cardOwner)
        {
            cardOwner.Board.Cards.Remove(this);

            if (DeathAbilities != null && DeathAbilities.Any())
            {
                foreach (var ability in DeathAbilities)
                    ability.Apply(attacker, this, this);
            }
        }

        public bool CanPlayOnBoard(Player player, PlayerBoard board)
        {
            return player.Mana >= GetCost(player) && (board.Cards == null || board.Cards.Count < board.MaxSize);
        }

        public bool CanAttackCard(Player player, Card card, Card enemyCard)
        {
            if (!enemyCard.HasTaunt && player.Enemy.Board.Cards.Any(el => el.HasTaunt))
                return false;

            return card.Attack > 0 && card.NoActionsAvailable > 0 && !enemyCard.HasStealth;
        }

        public bool CanAttackEnemy(Player player, Card card, Player enemy, PlayerBoard enemyBoard)
        {
            if (player.Enemy.Board.Cards.Any(el => el.HasTaunt))
                return false;

            return card.Attack > 0 && card.NoActionsAvailable > 0;
        }

        public int GetScore(bool isOnDeck)
        {
            var score = 1 + (Attack + (isOnDeck ? Attack : 0)) * Health * (NoActionsAvailable + 1) - (isOnDeck ? 0 : BaseCost);

            //TODO - de adaugat score pe ability
            if (HasTaunt && isOnDeck)
                score += Attack * Health * (NoActionsAvailable + 1) * 2;

            return score;
        }

        public Card Clone()
        {
            var c = new Card(Name, BaseCost, Attack, Health, Type);
            c.BaseHealth = BaseHealth;
            c.BaseAttack = BaseAttack;
            c.Id = Id;
            c.NoActionsAvailable = NoActionsAvailable;
            c.MaxActionsAvailable = MaxActionsAvailable;

            if (StartTurnAbilities != null)
            {
                c.StartTurnAbilities = new List<BaseAbility>();
                foreach (var item in StartTurnAbilities)
                    c.StartTurnAbilities.Add(item.Clone());
            }

            if (EndTurnAbilities != null)
            {
                c.EndTurnAbilities = new List<BaseAbility>();
                foreach (var item in EndTurnAbilities)
                    c.EndTurnAbilities.Add(item.Clone());
            }

            if (PlayCardAbilities != null)
            {
                c.PlayCardAbilities = new List<BaseAbility>();
                foreach (var item in PlayCardAbilities)
                    c.PlayCardAbilities.Add(item.Clone());
            }

            if (BirthAbilities != null)
            {
                c.BirthAbilities = new List<BaseAbility>();
                foreach (var item in BirthAbilities)
                    c.BirthAbilities.Add(item.Clone());
            }

            if (DeathAbilities != null)
            {
                c.DeathAbilities = new List<BaseAbility>();
                foreach (var item in DeathAbilities)
                    c.DeathAbilities.Add(item.Clone());
            }

            if (TakeDamageAbilities != null)
            {
                c.TakeDamageAbilities = new List<BaseAbility>();
                foreach (var item in TakeDamageAbilities)
                    c.TakeDamageAbilities.Add(item.Clone());
            }

            return c;
        }

        public override string ToString()
        {
            string description = string.Format("{0} {1} {2} {3}", Name, Attack, Health, Cost);

            if (HasStealth)
                description += " STEALTH";

            if (HasTaunt)
                description += " TAUNT";

            if (HasDivineShield)
                description += " DIVINE SHIELD";

            return description;
        }
    }
}