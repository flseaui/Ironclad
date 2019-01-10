using UnityEngine;

namespace MISC
{
    /// <summary>
    /// Allows for controlled updating of a script through ControlledStep(). 
    /// <para>
    /// Auto-steps on FixedUpdate unless ControlledStep() has previously been called that frame.
    /// </para>
    /// </summary>
    public abstract class Steppable : MonoBehaviour
    {
        private bool _stepNext;

        private void FixedUpdate()
        {
            if (_stepNext)
                Step();
            else
                _stepNext = true;
        }

        /// <summary>
        /// Manually simulate a game frame, disables auto-step for next FixedUpdate.
        /// </summary>
        public void ControlledStep()
        {
            Step();
            _stepNext = false;
        }

        /// <summary>
        /// Simulates one game frame, called from FixedUpdate.
        /// </summary>
        protected abstract void Step();
    }
}