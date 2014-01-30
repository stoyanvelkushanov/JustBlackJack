using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustBlackJack
{
    public class ConsoleRenderer : IRenderer
    {
        public void RenderHand(IHand hand, int marginTop)
        {
            if (hand.Cards == null)
            {
                throw new ArgumentNullException("Can't render empty hand!");
            }

            IList<ICard> cards = hand.Cards;
            int marginLeft = 0;
            int distanceBetweenCards = 15;

            for (int currentCardIndex = 0; currentCardIndex < cards.Count; currentCardIndex++)
            {
                ICard currentCard = cards[currentCardIndex];
                RenderCard(currentCard, marginLeft, marginTop);
                marginLeft += distanceBetweenCards;
            }
        }

        public void RenderCard(ICard currentCard, int marginLeft, int marginTop)
        {
            string cardAsString = currentCard.ToString();
            string rank = string.Empty;
            char suit = default(char);
            Console.BackgroundColor = ConsoleColor.White;

            if (currentCard.Face.Equals(CardFace.Ten))
            {
                rank = cardAsString.Substring(0, 2);
                suit = cardAsString[2];

                Console.ForegroundColor = SetForeGroundColor(suit);
                Console.SetCursorPosition(marginLeft, marginTop);
                Console.WriteLine("╔═════════╗"); 
                Console.SetCursorPosition(marginLeft, marginTop + 1);
                Console.WriteLine("║         ║");
                Console.SetCursorPosition(marginLeft, marginTop + 2);
                Console.WriteLine("║ {0}      ║", rank);
                Console.SetCursorPosition(marginLeft, marginTop + 3);
                Console.WriteLine("║ {0}       ║", suit);
                Console.SetCursorPosition(marginLeft, marginTop + 4);
                Console.WriteLine("║         ║");
                Console.SetCursorPosition(marginLeft, marginTop + 5);
                Console.WriteLine("║         ║");
                Console.SetCursorPosition(marginLeft, marginTop + 6);
                Console.WriteLine("║      {0}  ║", suit);
                Console.SetCursorPosition(marginLeft, marginTop + 7);
                Console.WriteLine("║      {0} ║", rank);
                Console.SetCursorPosition(marginLeft, marginTop + 8);
                Console.WriteLine("╚═════════╝");
            }
            else
            {
                rank = cardAsString[0].ToString();
                suit = cardAsString[1];

                Console.ForegroundColor = SetForeGroundColor(suit);

                Console.SetCursorPosition(marginLeft, marginTop);
                Console.WriteLine("╔═════════╗"); 
                Console.SetCursorPosition(marginLeft, marginTop + 1);
                Console.WriteLine("║         ║");
                Console.SetCursorPosition(marginLeft, marginTop + 2);
                Console.WriteLine("║ {0}       ║", rank);
                Console.SetCursorPosition(marginLeft, marginTop + 3);
                Console.WriteLine("║ {0}       ║", suit);
                Console.SetCursorPosition(marginLeft, marginTop + 4);
                Console.WriteLine("║         ║");
                Console.SetCursorPosition(marginLeft, marginTop + 5);
                Console.WriteLine("║         ║");
                Console.SetCursorPosition(marginLeft, marginTop + 6);
                Console.WriteLine("║       {0} ║", suit);
                Console.SetCursorPosition(marginLeft, marginTop + 7);
                Console.WriteLine("║       {0} ║", rank);
                Console.SetCursorPosition(marginLeft, marginTop + 8);
                Console.WriteLine("╚═════════╝");
            }
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public void RenderHiddenCard(int marginLeft, int marginTop)
        {
            Console.BackgroundColor = ConsoleColor.White;

            Console.SetCursorPosition(marginLeft, marginTop);
            Console.WriteLine("╔═════════╗");
            Console.SetCursorPosition(marginLeft, marginTop + 1);
            Console.WriteLine("║         ║");
            Console.SetCursorPosition(marginLeft, marginTop + 2);
            Console.WriteLine("║ {0}       ║", '?');
            Console.SetCursorPosition(marginLeft, marginTop + 3);
            Console.WriteLine("║ {0}       ║", '?');
            Console.SetCursorPosition(marginLeft, marginTop + 4);
            Console.WriteLine("║         ║");
            Console.SetCursorPosition(marginLeft, marginTop + 5);
            Console.WriteLine("║         ║");
            Console.SetCursorPosition(marginLeft, marginTop + 6);
            Console.WriteLine("║       {0} ║", '?');
            Console.SetCursorPosition(marginLeft, marginTop + 7);
            Console.WriteLine("║       {0} ║", '?');
            Console.SetCursorPosition(marginLeft, marginTop + 8);
            Console.WriteLine("╚═════════╝");

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        public void ClearField()
        {
            Console.Clear();
        }

        private ConsoleColor SetForeGroundColor(char suit)
        {
            ConsoleColor color = default(ConsoleColor);

            if (suit == '♣')
            {
                color = ConsoleColor.Black;
            }
            else if (suit == '♠')
            {
                color = ConsoleColor.Black;
            }
            else if (suit == '♥')
            {
                color = ConsoleColor.Red;
            }
            else if (suit == '♦')
            {
                color = ConsoleColor.Red;
            }

            return color;
        }

        public void SetConsole()
        {
            Console.CursorVisible = false;
            Console.WindowWidth = 100;
            Console.WindowHeight = 35;
            Console.BufferWidth = 100;
            Console.BufferHeight = 35;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Clear();
        }

        public void RenderCommands(int marginTop)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, marginTop);
            Console.Write("Double = D");
            Console.SetCursorPosition(15, marginTop);
            Console.Write("Hit = H");
            Console.SetCursorPosition(27, marginTop);
            Console.Write("Stand = S");
        }

        public void RenderInfo(int marginTop, string info)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, marginTop);
            Console.WriteLine(info);
        }

        public void RenderWinLooseDraw(int marginTop, string message)
        {
            Console.SetCursorPosition(40, marginTop);
            Console.WriteLine(message);
        }

        public void RenderAdditionalCommands(int marginTop)
        {
            Console.SetCursorPosition(0, marginTop);
            Console.WriteLine(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, marginTop);
            Console.Write("Continue = C");
            Console.SetCursorPosition(15, marginTop);
            Console.Write("Exit = E");
        }

    }
}
