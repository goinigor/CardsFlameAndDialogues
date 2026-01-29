using UnityEngine;

namespace CFD.Features.Dialogues
{
    [CreateAssetMenu(fileName = nameof(DialoguesConfig), menuName = "CFD/Dialogues/" + nameof(DialoguesConfig))]
    public class DialoguesConfig : ScriptableObject
    {
        [SerializeField] private string _dataUrl;
        [SerializeField] private Sprite _fallbackUserIcon;

        public string DataUrl => _dataUrl;
        public Sprite FallbackUserIcon => _fallbackUserIcon;
    }
}