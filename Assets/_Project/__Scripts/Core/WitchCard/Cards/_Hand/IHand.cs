using System.Collections.Generic;

namespace _Project.__Scripts.Core.WitchCard.Cards._Hand
{
    public interface IHand
    {
        IReadOnlyList<CardController> Cards { get; }
        void Init(CardModel[] cardModels);
        void InitMock(int mockCount);
        void AllowPick();
    }
}
