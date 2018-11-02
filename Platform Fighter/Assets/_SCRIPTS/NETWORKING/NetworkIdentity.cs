using UnityEngine;

namespace NETWORKING
{
    public class NetworkIdentity : MonoBehaviour
    {
        private void Start()
        {
            Events.OnMatchJoined(this);
        }
        
        public int Id;
    }
}