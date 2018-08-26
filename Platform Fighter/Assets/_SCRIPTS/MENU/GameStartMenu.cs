using System.Collections;
using ATTRIBUTES;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Types = DATA.Types;

namespace MENU
{
    [MenuType(Types.Menu.GameStartMenu)]
    public class GameStartMenu : Menu
    {
        [SerializeField] private TextMeshProUGUI _loadingText;
        private bool _loadScene;

        protected override void SwitchToThis(params string[] args)
        {
            if (SceneManager.GetActiveScene().name.Equals("MENU_SCENE"))
            {
                _loadScene = true;
                StartCoroutine(LoadNewScene("GAME_SCENE"));
            }
        }

        private void Update()
        {
            if (_loadScene)
                _loadingText.color = new Color(_loadingText.color.r, _loadingText.color.g, _loadingText.color.b,
                    Mathf.PingPong(Time.time, 1));
        }

        private IEnumerator LoadNewScene(string scene)
        {
            yield return new WaitForSeconds(3);

            var async = SceneManager.LoadSceneAsync(scene);

            while (!async.isDone) yield return null;
        }
    }
}