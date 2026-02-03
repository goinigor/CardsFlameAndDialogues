using UnityEngine;
using UnityEngine.Serialization;

namespace CFD.Misc
{
    /// <summary>
    /// Adjusts camera position to fit the screen aspect ratio
    /// </summary>
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

            // Calculate base position adjustment
            var baseAdjustment = (1f - refRatio / ratio);
            
            // Additional back movement for wider aspect ratios than 16:9
            var wideRatioAdjustment = 0f;
            if (ratio > 16f / 9f)
            {
                // Move camera back progressively as aspect ratio gets wider
                wideRatioAdjustment = (ratio - 16f / 9f) * 2f;
            }

            transform.position = OriginPosition + _transformForward * (baseAdjustment * ZoomFactor.z - wideRatioAdjustment)
                                                + _transformRight * baseAdjustment * ZoomFactor.x
                                                + _transformUp * baseAdjustment * ZoomFactor.y;
        }
    }
}
