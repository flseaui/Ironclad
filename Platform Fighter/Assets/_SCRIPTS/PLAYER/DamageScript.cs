using UnityEngine;

namespace PLAYER
{
    public class DamageScript : MonoBehaviour
    {
        private PlayerData _playerData;

        private void Awake()
        {
            _playerData = GetComponent<PlayerData>();
        }

        public void ApplyDamage(double damage, double knockbackStrength, double knockbackAngle)
        {
            _playerData.Percent += damage;

            //Find some way to determine direction
            _playerData.KnockbackVelocity.x = (float) ((knockbackStrength + knockbackStrength * _playerData.Percent) *
                                                       Mathf.Cos((float) knockbackAngle)
                ); //* positive or negitive for left or right
            _playerData.KnockbackVelocity.x = (float) ((knockbackStrength + knockbackStrength * _playerData.Percent) *
                                                       Mathf.Sin((float) knockbackAngle));

            Debug.Log("DAMAGE BITCH");
        }
    }
}