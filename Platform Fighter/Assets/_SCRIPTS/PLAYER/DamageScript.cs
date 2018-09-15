using TOOLS;
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

		public void ApplyDamage(double Damage, double KnockbackStrength, double KnockbackAngle)
		{
			_playerData.Percent += Damage;

			//Find some way to determine direction
			_playerData.KnockbackVelocity.x = (float)((KnockbackStrength + KnockbackStrength * _playerData.Percent) * Mathf.Cos((float)KnockbackAngle)); //* positive or negitive for left or right
			_playerData.KnockbackVelocity.x = (float)((KnockbackStrength + KnockbackStrength * _playerData.Percent) * Mathf.Sin((float)KnockbackAngle));

			Debug.Log("DAMAGE BITCH");
		}
	}
}
