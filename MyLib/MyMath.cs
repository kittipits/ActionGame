using System;

namespace MyLib
{
    public static class MyMath
    {
        public const float Sqrt2 = 1.41421356237f;
        public const float PI = 3.14159265359f;
        public const float Deg2Rad = PI / 180f;

        /// <summary>
        /// 円と円が重なっているかを調べる
        /// </summary>
        /// <param name="x1">円1の中心x</param>
        /// <param name="y1">円1の中心y</param>
        /// <param name="radius1">円1の半径</param>
        /// <param name="x2">円2の中心x</param>
        /// <param name="y2">円2の中心y</param>
        /// <param name="radius2">円2の半径</param>
        /// <returns>重なっていればtrue、重なっていなければfalseを返却する</returns>
        public static bool CircleCircleIntersection(
            float x1, float y1, float radius1,
            float x2, float y2, float radius2)
        { 
            return ((x1-x2) * (x1-x2) + (y1-y2) * (y1-y2)) 
                < ((radius1 + radius2) * (radius1 + radius2));
        }

        /// <summary>
        /// 四角形同士が重なっているか？
        /// </summary>
        /// <param name="aLeft">>A左端</param>
        /// <param name="aTop">A上端</param>
        /// <param name="aRight">A右端</param>
        /// <param name="aBottom">A下端</param>
        /// <param name="bLeft">B左端</param>
        /// <param name="bTop">B上端</param>
        /// <param name="bRight">B右端</param>
        /// <param name="bBottom">B下端</param>
        /// <returns></returns>
        public static bool RectRectIntersect(
            float aLeft, float aTop, float aRight, float aBottom,
            float bLeft, float bTop, float bRight, float bBottom)
        {
            return
                aLeft < bRight &&
                aRight > bLeft &&
                aTop < bBottom &&
                aBottom > bTop;
        }

        /// <summary>
        /// 点から点への角度（ラジアン）を求める。
        /// </summary>
        /// <param name="fromX">始点x</param>
        /// <param name="fromY">始点y</param>
        /// <param name="toX">終点x</param>
        /// <param name="toY">終点y</param>
        /// <returns></returns>
        public static float PointToPointAngle(float fromX, float fromY, float toX, float toY)
        {
            return (float)Math.Atan2(toY - fromY, toX - fromX);
        }
        

        public static float Lerp(float a, float b, float t)
        { 
            return a + (b - a) * t;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            else
            {
                return value;
            }
        }

        public static float PointToPointDistance(float x1, float y1, float x2, float y2)
        {
            // ユークリッド距離を計算
            float deltaX = x2 - x1;
            float deltaY = y2 - y1;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}
