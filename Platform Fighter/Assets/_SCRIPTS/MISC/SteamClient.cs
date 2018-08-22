using Facepunch.Steamworks;
using UnityEngine;

namespace MISC
{
    public class SteamClient : MonoBehaviour
    {
        public uint AppId;

        private Facepunch.Steamworks.Client client;

        private void Start ()
        {
            DontDestroyOnLoad(gameObject);

            if (AppId == 0)
                throw new System.Exception("You need to set the AppId to your game");

            Facepunch.Steamworks.Config.ForUnity( Application.platform.ToString() );

            // Create a steam_appid.txt
            try
            {    
                System.IO.File.WriteAllText("steam_appid.txt", AppId.ToString());
            }
            catch ( System.Exception e )
            {
                Debug.LogWarning("Couldn't write steam_appid.txt: " + e.Message );
            }

            // Create the client
            client = new Facepunch.Steamworks.Client( AppId );

            if ( !client.IsValid )
            {
                client = null;
                Debug.LogWarning("Couldn't initialize Steam");
                return;
            }

            Debug.Log( "Steam Initialized: " + client.Username + " / " + client.SteamId ); 
        }

        private void Update()
        {
            if (client == null)
                return;

            try
            {
                UnityEngine.Profiling.Profiler.BeginSample("Steam Update");
                client.Update();
            }
            finally
            {
                UnityEngine.Profiling.Profiler.EndSample();
            }
        }

        private void OnDestroy()
        {
            if (client != null)
            {
                
                client.Dispose();
                client = null;
            }
        }
    }
}