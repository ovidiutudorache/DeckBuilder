using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RS.DeckBuilder.DM;
using RS.DeckBuilder.DM.Abilities;

namespace RS.DeckBuilder.UnitTest
{
    [TestClass]
    public class Abilities
    {
        [TestMethod]
        public void RandomFriendlyHealth()
        {
            var h1 = new List<Card>();

            h1.Add(new Card("Wisp", 0, 1, 1));
            h1.Add(new Card("Young Priestess", 1, 2, 1)
            {
                EndTurnAbilities = new BaseAbility[] { new RandomFriendlyHealth(1) }.ToList()
            });

            var p1 = new Player(new Deck(), new Hand()
            {
                Cards = h1
            });

            var p2 = new Player(new Deck());

            var game = new Game(p1, p2);
            game.PlayNextRound();

            var wisp = p1.Board.GetCardByName("Wisp");
            Assert.AreEqual(wisp.Health, wisp.BaseHealth + 1);
        }

        [TestMethod]
        public void PlayCardGainAttack()
        {
            var h1 = new List<Card>();

            h1.Add(new Card("Murloc", 1, 1, 2)
            {
                Race = Races.Murloc
            });

            h1.Add(new Card("Murloc Tidecaller", 1, 1, 2)
            {
                PlayCardAbilities = new BaseAbility[] { new PlayCardGainAttack(Races.Murloc, 1) }.ToList(),
                Race = Races.Murloc
            });

            var p1 = new Player(new Deck(), new Hand()
            {
                Cards = h1
            });

            var p2 = new Player(new Deck());

            var game = new Game(p1, p2);
            game.PlayNextRound(2);

            var murloc = p1.Board.GetCardByName("Murloc Tidecaller");
            Assert.AreEqual(murloc.Attack, murloc.BaseAttack + 1);
        }

        [TestMethod]
        public void Stealth()
        {
            var p1 = new Player(new Deck(), new Hand())
            {
                Board = new PlayerBoard()
            };

            p1.Board.Cards.Add(new Card("Murloc", 1, 1, 2)
            {
                Race = Races.Murloc
            });

            var p2 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            p2.Board.Cards.Add(new Card("Murloc", 1, 1, 2)
            {
            });

            p2.Board.Cards.Add(new Card("Murloc Stealth", 1, 5, 1)
            {
                StartTurnAbilities = new BaseAbility[] { new Stealth() }.ToList()
            });

            var game = new Game(p1, p2);
            game.PlayNextRound();

            var murloc = p2.Board.GetCardByName("Murloc Stealth");
            Assert.IsNotNull(murloc);
        }

        [TestMethod]
        public void Taunt()
        {
            var p1 = new Player(new Deck(), new Hand())
            {
                Board = new PlayerBoard()
            };

            p1.Board.Cards.Add(new Card("Murloc", 1, 2, 2));
            p1.Board.Cards.Add(new Card("Murloc", 1, 2, 2));

            var p2 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            p2.Board.Cards.Add(new Card("Taunt", 1, 1, 5)
            {
                StartTurnAbilities = new BaseAbility[] { new Taunt() }.ToList()
            });

            var game = new Game(p1, p2);
            game.PlayNextRound();

            var taunt = p2.Board.GetCardByName("Taunt");
            Assert.AreEqual(taunt.Health, 1);
            Assert.AreEqual(p2.Health, Player.MaxHealth - 1);
        }

        [TestMethod]
        public void Windfury()
        {
            var p1 = new Player(new Deck(), new Hand())
            {
                Board = new PlayerBoard()
            };

            p1.Board.Cards.Add(new Card("Murloc", 1, 1, 2)
            {
                StartTurnAbilities = new BaseAbility[] { new Windfury() }.ToList()
            });

            var p2 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };


            var game = new Game(p1, p2);
            game.PlayNextRound();

            Assert.AreEqual(p2.Health, Player.MaxHealth - 3);
        }

        [TestMethod]
        public void DealDamageToEnemy()
        {
            var p1 = new Player(new Deck(), new Hand())
            {
                Board = new PlayerBoard()
            };

            p1.Board.Cards.Add(new Card("Murloc", 1, 2, 2)
            {
            });

            var p2 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            p2.Board.Cards.Add(new Card("Murloc", 1, 10, 2)
            {
                DeathAbilities = new BaseAbility[] { new DealDamageToEnemy(5) }.ToList()
            });

            var game = new Game(p1, p2);
            game.PlayNextRound();

            Assert.AreEqual(p1.Health, Player.MaxHealth - 6);
        }

