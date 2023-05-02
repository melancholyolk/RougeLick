using System.Collections.Generic;
using UnityEngine;

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
            for (int i = 0; i < ratio.Length; i++)
            {
                start += ratio[i];
                if (seed <= start)
                    return i;
            }

            return -1;
        }

        public static void Swap<T>(this IList<T> list, int a, int b)
        {
            (list[a], list[b]) = (list[b], list[a]);
        }

        public static void RandomSort<T>(this IList<T> list)
        {
            int len = list.Count;
            int i, j = len - 1;
            for (i = 0; i < len; i++)
                list.Swap(UnityEngine.Random.Range(0, len - i), j--);
        }

        public static float PointLineDistance(Vector3 point, Vector3 p1, Vector3 p2)
        {
            var sideVector = p1 - p2;
            Vector3 tarVector = point - p1;
            float area = Vector3.Cross(tarVector, sideVector).magnitude;
            return area / sideVector.magnitude;
        }

        /// <summary>
        /// 随机漫步算法
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="length">迭代次数</param>
        /// <param name="step">步长最大值</param>
        /// <returns>生成点</returns>
        public static List<Vector2Int> RandomWalk(Vector2Int start, Vector2Int border, int length,
            int step = 1)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Vector2Int cur = start;
            for (int i = 0; i < length; i++)
            {
                int dir = Random.Range(0, 4);
                int dis = Random.Range(1, step + 1);
                var temp = cur;
                switch (dir)
                {
                    case 0:
                        temp += Vector2Int.up * dis;
                        break;
                    case 1:
                        temp += Vector2Int.down * dis;
                        break;
                    case 2:
                        temp += Vector2Int.left * dis;
                        break;
                    case 3:
                        temp += Vector2Int.right * dis;
                        break;
                }

                if (temp.x >= start.x - border.x / 2 && temp.x <= start.x + border.x / 2 &&
                    temp.y >= start.y - border.y / 2 && temp.y <= start.y + border.y / 2)
                {
                    cur = temp;
                    path.Add(cur);
                }
                else
                    i--;
            }

            return path;
        }

        /// <summary>
        /// 几何分割法随机地图
        /// </summary>
        /// <param name="start">分割中心点</param>
        /// <param name="border">地图边界</param>
        /// <param name="count">每个区域生成数量</param>
        /// <param name="path">生成点</param>
        /// <param name="offset">相对于原点偏移</param>
        /// <param name="currrentLength">当前迭代次数</param>
        /// <param name="length">总迭代次数</param>
        /// <returns>分割后的区域</returns>
        public static void GeometricSegmentation(Vector2 offset, Vector2Int start, Vector2Int border, int count,
            List<Vector2Int> path,
            int currrentLength, int length = 5)
        {
            if (currrentLength > length)
                return;
            currrentLength++;
            Bounds[] bounds = new Bounds[4];
            var leftUp = new Vector2(-border.x / 2, -border.y / 2);
            var rightUp = new Vector2(border.x / 2, -border.y / 2);
            var leftDown = new Vector2(-border.x / 2, border.y / 2);
            var rightDown = new Vector2(border.x / 2, border.y / 2);
            bounds[0] = new Bounds(offset + (leftUp + start) / 2,
                new Vector3(border.x + start.x, 0, border.y + start.y));
            bounds[1] = new Bounds(offset + (rightUp + start) / 2,
                new Vector3(border.x + start.x, 0, border.y - start.y));
            bounds[2] = new Bounds(offset + (leftDown + start) / 2,
                new Vector3(border.x - start.x, 0, border.y + start.y));
            bounds[3] = new Bounds(offset + (rightDown + start) / 2,
                new Vector3(border.x - start.x, 0, border.y - start.y));
            for (int i = 0; i < 4; i++)
            {
                GeometricSegmentation(
                    start, new Vector2Int((int)bounds[i].center.x, (int)bounds[i].center.z),
                    new Vector2Int((int)bounds[i].size.x, (int)bounds[i].size.z), count, path, currrentLength, length);
                for (int j = 0; j < count; j++)
                {
                    var temp = new Vector2Int(Random.Range((int)bounds[i].min.x, (int)bounds[i].max.x),
                        Random.Range((int)bounds[i].min.z, (int)bounds[i].max.z));
                    path.Add(temp);
                }
            }
        }
    }
}