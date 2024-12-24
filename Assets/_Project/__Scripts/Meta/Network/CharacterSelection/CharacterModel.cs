using System;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Classes.Enums;
using Unity.Netcode;

namespace _Project.__Scripts.Meta.Network.CharacterSelection
{
    [Serializable]
    public struct CharacterModel
    {
        public Character character;
        public NetworkObject prefab;
    }
}
