﻿namespace PlatFighter.PLAYER
{
    public class TonkyBehaviors
    {

        public void ThatOneMethodThatWeReallyShouldNameButWeJustCantThinkOfAName (Types.ActionType action, ref double acclereation, ref double terminalVelocity) {
        
            switch (action)
            {

                case (Types.ActionType.WALK):
                    acclereation = .25;
                    terminalVelocity = 2.5;
                    break;

                case (Types.ActionType.RUN):
                    acclereation = .25;
                    terminalVelocity = 5;
                    break;

            }
        }
    }
}