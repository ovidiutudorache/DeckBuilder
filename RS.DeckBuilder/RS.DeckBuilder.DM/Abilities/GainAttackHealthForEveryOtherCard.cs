using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;

namespace RS.DeckBuilder.DM.Abilities
{
    public class GainAttackHealthForEveryOtherCard : BaseAbility
    {
        public int Attack { get; set; }
        public int Health { get; set; }
        public bool InHand { get; set; }
        public bool OnBoard { get; set; }

        public GainAttackHealthForEveryOtherCard(int attack, int health, bool inHand = false, bool onBoard = false)
        {
            Attack = attack;
            Health = health;
            InHand = inHand;
            OnBoard = onBoard;
        }

        public override AbilityResult Apply(Player player, Card card, Card cardPlayed)
        {
            int attack = 0;
            int health = 0;

            if (InHand)
            {
                attack += Attack * player.Hand.Cards.Count;
                health = Health * player.Hand.Cards.Count;
            }

            if (OnBoard)
            {
                attack += Attack * player.Board.Cards.Count;
                health = Health * player.Board.Cards.Count;
            }

            card.Attack += attack;
            card.Health += health;

            return null;
        }

        public override BaseAbility Clone()
        {
            return new GainAttackHealthForEveryOtherCard(Attack, Health, InHand, OnBoard);
        }
    }
}