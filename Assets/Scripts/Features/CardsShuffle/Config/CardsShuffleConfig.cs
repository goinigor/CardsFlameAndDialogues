using UnityEngine;

namespace CFD.Features.CardsShuffle
{
    [CreateAssetMenu(fileName = nameof(CardsShuffleConfig), menuName = "CFD/CardsShuffle/" + nameof(CardsShuffleConfig))]
    public class CardsShuffleConfig : ScriptableObject
    {
        [SerializeField] private CardView _cardPrefab;
        [SerializeField] private int _cardsCount;
        [SerializeField] private Vector3 _cardShiftingOffset;
        [SerializeField] private Material[] _cardMaterials;
        [SerializeField] private Material _fallbackMaterial;
        
        public CardView CardPrefab => _cardPrefab;
        public int CardsCount => _cardsCount;
        public Vector3 CardShiftingOffset => _cardShiftingOffset;
        public Material[] CardMaterials => _cardMaterials;
        public Material FallbackMaterial => _fallbackMaterial;
    }
}