        [TestMethod]
        public void DestroyCardGainDamageHealth()
        {
            var h1 = new Hand();
            h1.Cards.Add(new Card("Hungry Crab", 1, 1, 2)
            {
                BirthAbilities = new BaseAbility[] { new DestroyCardGainDamageHealth(Races.Murloc, 2, 2) }.ToList()
            });

            var p1 = new Player(new Deck(), h1)
            {
                Board = new PlayerBoard()
            };

            var p2 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            p2.Board.Cards.Add(new Card("Murloc", 1, 10, 2)
            {
                Race = Races.Murloc
            });

            var game = new Game(p1, p2);
            game.PlayNextRound(drawCards: false);

            Assert.AreEqual(p1.Board.Cards[0].Attack, 3);
            Assert.AreEqual(p1.Board.Cards[0].Health, 4);
            Assert.AreEqual(p2.Board.Cards.Count, 0);
            Assert.AreEqual(p1.Health, Player.MaxHealth);
        }

        [TestMethod]
        public void DealDamageToPlayer()
        {
            var d1 = new Deck();
            d1.Cards.Add(new Card("Flame Imp", 1, 3, 2)
            {
                BirthAbilities = new BaseAbility[] { new DealDamageToPlayer(3) }.ToList()
            });

            var p1 = new Player(d1)
            {
                Board = new PlayerBoard()
            };

            var p2 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            var game = new Game(p1, p2);
            game.PlayNextRound();

            Assert.AreEqual(p1.Health, Player.MaxHealth - 3);
        }

        [TestMethod]
        public void GiveAttackHealthToFriend()
        {
            var p1 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            p1.Board.Cards.Add(new Card("Blood Imp", 1, 0, 1)
            {
                EndTurnAbilities = new BaseAbility[] { new GiveAttackHealthToFriend(1, 1) }.ToList(),
                StartTurnAbilities = new BaseAbility[] { new Stealth() }.ToList()
            });

            p1.Board.Cards.Add(new Card("test", 1, 0, 10));

            var p2 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            var game = new Game(p1, p2);
            game.PlayNextRound();

            var card = p1.Board.GetCardByName("test");

            Assert.AreEqual(card.Attack, 1);
            Assert.AreEqual(card.Health, 11);
        }

        [TestMethod]
        public void DivineShield()
        {
            var p1 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            p1.Board.Cards.Add(new Card("divineshield", 1, 10, 1)
            {
                StartTurnAbilities = new BaseAbility[] { new DivineShield() }.ToList()
            });

            var p2 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            p2.Board.Cards.Add(new Card("test1", 1, 1, 1));
            p2.Board.Cards.Add(new Card("test2", 1, 1, 1));

            var game = new Game(p1, p2);
            game.PlayNextRound();

            var card = p1.Board.GetCardByName("divineshield");

            Assert.IsNull(card);
            Assert.AreEqual(p2.Health, Player.MaxHealth - 1 - 10);
        }

        [TestMethod]
        public void Enrage()
        {
            var p1 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            p1.Board.Cards.Add(new Card("enrage", 1, 1, 5)
            {
                StartTurnAbilities = new BaseAbility[] { new Enrage(5) }.ToList()
            });

            var p2 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            p2.Board.Cards.Add(new Card("test1", 1, 1, 1));
            p2.Board.Cards.Add(new Card("test2", 1, 1, 1));

            var game = new Game(p1, p2);
            game.PlayNextRound();

            var card = p1.Board.GetCardByName("enrage");

            Assert.AreEqual(card.Attack, 6);
        }

        [TestMethod]
        public void SilenceEnemyCard()
        {
            var p1 = new Player(new Deck(), new Hand())
            {
                Board = new PlayerBoard()
            };

            p1.Hand.Cards.Add(new Card("Spellbreaker", 4, 4, 3)
            {
                BirthAbilities = new BaseAbility[] { new SilenceEnemyCard() }.ToList()
            });

            var p2 = new Player(new Deck())
            {
                Board = new PlayerBoard()
            };

            p2.Board.Cards.Add(new Card("test", 4, 0, 4)
            {
                StartTurnAbilities = new BaseAbility[] { new ReduceCardCost(CardTypes.Minion, 2, 1) }.ToList()
            });

            var game = new Game(p1, p2);
            game.PlayNextRound(startMana: 100, drawCards: false);

            var card = p2.Board.GetCardByName("test");

            Assert.AreEqual(card.HasAbilities, false);
        }
    }
}
