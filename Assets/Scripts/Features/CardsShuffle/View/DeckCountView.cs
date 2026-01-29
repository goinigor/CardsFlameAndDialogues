using CFD.Core.UI;
using TMPro;
using UnityEngine;

namespace CFD.Features.CardsShuffle
{
    public class DeckCountView : View
    {
        [SerializeField] private TMP_Text _text;
        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}