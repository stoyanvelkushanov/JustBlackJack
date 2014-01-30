using System;

namespace JustBlackJack
{
    class Program
    {
        static void Main()
        {
            //IIsValidHand checker = new HandChecker();

            IUserInterface userInterface = new KeyBoard();
            ConsoleRenderer renderer = new ConsoleRenderer();
            GameEngine engine = new GameEngine(renderer, userInterface as KeyBoard);

            userInterface.onHitPressed += (sender, eventInfo) =>
            {
                bool canPressHit = (engine.isNotDoublePressed && engine.isNotStandPressedPlayer);
                if (canPressHit)
                {
                    engine.HitPressed();
                }
            };

            userInterface.onDoublePressed += (sender, eventInfo) =>
                {
                    engine.DoublePressed();
                };

            userInterface.onContinuePressed += (sender, eventInfo) =>
            {
                if (engine.isGameEnd)
                {
                    engine.ContinuePressed();
                } 
            };

           userInterface.onExitPressed += (sender, eventInfo) =>
              {
                
                 engine.ExitPressed();
                
              };

            userInterface.onStandPressed += (sender, eventInfo) =>
                {
                    if (engine.isNotStandPressedPlayer)
                    {
                        engine.StandPressed(); 
                    }
                };

            engine.Run();     
        }
    }
}
