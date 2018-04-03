using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS.DeckBuilder.DM
{
    public class Deck
    {
        public List<Card> Cards { get; set; }

        public Deck(List<Card> cards = null)
        {
            Cards = cards ?? new List<Card>();
        }

        public Card GetCardById(Guid id)
        {
            if (Cards == null)
                return null;

            return Cards.FirstOrDefault(el => el.Id == id);
        }

        public Card GetCardByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return Cards.FirstOrDefault(el => el.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public Deck Clone()
        {
            List<Card> cards = null;
            if (Cards != null)
            {
                cards = new List<Card>();
                foreach (var item in Cards)
                    cards.Add(item.Clone());
            }

            return new Deck(cards);
        }

        public override string ToString()
        {
            if (Cards == null || !Cards.Any())
                return null;

            var builder = new StringBuilder();
            foreach (var item in Cards)
                builder.AppendLine(item.ToString());

            return builder.ToString();
        }
    }
}