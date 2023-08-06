using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TestTask.CameraSettings
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] float endPointX; //At this point, the camera stops moving
        CinemachineVirtualCamera vCam;
        Camera mainCamera;

        private void Start()
        {
            vCam = GetComponent<CinemachineVirtualCamera>();
            mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (vCam.Follow == null) { return; }
            Vector2 cameraTopRight=mainCamera.ViewportToWorldPoint(new Vector2(1,1));
            if (cameraTopRight.x >= endPointX)
            {
               vCam.Follow = null;

            }
        }


        public static bool IsVisibleToCamera(Transform transform) //Check if an object is in the camera's field of view
        {
            Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
            return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
        }
    }
}
