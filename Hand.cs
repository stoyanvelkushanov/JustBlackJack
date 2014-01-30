using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustBlackJack
{
    public class Hand : IHand
    {
        public IList<ICard> Cards { get; private set; }

        public Hand(IList<ICard> cards)
        {
            this.Cards = new List<ICard>();
            foreach (var card in cards)
            {
                this.Cards.Add(card);
            }
        }

        public override string ToString()
        {
            string result = string.Empty;

            foreach (var card in this.Cards)
            {
                result += card.ToString();
            }

            return result;
        }
    }
}
