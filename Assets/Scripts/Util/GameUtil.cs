using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace RougeLike.Util
{
    public static class GameUtil
    {
        /// <summary>
        /// 带概率的随机数
        /// </summary>
        /// <param name="ratio">概率数组.</param>
        /// <param name="total">总概率</param>
        /// <returns>概率数组索引</returns>
        /// ex:ration = [30,70];total = 100;
        /// 30%概率返回0,70%概率返回1
        public static int GetRandWithPossibility(int[] ratio, int total)
        {
            int seed = Random.Range(0, total + 1);
            int start = 0;
            for (int i = 0; i < ratio.Length;i++)
            {
                start += ratio[i];
                if (seed <= start)
                    return i;
            }

            return -1;
        }
        public static void Swap<T>(this IList<T> list, int a, int b)
        {
            var temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }
        public static void RandomSort<T>(this IList<T> list)
        {
            int len = list.Count;
            int i, j = len - 1;
            for (i = 0; i < len; i++)
                list.Swap(UnityEngine.Random.Range(0, len - i), j--);
        }
        public static float PointLineDistance(Vector3 point,Vector3 p1,Vector3 p2)
        {
            var sideVector = p1 - p2;
            Vector3 tarVector = point - p1;
            float area = Vector3.Cross(tarVector, sideVector).magnitude;
            return area / sideVector.magnitude;
        }
        
    }
}