using UnityEngine;

namespace CFD.Features.CardsShuffle
{
    [CreateAssetMenu(fileName = nameof(CardsShuffleConfig), menuName = "CFD/CardsShuffle/" + nameof(CardsShuffleConfig))]
    public class CardsShuffleConfig : ScriptableObject
    {
        [SerializeField] private CardView _cardPrefab;
        [SerializeField] private int _cardsCount;
        [SerializeField] private Vector3 _cardShiftingOffset;
        
        public CardView CardPrefab => _cardPrefab;
        public int CardsCount => _cardsCount;
        public Vector3 CardShiftingOffset => _cardShiftingOffset;
    }
}