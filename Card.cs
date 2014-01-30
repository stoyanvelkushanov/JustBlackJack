using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustBlackJack
{
    public class Card : ICard
    {
        public CardFace Face { get; private set; }
        public CardSuit Suit { get; private set; }

        public Card(CardFace face, CardSuit suit)
        {
            this.Face = face;
            this.Suit = suit;
        }

        public override string ToString()
        {
            string result = string.Empty;

            if ((int)this.Face <= 10)
            {
                int cardNumber = (int)this.Face;
                result = cardNumber.ToString();
            }
            else
            {
                char cardFace = this.Face.ToString()[0];
                result = cardFace.ToString();
            }

            switch (this.Suit)
            {
                case CardSuit.Clubs:
                    result += '♣';
                    break;
                case CardSuit.Diamonds:
                    result += '♦';
                    break;
                case CardSuit.Hearts:
                    result += '♥';
                    break;
                case CardSuit.Spades:
                    result += '♠';
                    break;
                default:
                    throw new InvalidOperationException("Invalid card suit!");
            }

            return result;
        }
    }
}
