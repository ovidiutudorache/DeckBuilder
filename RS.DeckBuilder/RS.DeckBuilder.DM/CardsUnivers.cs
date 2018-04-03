using RS.DeckBuilder.DM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS.DeckBuilder.Common;
using RS.DeckBuilder.DM.Abilities;

namespace RS.DeckBuilder.DM
{
    public class CardsUnivers
    {
        public List<Card> GetAllCards()
        {
            var list = new List<Card>();

            list.Add(new Card("Wisp", 0, 1, 1));
            list.Add(new Card("Young Priestess", 1, 2, 1)
            {
                EndTurnAbilities = new BaseAbility[] { new RandomFriendlyHealth(1) }.ToList()
            });

            list.Add(new Card("Young Dragonhawk", 1, 1, 1)
            {
                StartTurnAbilities = new BaseAbility[] { new Windfury() }.ToList()
            });

            list.Add(new Card("Worgen Infiltrator", 1, 2, 1)
            {
                StartTurnAbilities = new BaseAbility[] { new Stealth() }.ToList()
            });

            list.Add(new Card("Shieldbearer", 1, 0, 4)
            {
                StartTurnAbilities = new BaseAbility[] { new Taunt() }.ToList()
            });

            list.Add(new Card("Murloc Tidecaller", 1, 1, 2)
            {
                PlayCardAbilities = new BaseAbility[] { new PlayCardGainAttack(Races.Murloc, 1) }.ToList(),
                Race = Races.Murloc
            });

            list.Add(new Card("Leper Gnome", 1, 1, 1)
            {
                DeathAbilities = new BaseAbility[] { new DealDamageToEnemy(2) }.ToList(),
            });

            list.Add(new Card("Hungry Crab", 1, 1, 2)
            {
                BirthAbilities = new BaseAbility[] { new DestroyCardGainDamageHealth(Races.Murloc, 2, 2) }.ToList()
            });

            list.Add(new Card("Flame Imp", 1, 3, 2)
            {
                BirthAbilities = new BaseAbility[] { new DealDamageToPlayer(3) }.ToList()
            });

            list.Add(new Card("Blood Imp", 1, 0, 1)
            {
                EndTurnAbilities = new BaseAbility[] { new GiveAttackHealthToFriend(0, 1) }.ToList(),
                StartTurnAbilities = new BaseAbility[] { new Stealth() }.ToList()
            });

            list.Add(new Card("Argent Squire", 1, 1, 1)
            {
                StartTurnAbilities = new BaseAbility[] { new DivineShield() }.ToList()
            });

            list.Add(new Card("Angry Chicken", 1, 1, 1)
            {
                TakeDamageAbilities = new BaseAbility[] { new Enrage(5) }.ToList()
            });

            list.Add(new Card("Abusive Sergeant", 1, 1, 1)
            {
                BirthAbilities = new BaseAbility[] { new GiveAttackHealthToFriend(2, 0, true) }.ToList()
            });

            list.Add(new Card("Arcane Golem", 3, 4, 4)
            {
                BirthAbilities = new BaseAbility[] { new GiveMana(1, true, true) }.ToList()
            });

            list.Add(new Card("Acolyte of Pain", 3, 1, 3)
            {
                TakeDamageAbilities = new BaseAbility[] { new DrawCard() }.ToList()
            });

            list.Add(new Card("Violet Apprentice", 1, 1, 1));

            list.Add(new Card("Violet Teacher", 4, 3, 5)
            {
                PlayCardAbilities = new BaseAbility[] { new PlaySpellPlayCard("Violet Apprentice") }.ToList()
            });

            list.Add(new Card("Twilight Drake", 4, 4, 1)
            {
                BirthAbilities = new BaseAbility[] { new GainAttackHealthForEveryOtherCard(0, 1, true, false) }.ToList()
            });

            list.Add(new Card("Summoning Portal", 4, 0, 4)
            {
                StartTurnAbilities = new BaseAbility[] { new ReduceCardCost(CardTypes.Minion, 2, 1) }.ToList()
            });

            list.Add(new Card("Spellbreaker", 4, 4, 3)
            {
                BirthAbilities = new BaseAbility[] { new SilenceEnemyCard() }.ToList()
            });

            list.Add(new Card("Silvermoon Guardian", 4, 3, 3));
            list.Add(new Card("Leeroy Jenkins", 5, 6, 2));
            list.Add(new Card("Fen Creeper", 5, 3, 6));
            list.Add(new Card("Abomination", 5, 4, 4));
            list.Add(new Card("Illidan Stormrage", 6, 7, 5));

            return list;
        }

        public Card GetCardByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return GetAllCards().FirstOrDefault(el => el.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public Deck GetRandomDeck(int number)
        {
            return new Deck(GetRandomCards(number));
        }

        public Deck GetStandardDeck(int number)
        {
            return new Deck(GetAllCards().Take(number).ToList());
        }

        public List<Card> GetRandomCards(int number)
        {
            var list = GetAllCards();
            list.Shuffle();

            return list.Take(number).ToList();
        }
    }
}