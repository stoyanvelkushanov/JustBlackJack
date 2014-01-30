using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustBlackJack
{
    public static class GameCalculator
    {
        public static int CalculatePoints(IHand hand)
        {
            int points = 0;
            for (int i = 0; i < hand.Cards.Count; i++)
            {
                bool isHighCards = hand.Cards[i].Face.Equals(CardFace.Jack) ||
                    hand.Cards[i].Face.Equals(CardFace.Queen) || hand.Cards[i].Face.Equals(CardFace.King);
                if (isHighCards)
                {
                    points += 10;
                }
                else if (hand.Cards[i].Face.Equals(CardFace.Ace))
                {
                    if (points <= 10)
                    {
                        points += 11;
                    }
                    else
                    {
                        points += 1;
                    }
                }
                else
                {
                    points += (int)hand.Cards[i].Face;
                }
            }

            int pointsWithoutAcesPoints = CalcPointsWithoutAces(hand);
            int acesNumber = CalculateAcesNumber(hand);
            if (pointsWithoutAcesPoints >= 21 && (acesNumber > 0))
            {
                //not substract value of aces
            }
            else
            {
                if (points > 21 && (acesNumber > 0))
                {
                    points -= (acesNumber * 10);
                }

            }
            return points;
        }

        private static int CalcPointsWithoutAces(IHand hand)
        {
            int points = 0;
            for (int i = 0; i < hand.Cards.Count; i++)
            {
                bool isHighCards = hand.Cards[i].Face.Equals(CardFace.Jack) ||
                    hand.Cards[i].Face.Equals(CardFace.Queen) || hand.Cards[i].Face.Equals(CardFace.King);
                if (isHighCards)
                {
                    points += 10;
                }
                else if (hand.Cards[i].Face != CardFace.Ace)
                {
                    points += (int)hand.Cards[i].Face;
                }
            }
            return points;
        }

        private static int CalculateAcesNumber(IHand hand)
        {
            int acesCount = 0;
            for (int i = 0; i < hand.Cards.Count; i++)
            {
                if (hand.Cards[i].Face.Equals(CardFace.Ace))
                {
                    acesCount++;
                }
            }
            return acesCount;
        }
    }
}
