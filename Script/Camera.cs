using DxLibDLL;
using MyLib;

namespace ActionGame
{
    // 画面のスクロール量を管理するクラス。
    // 画像の描画機能を持つ。
    // スクロールの影響を受けるオブジェクトはこのクラスを通じて描画することで
    // スクロールを意識せずに描画を行うことができる。
    public static class Camera
    {
        //Clamp Camera
        const int MinCameraX = 0;
        const int MinCameraY = 0;
        const int MaxCameraX = Map.Width * Map.CellSize;
        const int MaxCameraY = Map.Height * Map.CellSize;

        // カメラの位置。
        // 画面左上のワールド座標を表す。
        public static float x;
        public static float y;

        //Lerp-Smoothing
        private static float lerpFactor = 0.12f;
        //Dual Forward Focus
        private static float focusWidth = 320f;
        private static float focusX1;
        private static float focusX2;

        // 指定されたワールド座標が画面の中心に来るように、カメラの位置を変更する
        public static void LookAt(float targetX)
        {
            //Simple Version
            //x = targetX - Screen.Width / 2;

            //Dual Forward Focus
            focusX1 = targetX;
            focusX2 = targetX + focusWidth;

            //Lerp-Smoothing
            float midX = (focusX1 + focusX2) / 2;
            x = MyMath.Lerp(x, midX - (Screen.Width / 2), lerpFactor);

            //Clamp Camera
            x = MyMath.Clamp(x, MinCameraX, MaxCameraX);
            y = MyMath.Clamp(y, MinCameraY, MaxCameraY);
        }

        /// <summary>
        /// 画像を描画する。スクロールの影響を受ける。
        /// </summary>
        /// <param name="worldX">左端のx座標</param>
        /// <param name="worldY">上端のy座標</param>
        /// <param name="handle">画像ハンドル</param>
        /// <param name="flip">左右反転するかしないか</param>
        public static void DrawGraph(float worldX, float worldY, int handle, bool flip = false)
        {
            if (flip) DX.DrawTurnGraphF(worldX - x, worldY - y, handle);
            else DX.DrawGraphF(worldX - x, worldY - y, handle);
        }

        /// <summary>
        /// 四角形（枠線のみ）を描画する
        /// </summary>
        /// <param name="left">左端</param>
        /// <param name="top">上端</param>
        /// <param name="right">右端</param>
        /// <param name="bottom">下端</param>
        /// <param name="color">色</param>
        public static void DrawLineBox(float left, float top, float right, float bottom, uint color)
        {
            DX.DrawBox((int)(left - x + 0.5f),
                (int)(top - y + 0.5f),
                (int)(right - x + 0.5f),
                (int)(bottom - y + 0.5f),
                color,DX.FALSE);
        }

        public static void DrawLifeBar(float left, float top, float right, int maxLife, int life )
        {
            DX.DrawBoxAA( left - x, top - y - 7, (left - x) + (life * (right-left) / maxLife) , top - y - 3, DX.GetColor(191, 0, 0), DX.TRUE);
        }

        public static void DrawCircle(int left, int top, int radius, uint color)
        {
            DX.DrawCircle((int)(left - x), (int)(top - y), radius, color);
        }
    }
}
