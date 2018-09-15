using System;
using System.IO;
using Facepunch.Steamworks;
using TOOLS;
using UnityEngine;
using UnityEngine.Profiling;

namespace MISC
{
    public class SteamClient : MonoBehaviour
    {
        public uint AppId;

        private Client client;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            if (AppId == 0)
                throw new Exception("You need to set the AppId to your game");

            Config.ForUnity(Application.platform.ToString());

            // Create a steam_appid.txt
            try
            {
                File.WriteAllText("steam_appid.txt", AppId.ToString());
            }
            catch (Exception e)
            {
                Debug.LogWarning("Couldn't write steam_appid.txt: " + e.Message);
            }

            // Create the client
            client = new Client(AppId);

            if (!client.IsValid)
            {
                client = null;
                Debug.LogWarning("Couldn't initialize Steam");
                return;
            }

            Debug.Log("Steam Initialized: " + client.Username + " / " + client.SteamId);
        }

        private void Update()
        {
            if (client == null)
                return;

            try
            {
                Profiler.BeginSample("Steam Update");
                client.Update();
            }
            finally
            {
                Profiler.EndSample();
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