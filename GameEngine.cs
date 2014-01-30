using System;
using System.Collections.Generic;
using System.Media;

namespace JustBlackJack
{
    public class GameEngine
    {
        Random randomGenerator = null;
        IList<ICard> playerCards = null;
        IList<ICard> dealerCards = null;
        IList<ICard> allCards = null;
        IList<ICard> mixedCards = null;
        IHand playerHand = null;
        IHand dealerHand = null;
        IRenderer renderer;
        IUserInterface userInterface = null;
        int sleepTime = 0;
        decimal betAmount = 0;
        decimal bankRollAmount = 0;
        public bool isNotDoublePressed = true;
        public bool isNotContinuePressed = true;
        public bool isNotExitPressed = true;
        public bool isNotStandPressedPlayer = true;
        public bool isNotStandPressedDealer = true;
        public bool isNotHitPressed = true;
        public bool isGameEnd = false;

        public GameEngine(ConsoleRenderer renderer, KeyBoard userInterface)
        {
            this.randomGenerator = new Random();
            this.playerCards = new List<ICard>();
            this.dealerCards = new List<ICard>();
            this.allCards = this.GenerateAllCards();
            this.mixedCards = this.MixCards(this.allCards);
            this.renderer = renderer;
            this.userInterface = userInterface;
        }

        private IList<ICard> GenerateAllCards()
        {
            IList<ICard> allCards = new List<ICard>();
            int allFacesCount = (int)CardFace.Ace + 1;
            int allSuitsCount = (int)CardSuit.Hearts + 1;

            for (int currentFace = 2; currentFace < allFacesCount; currentFace++)
            {
                for (int currentSuit = 1; currentSuit < allSuitsCount; currentSuit++)
                {
                    ICard currentCard = new Card((CardFace)currentFace, (CardSuit)currentSuit);
                    allCards.Add(currentCard);
                }
            }

            return allCards;
        }

        public IList<ICard> InitialFirstTwoCards()
        {
            IList<ICard> initialCards = new List<ICard>();
            if (this.mixedCards.Count < 3)
            {
                //SoundPlayer shuffleSound = new SoundPlayer("../../shuffling-cards-1.wav");
                //shuffleSound.Play();
                this.allCards = this.GenerateAllCards();
                this.mixedCards = this.MixCards(this.allCards);
            }

            ICard first = this.mixedCards[0];
            ICard second = this.mixedCards[1];
            initialCards.Add(first);
            initialCards.Add(second);
            mixedCards.RemoveAt(0);
            mixedCards.RemoveAt(1);           

            return initialCards;
        }

        public void Run()
        {
            this.sleepTime = 500;
            this.renderer.SetConsole();
            this.playerCards = this.InitialFirstTwoCards();
            this.playerHand = new Hand(this.playerCards);
            this.dealerCards = this.InitialFirstTwoCards();
            this.dealerHand = new Hand(this.dealerCards);
            this.bankRollAmount = 1000;
            this.betAmount = PlaceBet(ref bankRollAmount);
            decimal winAmount = 0;
            int playerCurrentPoints = 0;
            int dealerCurrentPoints = 0;
            Console.Clear();

            while (this.isNotExitPressed)
            {
                playerCurrentPoints = GameCalculator.CalculatePoints(this.playerHand);
                dealerCurrentPoints = GameCalculator.CalculatePoints(this.dealerHand);

                bool playerHaveBlackJack = Checker.BlackJackByInitialCards(this.playerCards.Count, playerCurrentPoints);
                bool dealerHaveBlackJack = Checker.BlackJackByInitialCards(this.dealerCards.Count, dealerCurrentPoints);

                this.renderer.RenderCommands(15);
                this.renderer.RenderHand(playerHand, 20);

                IsDealerStand();

                this.DeciteWinnerByBJ(ref winAmount, ref playerCurrentPoints,
                    ref dealerCurrentPoints, ref playerHaveBlackJack, ref dealerHaveBlackJack);

                IsDealersTurn(ref winAmount, ref playerCurrentPoints,
                    ref dealerCurrentPoints, ref playerHaveBlackJack, ref dealerHaveBlackJack);

                ShowInfo(winAmount, playerCurrentPoints, dealerCurrentPoints);

                DecideWinner(ref winAmount, ref playerCurrentPoints, ref dealerCurrentPoints,
                ref playerHaveBlackJack, ref dealerHaveBlackJack);

                System.Threading.Thread.Sleep(sleepTime);

                this.renderer.ClearField();

                this.userInterface.ProccessInput(); 
            }
        }

