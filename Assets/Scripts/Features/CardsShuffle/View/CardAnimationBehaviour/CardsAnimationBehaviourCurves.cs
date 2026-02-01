using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CFD.Features.CardsShuffle
{
    public class CardsAnimationBehaviourCurves : MonoBehaviour, ICardsAnimationBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _cardsStartPosition;
        [SerializeField] private Transform _cardsEndPosition;
        
        [Header("Animation Curves")]
        [Tooltip("Controls the easing of card movement (0 to 1). Use this for EaseIn, EaseOut, etc.")]
        [SerializeField] private AnimationCurve _speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [Tooltip("Controls the height offset during movement (0 to 1). Peak determines maximum height.")]
        [SerializeField] private AnimationCurve _heightToDistanceCurve = AnimationCurve.EaseInOut(0, 0, 1, 0);
        
        [Header("Timing")]
        [Tooltip("Delay between each card drop at the start")]
        [SerializeField] private float _dropDelay = 0.05f;
        [Tooltip("Duration of the drop animation in seconds")]
        [SerializeField] private float _dropDuration = 0.5f;
        [Tooltip("Delay between each card shuffle")]
        [SerializeField] private float _shuffleDelay = 1f;
        [Tooltip("Duration of the shuffle animation in seconds")]
        [SerializeField] private float _shuffleDuration = 1f;
        
        public Transform SpawnPoint => _spawnPoint;
        public Transform StartDeckTransform => _cardsStartPosition;
        public Transform EndDeckTransform => _cardsEndPosition;

        private List<CardView> _cards;

        /// <summary>
        /// This offset will be applied to each card
        /// </summary>
        private Vector3 _cardShiftingOffset;

        public void SetCardShiftingOffset(Vector3 cardShiftingOffset)
        {
            _cardShiftingOffset = cardShiftingOffset;
        }

        public void SetCardsTransforms(List<CardView> cards)
        {
            _cards = cards;
        }

        /// <summary>
        /// Animate cards to drop from the _spawnPoint to the _cardsStartPosition
        /// </summary>
        /// <param name="onCardAnimationEnd">Callback invoked when each card animation completes</param>
        /// <param name="token"></param>
        public async UniTask AnimateStartingDrop(Action<CardView> onCardAnimationEnd, CancellationToken token)
        {
            if (_cards == null || _cards.Count == 0)
                return;

            var dropTasks = new List<UniTask>();

            for (int i = 0; i < _cards.Count; i++)
            {
                var card = _cards[i];
                dropTasks.Add(AnimateCardDrop(card, i * _dropDelay, i * _cardShiftingOffset, onCardAnimationEnd, token));
            }

            await UniTask.WhenAll(dropTasks);
        }

        private async UniTask AnimateCardDrop(CardView card, float initialDelay, Vector3 cardShiftingOffset, Action<CardView> onCardAnimationEnd, CancellationToken token)
        {
            if (initialDelay > 0)
                await UniTask.Delay(TimeSpan.FromSeconds(initialDelay), cancellationToken: token).SuppressCancellationThrow();

            if (token.IsCancellationRequested)
                return;
            
            var endPosition = _cardsStartPosition.position + cardShiftingOffset;
            await AnimateCard(card, _spawnPoint.position, endPosition, _dropDuration, onCardAnimationEnd, token);
        }

        /// <summary>
        /// Animate cards to shuffle from the _cardsStartPosition to the _cardsEndPosition
        /// </summary>
        /// <param name="onCardAnimationEnd">Callback invoked when each card animation completes</param>
        /// <param name="token"></param>
        public async UniTask AnimateShuffle(Action<CardView> onCardAnimationEnd, CancellationToken token)
        {
            if (_cards == null || _cards.Count == 0)
                return;

            var shuffleTasks = new List<UniTask>();

            for (int i = 0; i < _cards.Count; i++)
            {
                var backwardIndex = _cards.Count - 1 - i;
                var card = _cards[backwardIndex];
                shuffleTasks.Add(AnimateCardShuffle(card, i * _shuffleDelay, i * _cardShiftingOffset, onCardAnimationEnd, token));
            }

            await UniTask.WhenAll(shuffleTasks);
        }

        private async UniTask AnimateCardShuffle(
            CardView card,
            float initialDelay,
            Vector3 cardShiftingOffset,
            Action<CardView> onCardAnimationEnd,
            CancellationToken token
            )
        {
            if (initialDelay > 0)
                await UniTask.Delay(TimeSpan.FromSeconds(initialDelay), cancellationToken: token).SuppressCancellationThrow();

            if (token.IsCancellationRequested)
                return;
            
            var endPosition = _cardsEndPosition.position + cardShiftingOffset;
            await AnimateCard(card, card.transform.position, endPosition, _shuffleDuration, onCardAnimationEnd, token);
        }

        /// <summary>
        /// This method moves a card from start to end using the curves and delays
        /// </summary>
        private async UniTask AnimateCard(
            CardView card,
            Vector3 startPos,
            Vector3 endPos,
            float duration,
            Action<CardView> onCardAnimationEnd,
            CancellationToken token)
        {
            var elapsedTime = 0f;
            var cardTransform = card.transform;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var progress = Mathf.Clamp01(elapsedTime / duration);
                var speedValue = _speedCurve.Evaluate(progress);
                
                var position = Vector3.Lerp(startPos, endPos, speedValue);
                var heightOffset = _heightToDistanceCurve.Evaluate(progress);
                var finalPosition = position + Vector3.up * heightOffset;
                
                cardTransform.position = finalPosition;
                
                await UniTask.Yield(cancellationToken: token).SuppressCancellationThrow();
                
                if (token.IsCancellationRequested)
                    return;
            }
            
            //hard set the position in the end
            cardTransform.position = endPos;
            onCardAnimationEnd?.Invoke(card);
        }
    }
}