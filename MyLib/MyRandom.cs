using System;

namespace MyLib
{
    public static class MyRandom
    {
        //インスタンス生成
        static Random random;           
        
        public static void Init()   
        {
            random = new Random();
        }

        public static void Init(int seed)
        {
            random = new Random(seed);
        }

        //整数の乱数
        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }

        //小数の乱数
        public static float Range(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }

        //確率（％）
        public static bool Percent(float probability)
        {
            return random.NextDouble() * 100 < probability;
        }

        //-value～+valueの範囲の乱数を返却する。
        public static float PlusMinus(float value)
        {
            return Range(-value, value);
        }
    }
}