        private void IsDealersTurn(ref decimal winAmount, ref int playerCurrentPoints,
            ref int dealerCurrentPoints, ref bool playerHaveBlackJack, ref bool dealerHaveBlackJack)
        {
            bool isDealersTurn = !(isNotDoublePressed && isNotStandPressedPlayer);
            if (isDealersTurn)
            {
                if (dealerCurrentPoints <= 17 && (dealerCurrentPoints < playerCurrentPoints) && (dealerCurrentPoints < 21))
                {
                    System.Threading.Thread.Sleep(this.sleepTime);
                    this.dealerCards.Add(DrawCard());
                    this.dealerHand = new Hand(this.dealerCards);
                    dealerCurrentPoints = GameCalculator.CalculatePoints(this.dealerHand);
                    this.ShowInfo(winAmount, playerCurrentPoints, dealerCurrentPoints);
                }
                if (dealerCurrentPoints <= playerCurrentPoints)
                {
                    if (dealerCurrentPoints > playerCurrentPoints && (dealerCurrentPoints < 21) && !this.isNotDoublePressed)
                    {
                        this.StandPressed();
                        isDealersTurn = false;
                        this.isNotStandPressedDealer = false;
                    }
                    else
                    {
                        this.dealerCards.Add(DrawCard());
                        this.dealerHand = new Hand(this.dealerCards);
                        dealerCurrentPoints = GameCalculator.CalculatePoints(this.dealerHand);
                    }
                }
            }
        }

        private void IsDealerStand()
        {
            if (!this.isNotStandPressedDealer)
            {
                this.renderer.RenderHand(dealerHand, 0);
            }
            else
            {
                this.renderer.RenderCard(dealerHand.Cards[0], 0, 0);
                this.renderer.RenderHiddenCard(15, 0);
            }
        }

        private void DeciteWinnerByBJ(ref decimal winAmount, ref int playerCurrentPoints, ref int dealerCurrentPoints, ref bool playerHaveBlackJack, ref bool dealerHaveBlackJack)
        {
            string BJDecision = Checker.CheckForBlackJack(playerHaveBlackJack, dealerHaveBlackJack,
                renderer as ConsoleRenderer);
            bool isPlayerBJ = BJDecision.Equals("player");
            bool isTwoBJ = BJDecision.Equals("push");
            bool isDealerBJ = BJDecision.Equals("dealer");
            if (isPlayerBJ)
            {
                this.isGameEnd = true;
                winAmount = (betAmount * 2);
                this.bankRollAmount += (winAmount * 2);
                dealerCurrentPoints = GameCalculator.CalculatePoints(dealerHand);
                ShowInfo(winAmount, playerCurrentPoints, dealerCurrentPoints);
                this.renderer.RenderAdditionalCommands(15);
                ResetGame(ref playerCurrentPoints, ref dealerCurrentPoints, ref winAmount,
                    ref playerHaveBlackJack, ref dealerHaveBlackJack);

                ContinueGame();
            }
            else if (isTwoBJ)
            {
                this.isGameEnd = true;
                winAmount = betAmount;
                this.bankRollAmount += winAmount;

                ShowInfo(winAmount, playerCurrentPoints, dealerCurrentPoints); 
                this.renderer.RenderAdditionalCommands(15);
                ResetGame(ref playerCurrentPoints, ref dealerCurrentPoints, ref winAmount,
                    ref playerHaveBlackJack, ref dealerHaveBlackJack);

                ContinueGame();
            }
            else if (isDealerBJ)
            {
                this.isGameEnd = true;
                this.isNotStandPressedDealer = false;
                winAmount = 0;
                dealerCurrentPoints = GameCalculator.CalculatePoints(this.dealerHand);
                ShowInfo(winAmount, playerCurrentPoints, dealerCurrentPoints);
                this.renderer.RenderAdditionalCommands(15);
                ResetGame(ref playerCurrentPoints, ref dealerCurrentPoints, ref winAmount,
                    ref playerHaveBlackJack, ref dealerHaveBlackJack);

                ContinueGame();
            }
        }

