using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CFD.Features.CardsShuffle
{
    public interface ICardsAnimationBehaviour
    {
        event Action<CardView> OnCardAnimationEnd;
        Transform SpawnPoint { get; }
        public Transform StartDeckTransform {get;}
        public Transform EndDeckTransform {get;}
        void SetCardShiftingOffset(Vector3 cardShiftingOffset);
        void SetCardsTransforms(List<CardView> cards);
        UniTask AnimateStartingDrop(CancellationToken token);
        UniTask AnimateShuffle(CancellationToken token);
    }
}