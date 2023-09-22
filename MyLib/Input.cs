using DxLibDLL;

namespace MyLib
{
    public static class Input
    {
        static int prevState;
        static int currentState;

        public static void Init()
        { 
            prevState = 0;
            currentState = 0;
        }

        public static void Update()
        { 
            prevState = currentState;

            currentState = DX.GetJoypadInputState(DX.DX_INPUT_KEY_PAD1);
        }

        public static bool GetButton(int buttonId)
        { 
            return (currentState & buttonId) != 0;
        }
        public static bool GetButtonDown(int buttonId)
        {
            return ((currentState & buttonId) & ~(prevState & buttonId)) != 0;
        }
        public static bool GetButtonUp(int buttonId)
        {
            return ((prevState & buttonId) & ~(currentState & buttonId)) != 0;
        }
    }
}
