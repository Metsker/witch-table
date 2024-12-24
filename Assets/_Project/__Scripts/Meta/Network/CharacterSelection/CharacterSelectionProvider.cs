using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.__Scripts.Meta.Network.CharacterSelection
{
    public class CharacterSelectionProvider : MonoBehaviour
    {
        [SerializeField] private CharacterSelectorController[] characterSelectorControllers;

        public Dictionary<ulong, CharacterModel> GetSelectionForPlayers() =>
            characterSelectorControllers
                .Where(selector => selector.owner.Value != CharacterSelectorController.NotSelected)
                .ToDictionary(selector => selector.owner.Value, selector => selector.Model);
    }
}
