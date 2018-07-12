using System;
using UnityEngine;
using Types = DATA.Types;
    
namespace PLAYER
{
    public delegate void OnActionEndCallback();

    [RequireComponent(typeof(PlayerData))]
    public class PlayerScript : MonoBehaviour
    {
        public static event OnActionEndCallback OnActionEnd;

        private PlayerData _data;

        private void Start ()
        {
            _data = GetComponent<PlayerData>();

            _data.Direction = Types.Direction.RIGHT;
            _data.Grounded = true;
        }

        private void Update()
        {
            Debug.Log($"{_data.CurrentAction} {_data.Direction}");
        }

        private void ExecuteAction()
        {
            OnActionEnd?.Invoke();
        }

    }
}
