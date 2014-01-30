using System;
using System.Diagnostics;

namespace JustBlackJack
{
    public class HandChecker : IIsValidHand
    {
        public bool IsValidHand(IHand hand)
        {
            Debug.Assert(hand != null, "The hand is not initialized!");

            int maxPossibleSameFaces = 4;
            int cardCounter;
            int maxPossibleSuitForGivenCard = 1;
            int suitCounter;
            bool isValidHand = true;
            foreach (var card in hand.Cards)
            {
                cardCounter = 0;
                suitCounter = 0;
                ICard currentCard = card;
                for (int i = 0; i < hand.Cards.Count; i++)
                {
                    if (currentCard.Face == hand.Cards[i].Face)
                    {
                        cardCounter++;
                        if (currentCard.Suit == hand.Cards[i].Suit)
                        {
                            suitCounter++;
                        }
                    }
                }
                if (cardCounter > maxPossibleSameFaces || suitCounter > maxPossibleSuitForGivenCard)
                {
                    isValidHand = false;
                }
            }

            Debug.Assert(isValidHand, "The cards in hand are not valid!");
            return isValidHand;
        }
    }
}
