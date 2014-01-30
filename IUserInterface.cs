using System;

namespace JustBlackJack
{
    public interface IUserInterface
    {
        event EventHandler onHitPressed;

        event EventHandler onDoublePressed;

        event EventHandler onStandPressed;

        event EventHandler onContinuePressed;

        event EventHandler onExitPressed;

        void ProccessInput();
    }
}
