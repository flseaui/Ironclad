using UnityEngine;
using UnityEngine.Networking;

class PlayerInfo : NetworkBehaviour
{
	[SyncVar]
	public Color color;
}