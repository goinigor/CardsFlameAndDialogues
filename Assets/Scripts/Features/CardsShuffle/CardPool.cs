using UnityEngine;
using CFD.Core.Pooling;

namespace CFD.Features.CardsShuffle
{
    public class CardPool : ObjectPool<CardView>
    {
        private readonly Material[] _materials;
        private readonly Material _fallbackMaterial;

        public CardPool(
            Material[] materials,
            Material fallbackMaterial,
            CardView cardPrefab,
            Transform poolParent = null,
            int initialSize = 20,
            bool autoExpand = true
        )
        : base(cardPrefab, poolParent, initialSize, autoExpand)
        {
            _materials = materials;
            _fallbackMaterial = fallbackMaterial;

            if (poolParent == null)
                parent = new GameObject("CardPool").transform;
        }

        protected override CardView CreateNewObject()
        {
            var cardView = base.CreateNewObject();

            var meshRenderer = cardView.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer == null) 
                return cardView;
            
            Material material;
            if (_materials.Length > 0)
            {
                var random = Random.Range(0, _materials.Length);
                material = _materials[random];
            }
            else
            {
                if (_fallbackMaterial == null)
                {
                    Debug.LogError($"[{nameof(CardPool)}] missing materials and fallback material as well");
                    return cardView;
                }

                Debug.LogWarning($"[{nameof(CardPool)}] missing materials, used fallback material", cardView.gameObject);
                material = _fallbackMaterial;
            }

            if (material != null)
            {
                meshRenderer.sharedMaterial = material;
            }

            return cardView;
        }
    }
}
