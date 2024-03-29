using Facepunch.Steamworks;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using Image = Facepunch.Steamworks.Image;

namespace MENU
{
//
// To change at runtime set SteamId then call Fetch()
//
    public class SteamAvatar : MonoBehaviour
    {
        public Texture FallbackTexture;
        public Friends.AvatarSize Size;
        public ulong SteamId;

        private void Start()
        {
            Fetch();
        }

        public void Fetch()
        {
            if (SteamId == 0) return;
            if (Client.Instance == null)
            {
                ApplyTexture(FallbackTexture);
                return;
            }

            Client.Instance.Friends.GetAvatar(Size, SteamId, OnImage);
        }

        private void OnImage(Image image)
        {
            if (image == null)
            {
                ApplyTexture(FallbackTexture);
                return;
            }

            var texture = new Texture2D(image.Width, image.Height);

            for (var x = 0; x < image.Width; x++)
            for (var y = 0; y < image.Height; y++)
            {
                var p = image.GetPixel(x, y);

                texture.SetPixel(x, image.Height - y,
                    new Color(p.r / 255.0f, p.g / 255.0f, p.b / 255.0f, p.a / 255.0f));
            }

            texture.Apply();

            ApplyTexture(texture);
        }

        private void ApplyTexture(Texture texture)
        {
            var rawImage = GetComponent<RawImage>();
            if (rawImage != null)
                rawImage.texture = texture;
        }
    }
}