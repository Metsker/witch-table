using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Project.__Scripts.Core.WitchCard.Entities
{
    public interface IDealer
    {
        void Deal(IReadOnlyList<ulong> clientsCompleted);
        void DiscardPlayers();
    }
}
