using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustBlackJack
{
    public class KeyBoard : IUserInterface
    {
        public event EventHandler onHitPressed;

        public event EventHandler onDoublePressed;

        public event EventHandler onStandPressed;

        public event EventHandler onContinuePressed;

        public event EventHandler onExitPressed;

        public void ProccessInput()
        {
            if (Console.KeyAvailable)
            {
                var keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.H)
                {
                    if (this.onHitPressed != null)
                    {
                        this.onHitPressed(this, new EventArgs());
                    }
                }
                else if (keyInfo.Key == ConsoleKey.D)
                {
                    if (this.onDoublePressed != null)
                    {
                        this.onDoublePressed(this, new EventArgs());
                    }
                }
                else if (keyInfo.Key == ConsoleKey.S)
                {
                    if (this.onStandPressed != null)
                    {
                        this.onStandPressed(this, new EventArgs());
                    }
                }
                else if (keyInfo.Key == ConsoleKey.C)
                {
                    if (this.onContinuePressed != null)
                    {
                        this.onContinuePressed(this, new EventArgs());
                    }
                }
                else if (keyInfo.Key == ConsoleKey.E)
                {
                    if (this.onExitPressed != null)
                    {
                        this.onExitPressed(this, new EventArgs());
                    }
                }
            }
        }
    }
}
