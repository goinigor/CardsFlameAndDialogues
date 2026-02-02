using UnityEngine;

namespace CFD.Misc
{
    /// <summary>
    /// The box that is used to show the fake shadow of the cards to improve performance
    /// </summary>
    public class ShadowBox : MonoBehaviour
    {
        [SerializeField] private Transform _box;

        private Vector3 _bottomPosition;
        private float _startHeight;
        private float _startDepth;

        private void Start()
        {
            _bottomPosition = _box.position;
            _startDepth = _box.localScale.z;
        }

        // public void SetHeight(float targetHeight)
        // {
        //     _box.localScale = new Vector3(_box.localScale.x, targetHeight, _box.localScale.z);
        //     
        //     // Position to keep bottom fixed at original position
        //     _box.position = new Vector3(_box.position.x, _bottomPosition.y + targetHeight / 2, _box.position.z);
        // }
        //
        // public void SetDepth(float targetDepth)
        // {
        //     _box.localScale = new Vector3(_box.localScale.x, _box.localScale.y, targetDepth);
        //     
        //     // Position to keep back fixed at original position
        //     _box.position = new Vector3(_box.position.x, _box.position.y, _bottomPosition.z + targetDepth / 2);
        // }

        public void SetDimensions(float targetHeight, float targetDepth)
        {
            _box.localScale = new Vector3(_box.localScale.x, targetHeight, targetDepth + _startDepth);
            
            // Position to keep bottom and back fixed at original position
            _box.position = new Vector3(_box.position.x, _bottomPosition.y + targetHeight / 2, _bottomPosition.z + targetDepth / 2);
        }
    }
}