        private void DecideWinner(ref decimal winAmount, ref int playerCurrentPoints, ref int dealerCurrentPoints,
            ref bool playerHaveBlackJack, ref bool dealerHaveBlackJack)
        {
            string checkerDecision = Checker.CheckForWinner(playerCurrentPoints,
                dealerCurrentPoints, this.renderer as ConsoleRenderer, this.isNotDoublePressed,
                this.isNotStandPressedPlayer, this.isNotStandPressedDealer);
            bool isPlayerWin = checkerDecision.Equals("player");
            bool isDealerWin = checkerDecision.Equals("dealer");
            bool isPlayerBust = checkerDecision.Equals("playerBust");
            bool isPush = checkerDecision.Equals("push");
            bool isDealerBust = checkerDecision.Equals("dealerBust");

            IsPlayerWin(ref winAmount, ref playerCurrentPoints, ref dealerCurrentPoints,
                ref playerHaveBlackJack, ref dealerHaveBlackJack, isPlayerWin);
            IsDealerWin(winAmount, ref playerCurrentPoints, ref dealerCurrentPoints,
                ref playerHaveBlackJack, ref dealerHaveBlackJack, isDealerWin);
            IsPlayerBust(winAmount, ref playerCurrentPoints, ref dealerCurrentPoints,
                ref playerHaveBlackJack, ref dealerHaveBlackJack, isPlayerBust);
            IsPush(winAmount, ref playerCurrentPoints, ref dealerCurrentPoints,
                ref playerHaveBlackJack, ref dealerHaveBlackJack, isPush);
            IsDealerBust(winAmount, ref playerCurrentPoints, ref dealerCurrentPoints,
                ref playerHaveBlackJack, ref dealerHaveBlackJack, isDealerBust);
        }

        private void IsDealerBust(decimal winAmount, ref int playerCurrentPoints,
            ref int dealerCurrentPoints, ref bool playerHaveBlackJack,
            ref bool dealerHaveBlackJack, bool isDealerBust)
        {
            if (isDealerBust)
            {
                winAmount = this.betAmount * 2;
                this.bankRollAmount += winAmount;
                this.isNotStandPressedDealer = false;
                ShowInfo(winAmount, playerCurrentPoints, dealerCurrentPoints);
                this.renderer.RenderAdditionalCommands(15);
                ResetGame(ref playerCurrentPoints, ref dealerCurrentPoints, ref winAmount,
                    ref playerHaveBlackJack, ref dealerHaveBlackJack);

                ContinueGame();
            }
        }

        private void IsPush(decimal winAmount, ref int playerCurrentPoints,
            ref int dealerCurrentPoints, ref bool playerHaveBlackJack, ref bool dealerHaveBlackJack, bool isPush)
        {
            if (isPush)
            {
                this.isGameEnd = true;
                this.bankRollAmount += betAmount;
                winAmount = betAmount;
                this.isNotStandPressedDealer = false;
                ShowInfo(winAmount, playerCurrentPoints, dealerCurrentPoints);
                this.renderer.RenderAdditionalCommands(15);
                ResetGame(ref playerCurrentPoints, ref dealerCurrentPoints, ref winAmount,
                    ref playerHaveBlackJack, ref dealerHaveBlackJack);

                ContinueGame();
            }
        }

        private void IsPlayerBust(decimal winAmount, ref int playerCurrentPoints,
            ref int dealerCurrentPoints, ref bool playerHaveBlackJack, ref bool dealerHaveBlackJack, bool isPlayerBust)
        {
            if (isPlayerBust)
            {
                this.isGameEnd = true;
                this.isNotStandPressedDealer = false;
                ShowInfo(winAmount, playerCurrentPoints, dealerCurrentPoints);
                this.renderer.RenderAdditionalCommands(15);
                ResetGame(ref playerCurrentPoints, ref dealerCurrentPoints, ref winAmount,
                    ref playerHaveBlackJack, ref dealerHaveBlackJack);
                ContinueGame();
            }
        }

        private void IsDealerWin(decimal winAmount, ref int playerCurrentPoints,
            ref int dealerCurrentPoints, ref bool playerHaveBlackJack, ref bool dealerHaveBlackJack, bool isDealerWin)
        {
            if (isDealerWin)
            {
                this.isGameEnd = true;
                winAmount = 0;
                this.isNotStandPressedDealer = false;
                ShowInfo(winAmount, playerCurrentPoints, dealerCurrentPoints);
                this.renderer.RenderAdditionalCommands(15);
                ResetGame(ref playerCurrentPoints, ref dealerCurrentPoints, ref winAmount,
                    ref playerHaveBlackJack, ref dealerHaveBlackJack);
                this.renderer.RenderAdditionalCommands(15);

                this.ContinueGame();

            }
        }

        private void IsPlayerWin(ref decimal winAmount, ref int playerCurrentPoints,
            ref int dealerCurrentPoints, ref bool playerHaveBlackJack, ref bool dealerHaveBlackJack, bool isPlayerWin)
        {
            if (isPlayerWin)
            {
                this.isGameEnd = true;
                winAmount = this.betAmount * 2;
                this.bankRollAmount += (this.betAmount * 2);
                this.isNotStandPressedDealer = false;
                this.renderer.RenderAdditionalCommands(15);
                winAmount = this.betAmount * 2;
                ShowInfo(winAmount, playerCurrentPoints, dealerCurrentPoints);
                ResetGame(ref playerCurrentPoints, ref dealerCurrentPoints, ref winAmount,
                    ref playerHaveBlackJack, ref dealerHaveBlackJack);

                ContinueGame();
            }
        }

