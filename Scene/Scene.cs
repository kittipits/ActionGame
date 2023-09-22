using DxLibDLL;

namespace ActionGame
{
    public abstract class Scene
    {
        public abstract void Update();

        public abstract void Draw();

        public virtual void End()
        { 
        }

        public static void DrawStringCenter(int y, string s, uint color)
        {
            int width = DX.GetDrawStringWidth(s, s.Length);
            int screenWidth = 640;
            int x = screenWidth / 2 - width / 2;
            DX.DrawString(x, y, s, color);
        }

        public void DrawStringCenterToHandle(int y, string s, uint color, int fontHandle)
        {
            int width = DX.GetDrawStringWidthToHandle(s, s.Length, fontHandle);
            int screenWidth = Screen.Width;
            int x = screenWidth / 2 - width / 2;
            DX.DrawStringToHandle(x, y, s, color, fontHandle);
        }

        public void DrawStringLeftScreen(int y, string s, uint color, int fontHandle)
        {
            int width = DX.GetDrawStringWidthToHandle(s, s.Length, fontHandle);
            int screenWidth = Screen.Width / 2;
            int x = screenWidth / 2 - width / 2;
            DX.DrawStringToHandle(x, y, s, color, fontHandle);
        }

        public void DrawStringRightScreen(int y, string s, uint color, int fontHandle)
        {
            int width = DX.GetDrawStringWidthToHandle(s, s.Length, fontHandle);
            int screenWidth = Screen.Width / 2;
            int x = (Screen.Width / 2) + (screenWidth / 2 - width / 2);
            DX.DrawStringToHandle(x, y, s, color, fontHandle);
        }
    }
}
