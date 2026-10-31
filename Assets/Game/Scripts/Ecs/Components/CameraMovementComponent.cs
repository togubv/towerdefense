using Unity.Cinemachine;
using UnityEngine;

namespace TD
{
    public struct CameraMovementComponent
    {
        public Transform trans;
        public Vector2 input;
        public float speed;
        public float rotateInput;
        public float rotateSpeed;
        public CinemachineOrbitalFollow orbitalFollow;
    }
}