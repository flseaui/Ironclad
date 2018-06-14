// GENERATED AUTOMATICALLY FROM 'Assets/_INPUTACTIONS/CharacterControls.inputactions'

namespace PlatformFighter
{
    [System.Serializable]
    public class CharacterControls : UnityEngine.Experimental.Input.InputActionWrapper
    {
        private bool m_Initialized;
        private void Initialize()
        {
            // XInputGamepad
            m_XInputGamepad = asset.GetActionMap("XInputGamepad");
            m_XInputGamepad_WalkLeft = m_XInputGamepad.GetAction("WalkLeft");
            m_XInputGamepad_WalkRight = m_XInputGamepad.GetAction("WalkRight");
            m_XInputGamepad_Idle = m_XInputGamepad.GetAction("Idle");
            m_XInputGamepad_RunLeft = m_XInputGamepad.GetAction("RunLeft");
            m_XInputGamepad_RunRight = m_XInputGamepad.GetAction("RunRight");
            m_XInputGamepad_Jump = m_XInputGamepad.GetAction("Jump");
            m_XInputGamepad_ShortHop = m_XInputGamepad.GetAction("ShortHop");
            m_XInputGamepad_Crouch = m_XInputGamepad.GetAction("Crouch");
            m_XInputGamepad_Jab = m_XInputGamepad.GetAction("Jab");
            m_XInputGamepad_NeutralSpecial = m_XInputGamepad.GetAction("NeutralSpecial");
            m_Initialized = true;
        }
        // XInputGamepad
        private UnityEngine.Experimental.Input.InputActionMap m_XInputGamepad;
        private UnityEngine.Experimental.Input.InputAction m_XInputGamepad_WalkLeft;
        private UnityEngine.Experimental.Input.InputAction m_XInputGamepad_WalkRight;
        private UnityEngine.Experimental.Input.InputAction m_XInputGamepad_Idle;
        private UnityEngine.Experimental.Input.InputAction m_XInputGamepad_RunLeft;
        private UnityEngine.Experimental.Input.InputAction m_XInputGamepad_RunRight;
        private UnityEngine.Experimental.Input.InputAction m_XInputGamepad_Jump;
        private UnityEngine.Experimental.Input.InputAction m_XInputGamepad_ShortHop;
        private UnityEngine.Experimental.Input.InputAction m_XInputGamepad_Crouch;
        private UnityEngine.Experimental.Input.InputAction m_XInputGamepad_Jab;
        private UnityEngine.Experimental.Input.InputAction m_XInputGamepad_NeutralSpecial;
        public struct XInputGamepadActions
        {
            private CharacterControls m_Wrapper;
            public XInputGamepadActions(CharacterControls wrapper) { m_Wrapper = wrapper; }
            public UnityEngine.Experimental.Input.InputAction @WalkLeft { get { return m_Wrapper.m_XInputGamepad_WalkLeft; } }
            public UnityEngine.Experimental.Input.InputAction @WalkRight { get { return m_Wrapper.m_XInputGamepad_WalkRight; } }
            public UnityEngine.Experimental.Input.InputAction @Idle { get { return m_Wrapper.m_XInputGamepad_Idle; } }
            public UnityEngine.Experimental.Input.InputAction @RunLeft { get { return m_Wrapper.m_XInputGamepad_RunLeft; } }
            public UnityEngine.Experimental.Input.InputAction @RunRight { get { return m_Wrapper.m_XInputGamepad_RunRight; } }
            public UnityEngine.Experimental.Input.InputAction @Jump { get { return m_Wrapper.m_XInputGamepad_Jump; } }
            public UnityEngine.Experimental.Input.InputAction @ShortHop { get { return m_Wrapper.m_XInputGamepad_ShortHop; } }
            public UnityEngine.Experimental.Input.InputAction @Crouch { get { return m_Wrapper.m_XInputGamepad_Crouch; } }
            public UnityEngine.Experimental.Input.InputAction @Jab { get { return m_Wrapper.m_XInputGamepad_Jab; } }
            public UnityEngine.Experimental.Input.InputAction @NeutralSpecial { get { return m_Wrapper.m_XInputGamepad_NeutralSpecial; } }
            public UnityEngine.Experimental.Input.InputActionMap Get() { return m_Wrapper.m_XInputGamepad; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public UnityEngine.Experimental.Input.InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator UnityEngine.Experimental.Input.InputActionMap(XInputGamepadActions set) { return set.Get(); }
        }
        public XInputGamepadActions @XInputGamepad
        {
            get
            {
                if (!m_Initialized) Initialize();
                return new XInputGamepadActions(this);
            }
        }
    }
}
