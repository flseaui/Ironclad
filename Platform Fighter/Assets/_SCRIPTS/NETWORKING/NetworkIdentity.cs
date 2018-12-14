using System.Collections;
using UnityEngine;

namespace NETWORKING
{
    public class NetworkIdentity : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(Countdown());
        }

        private IEnumerator Countdown()
        {
            yield return new WaitForSeconds(3);
            Events.OnMatchJoined(this);
        }
        
        public int Id;
        public ulong SteamId;
    }
}