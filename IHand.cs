using System;
using System.Collections.Generic;

namespace JustBlackJack
{
    public interface IHand
    {
        IList<ICard> Cards { get; }
        string ToString();
    }
}
