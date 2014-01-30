using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JustBlackJack
{
    public interface IRenderer
    {
        void RenderHand(IHand hand, int marginTop);

        void RenderCard(ICard currentCard, int marginLeft, int marginTop);

        void RenderHiddenCard(int marginLeft, int marginTop);

        void ClearField();

        void SetConsole();

        void RenderCommands(int marginTop);

        void RenderInfo(int marginTop, string info);

        void RenderWinLooseDraw(int marginTop, string message);

        void RenderAdditionalCommands(int marginTop);
    }
}
