using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CFD.Features.CardsShuffle
{
    public class CardsShuffleSystem : IDisposable
    {
        private readonly CardsShuffleConfig _config;
        private readonly ICardsAnimationBehaviour _cardsAnimationBehaviour;
        private readonly DeckCountView _startDeckCounterView;
        private readonly DeckCountView _endDeckCounterView;
        private readonly GameObject _endingText;
        private readonly CardPool _cardsPool;

        private CancellationTokenSource _cancellationTokenSource;

        public CardsShuffleSystem(CardsShuffleConfig config,
            ICardsAnimationBehaviour cardsAnimationBehaviour,
            DeckCountView startDeckCounterView,
            DeckCountView endDeckCounterView,
            GameObject endingText, 
            CardPool cardsPool
        )
        {
            _config = config;
            _cardsAnimationBehaviour = cardsAnimationBehaviour;
            _startDeckCounterView = startDeckCounterView;
            _endDeckCounterView = endDeckCounterView;
            _endingText = endingText;
            _cardsPool = cardsPool;
        }

        /// <summary>
        /// Creates cards and starts the animation
        /// </summary>
        public void Initialize()
        {
            DisposeCTS();
            _cancellationTokenSource = new CancellationTokenSource();

            _endingText.SetActive(false);
            
            var cards = new List<CardView>();
            
            var count = _config.CardsCount;
            
            cards.Capacity = count;
            
            for (int i = 0; i < count; i++)
            {
                var cardView = _cardsPool.Get();
                
                cardView.transform.SetParent(_cardsAnimationBehaviour.SpawnPoint);
                cardView.transform.localPosition = Vector3.zero;
                cards.Add(cardView);
            }

            _cardsAnimationBehaviour.SetCardShiftingOffset(_config.CardShiftingOffset);
            _cardsAnimationBehaviour.SetCardsTransforms(cards);
            
            AnimateCards(_cancellationTokenSource.Token);
        }

        private async void AnimateCards(CancellationToken token)
        {
            await _cardsAnimationBehaviour.AnimateStartingDrop(OnStartDropCardAnimationEnded, token);
            
            if (token.IsCancellationRequested)
                return;

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token).SuppressCancellationThrow();
            
            if (token.IsCancellationRequested)
                return;
            
            await _cardsAnimationBehaviour.AnimateShuffle(OnShuffleCardAnimationEnded, token);
        }

        private void OnStartDropCardAnimationEnded(CardView card)
        {
            card.transform.SetParent(_cardsAnimationBehaviour.StartDeckTransform, true);
            _startDeckCounterView.SetText(_cardsAnimationBehaviour.StartDeckTransform.childCount.ToString());
        }
        
        private void OnShuffleCardAnimationEnded(CardView card)
        {
            card.transform.SetParent(_cardsAnimationBehaviour.EndDeckTransform, true);
            var childCount = _cardsAnimationBehaviour.StartDeckTransform.childCount;
            var endDeckCount = _cardsAnimationBehaviour.EndDeckTransform.childCount;
            _startDeckCounterView.SetText(childCount.ToString());
            _endDeckCounterView.SetText(endDeckCount.ToString());

            if (endDeckCount >= _config.CardsCount)
            {
                _endingText.SetActive(true);
            }
        }

        private void DisposeCTS()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void Dispose()
        {
            DisposeCTS();
        }
    }
}