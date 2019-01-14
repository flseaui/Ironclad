using System.Reflection;
using ATTRIBUTES;
using NETWORKING;
using UnityEngine;

namespace MISC
{
    /// <summary>
    /// Allows for controlled updating of a script through ControlledStep(). 
    /// <para>
    /// Auto-steps on FixedUpdate unless ControlledStep() has previously been called that frame.
    /// </para>
    /// <para>
    /// StepOrderAttribute can be used to denote stepping order during a rollback, defaults to last.
    /// </para>
    /// </summary>
    [StepOrder(999)]
    public abstract class Steppable : MonoBehaviour
    {
        private bool _stepNext;

        private void Awake()
        {
            RollbackManager.Instance.AddSteppable(this, GetType().GetCustomAttribute<StepOrderAttribute>()?.StepOrder ?? 999);
            LateAwake();
        }

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

        /// <summary>
        /// Called after Steppable's Awake
        /// </summary>
        protected virtual void LateAwake() { }
    }
}