using UnityEngine;
using UnityEngine.Serialization;

namespace CFD.Misc
{
    [RequireComponent(typeof(Camera))]
    public class AspectRatioCameraFitter : MonoBehaviour
    {
        public Vector2 ReferenceResolution;
        public Vector3 ZoomFactor = Vector3.one;

        [HideInInspector] public Vector3 OriginPosition;

        private Vector2 _savedScreenResolution;
        private Vector3 _transformForward;
        private Vector3 _transformRight;
        private Vector3 _transformUp;

        void Start()
        {
            OriginPosition = transform.position;

            _transformForward = transform.forward;
            _transformRight = transform.right;
            _transformUp = transform.up;
        }

        void Update()
        {
            if (ReferenceResolution.y == 0 || ReferenceResolution.x == 0)
                return;

            if (_savedScreenResolution == new Vector2(Screen.width, Screen.height))
                return;

            var refRatio = ReferenceResolution.x / ReferenceResolution.y;
            var ratio = (float)Screen.width / (float)Screen.height;
            _savedScreenResolution = new Vector2(Screen.width, Screen.height);


            transform.position = OriginPosition + _transformForward * (1f - refRatio / ratio) * ZoomFactor.z
                                                + _transformRight * (1f - refRatio / ratio) * ZoomFactor.x
                                                + _transformUp * (1f - refRatio / ratio) * ZoomFactor.y;
        }
    }
}