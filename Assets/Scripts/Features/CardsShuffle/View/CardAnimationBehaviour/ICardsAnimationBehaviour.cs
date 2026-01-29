using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CFD.Features.CardsShuffle
{
    public interface ICardsAnimationBehaviour
    {
        Transform SpawnPoint { get; }
        public Transform StartDeckTransform {get;}
        public Transform EndDeckTransform {get;}
        void SetCardShiftingOffset(Vector3 cardShiftingOffset);
        void SetCardsTransforms(List<CardView> cards);
        UniTask AnimateStartingDrop(Action<CardView> onCardAnimationEnd, CancellationToken token);
        UniTask AnimateShuffle(Action<CardView> onCardAnimationEnd, CancellationToken token);
    }
}