using System.Linq;
using Facepunch.Steamworks;
using UnityEngine;

namespace MISC
{
    public class SteamTest : MonoBehaviour
    {
        private void Start()
        {
            // Don't destroy this when loading new scenes
            DontDestroyOnLoad(gameObject);

            // Configure for Unity
            // This is VERY important - call this before doing anything
            Config.ForUnity(Application.platform.ToString());

            // Create the steam client using the test AppID (or your own AppID eventually)
            var client = new Client(480);

            // Make sure we started up okay
            if (Client.Instance == null)
            {
                Debug.LogError("Error starting Steam!");
                return;
            }

            // Print out some basic information
            Debug.Log("My Steam ID: " + Client.Instance.SteamId);
            Debug.Log("My Steam Username: " + Client.Instance.Username);
            Debug.Log("My Friend Count: " + Client.Instance.Friends.AllFriends.Count());
        }

        private void OnDestroy()
        {
            if (Client.Instance != null) Client.Instance.Dispose();
        }


        private void Update()
        {
            if (Client.Instance != null) Client.Instance.Update();
        }
    }
}