using UnityEngine;
using UnityEngine.EventSystems;

namespace CFD.Core
{
    public class EventSystemManager : MonoBehaviour
    {
        private void Awake()
        {
            // Check if another EventSystem already exists
            if (FindObjectsOfType<EventSystem>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }
        
            DontDestroyOnLoad(gameObject);
        }
    }
}