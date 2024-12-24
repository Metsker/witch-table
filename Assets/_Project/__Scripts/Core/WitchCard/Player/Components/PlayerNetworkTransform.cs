using DG.Tweening;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace _Project.__Scripts.Core.WitchCard.Entities.Players.Components
{
    public class PlayerNetworkTransform : NetworkBehaviour
    {
        [SerializeField] private CinemachineCamera cmCamera;
        [SerializeField] private Transform head;
        
        private Vector3 _startRotation;

        private void Start()
        {
            _startRotation = transform.rotation.eulerAngles;
        }

        private void FixedUpdate()
        {
            if (IsOwner)
                SetHeadRotationRpc(cmCamera.transform.rotation);
        }

        [Rpc(SendTo.NotOwner)]
        private void SetHeadRotationRpc(Quaternion rotation)
        {
            head.rotation = rotation;
        }

        [Rpc(SendTo.Everyone)]
        public void LookAtRpc(Vector3 position)
        {
            transform.DOLookAt(position, 0.5f);
        }

        [Rpc(SendTo.Everyone)]
        public void ResetRotationRpc()
        {
            transform.DORotate(_startRotation, 0.5f);
        }
    }
}
