using Unity.Cinemachine;
using UnityEngine;

namespace _Project.__Scripts.Core.DicePocker.Player
{
    public class VCamMediator : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera panCinemachineCamera;
        [SerializeField] private CinemachinePanTilt panTilt;
        [SerializeField] private CinemachineInputAxisController axisController;
        [Space]
        [SerializeField] private CinemachineCamera composerCinemachineCamera;

        public void SelectMovableCamera(bool copyFromComposer)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

            if (copyFromComposer)
            {
                Vector3 composerRotation = composerCinemachineCamera.transform.localRotation.eulerAngles;
                
                if(composerRotation.x > 180) 
                    composerRotation.x -= 360f;
                if (composerRotation.y > 180) 
                    composerRotation.y -= 360f;
                
                panTilt.TiltAxis.Value = composerRotation.x;
                panTilt.PanAxis.Value = composerRotation.y;
            }
            
            composerCinemachineCamera.Priority = 0;
            panCinemachineCamera.Priority = 10;
        }

        public void SelectLookAtCamera(Transform target, bool isSnapshot)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            
            if (isSnapshot)
            {
                (Vector3 position, Quaternion rotation) targetData = (target.position, target.rotation);
                
                target = new GameObject().transform;
                target.SetPositionAndRotation(targetData.position, targetData.rotation);
            }
            composerCinemachineCamera.LookAt = target;
            
            composerCinemachineCamera.Priority = 10;
            panCinemachineCamera.Priority = 0;
        }
    }
}