        private void ResetGame(ref int playerCurrentPoints, ref int dealerCurrentPoints, ref decimal winAmount,
            ref bool playerHaveBlackJack, ref bool dealerHaveBlackJack)
        {
            this.isNotDoublePressed = true;
            this.isNotContinuePressed = true;
            this.isNotStandPressedPlayer = true;
            this.isNotStandPressedDealer = true;
            this.isNotHitPressed = true;
            this.isGameEnd = false;
            playerHaveBlackJack = false;
            dealerHaveBlackJack = false;
            playerCurrentPoints = 0;
            dealerCurrentPoints = 0;
            winAmount = 0;
            this.renderer.RenderHand(this.dealerHand, 0);
            this.playerCards = this.InitialFirstTwoCards();
            this.dealerCards = this.InitialFirstTwoCards();
            this.playerHand = new Hand(this.playerCards);
            this.dealerHand = new Hand(this.dealerCards);
        }

        private void ShowInfo(decimal winAmount, int playerCurrentPoints, int dealerCurrentPoints)
        {
            string info = "BANKROLL = " + bankRollAmount + new string(' ', 27) + "BET = " +
                betAmount + new string(' ', 31) + "WIN = " + winAmount;
            this.renderer.RenderInfo(32, info);
            this.renderer.RenderInfo(18, new string(' ', 38) + "player points = " + playerCurrentPoints.ToString());

            if (!this.isNotStandPressedDealer)
            {
                this.renderer.RenderInfo(12, new string(' ', 38) + "dealer points = " + dealerCurrentPoints.ToString()); 
            }
            else
            {
                int firstCardPoints = GameCalculator.CalculatePoints(new Hand(new List<ICard> { this.dealerHand.Cards[0] }));
                this.renderer.RenderInfo(12, new string(' ', 38) + "dealer points = " + firstCardPoints.ToString()); 
            }
        }

        private decimal PlaceBet(ref decimal bankRollAmount)
        {
            Console.WriteLine(string.Empty);
            Console.Write("Bet amount: ");
            decimal betAmount = int.Parse(Console.ReadLine());
            while (betAmount > bankRollAmount)
            {
                Console.Write("Not enought amount!");
                Console.WriteLine(string.Empty);
                Console.Write("Bet amount: ");
                betAmount = int.Parse(Console.ReadLine());
            }
            bankRollAmount -= betAmount;
            return betAmount;
        }

        private IList<ICard> MixCards(IList<ICard> allCards)
        {
            IList<ICard> mixedCards = new List<ICard>();

            while (allCards.Count > 0)
            {
                int randomIndex = this.randomGenerator.Next(0, allCards.Count);
                mixedCards.Add(allCards[randomIndex]);
                allCards.RemoveAt(randomIndex);
            }
            return mixedCards;
        }

        public void HitPressed()
        {
            System.Threading.Thread.Sleep(this.sleepTime);
            this.playerCards.Add(DrawCard());
            this.playerHand = new Hand(this.playerCards);
        }

        public void DoublePressed()
        {
            if (this.isNotDoublePressed)
            {
                if (this.bankRollAmount - this.betAmount >= 0)
                {
                    this.bankRollAmount -= this.betAmount;
                    this.betAmount *= 2;
                    this.HitPressed();
                    this.isNotDoublePressed = false;
                    this.isNotHitPressed = false;
                    this.isNotStandPressedPlayer = false;
                }
            }
        }

        private ICard DrawCard()
        {
            if (this.mixedCards.Count < 1)
            {
                this.allCards = this.GenerateAllCards();
                this.mixedCards = this.MixCards(this.allCards);
            }
            ICard card = this.mixedCards[0];
            this.mixedCards.RemoveAt(0);
            return card;
        }

        public void ContinuePressed()
        {
            if (this.isNotContinuePressed)
            {
                this.renderer.ClearField();
                this.betAmount = PlaceBet(ref this.bankRollAmount);
                this.isNotContinuePressed = false;
            }
        }

        public void StandPressed()
        {
            this.isNotContinuePressed = false;
            this.isNotDoublePressed = false;
            this.isNotHitPressed = false;
            this.isNotStandPressedPlayer = false;
            this.isNotStandPressedDealer = false;
        }

        private void ContinueGame()
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.C)
            {
                this.ContinuePressed();
            }
            else if (key.Key == ConsoleKey.E)
            {
                this.ExitPressed();
            }
        }

        public void ExitPressed()
        {
            this.isNotExitPressed = false;
            this.renderer.ClearField();
            this.renderer.RenderInfo(0, "Thank you for playing");
        }
    }
}
