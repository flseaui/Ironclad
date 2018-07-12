using UnityEngine;
using static DATA.Types;

namespace PLAYER
{
    public interface IBehaviors
    {

        void RunAction(ActionType action, ref PlayerData data);

    }
}
