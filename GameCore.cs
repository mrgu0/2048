using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048
{
    class GameCore
    {
        private int[,] map;		//地图
        private int[,] originalMap;		//之前的地图 对比是否发生变化
        private int[] mergeArray;
        private int[] removeZeroArray;
        private Random random;
        private List<Location> emptyLOC;    //布局
        public int[,] Map
        {
            get
            { return map; }
        }
        /// <summary>
        /// 构造
        /// </summary>
        public GameCore()
        {
            map = new int[4, 4];
            originalMap = new int[4, 4];
            mergeArray = new int[map.GetLength(0)];
            removeZeroArray = new int[map.GetLength(0)];
            emptyLOC = new List<Location>(16);
            random = new Random();
        }

       
        /// <summary>
        /// 去零
        /// </summary>
        private void RemoveZero()
        {
            Array.Clear(removeZeroArray, 0, removeZeroArray.Length);
            int index = 0;

            for (int i = 0; i < mergeArray.Length; i++)
            {
                if (mergeArray[i] != 0)
                    removeZeroArray[index++] = mergeArray[i];
            }
            removeZeroArray.CopyTo(mergeArray, 0);
        }
        /// <summary>
        /// 合并
        /// </summary>
        private void Merge()
        {
            RemoveZero();//调用去零
            for (int i = 0; i < mergeArray.Length - 1; i++)
            {
                if (mergeArray[i] == mergeArray[i + 1] && mergeArray[i] != 0)
                {
                    mergeArray[i] += mergeArray[i + 1];
                    mergeArray[i + 1] = 0;
                }
            }
            RemoveZero();//调用去零
        }
        private void MoveUp()
        {
            for (int c = 0; c < map.GetLength(1); c++)
            {
                for (int r = 0; r < map.GetLength(0); r++)
                    mergeArray[r] = map[r, c];

                Merge();

                for (int r = 0; r < map.GetLength(0); r++)  //更新地图
                    map[r, c] = mergeArray[r];
            }
        }
        private void MoveDown()
        {
            for (int c = 0; c < map.GetLength(1); c++)
            {
                for (int r = map.GetLength(0) - 1; r >= 0; r--)
                {
                    mergeArray[3 - r] = map[r, c];
                }

                Merge();

                for (int r = map.GetLength(0) - 1; r >= 0; r--)
                {
                    map[r, c] = mergeArray[3 - r];
                }
            }
        }
        private void MoveLeft()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                    mergeArray[c] = map[r, c];

                Merge();

                for (int c = 0; c < 4; c++)
                    map[r, c] = mergeArray[c];
            }
        }
        private void MoveRight()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int c = 3; c >= 0; c--)
                    mergeArray[3 - c] = map[r, c];

                Merge();

                for (int c = 3; c >= 0; c--)
                    map[r, c] = mergeArray[3 - c];
            }
        }
        /// <summary>
        /// 地图是否改变
        /// </summary>
        public bool IsChange { get; set; }
        public void Move(MoveDirection direction)
        {
            //移动前记录Map   
            Array.Copy(map, originalMap, map.Length);
            IsChange = false;//假设没有发生改变

            switch (direction)
            {
                case MoveDirection.Up: MoveUp(); break;
                case MoveDirection.Down: MoveDown(); break;
                case MoveDirection.Left: MoveLeft(); break;
                case MoveDirection.Right: MoveRight(); break;
            }
            //移动后对比  重构 --> 提取方法
            CheckMapChange();
        }
        /// <summary>
        /// 检查地图改变
        /// </summary>
        private void CheckMapChange()
        {
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] != originalMap[r, c])
                    {
                        IsChange = true;//发生改变
                        return;
                    }
                }
            }
        }
        /*
           在空白位置上， 随机生成数字(2 (90%)     4(10%))
        * 1.计算空白位置
        * 2.随机选择位置
        * 3.随机生成数字
        */
        /// <summary>
        /// 保存空坐标
        /// </summary>
        private void CalculateEmpty()
        {
            emptyLOC.Clear();
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == 0)
                    {
                        emptyLOC.Add(new Location(r, c));
                    }
                }
            }
        }
        /// <summary>
        /// 生成新数字
        /// </summary>
        public void GenerateNumber(out Location loc, out int newNumber)
        {
            CalculateEmpty();
            if (emptyLOC.Count > 0)
            {
                int emptyLocIndex = random.Next(0, emptyLOC.Count);//0,15   //选一个坐标

                loc = emptyLOC[emptyLocIndex];//把坐标给loc

                newNumber = map[loc.RIndex, loc.CIndex] = random.Next(0, 10) == 1 ? 4 : 2;//十分之一概率

                //将该位置清除
                emptyLOC.RemoveAt(emptyLocIndex);
            }
            else
            {
                newNumber = -1;
                loc = new Location(-1, -1);
            }
        }
        /// <summary>
        /// 游戏是否结束
        /// </summary>
        public bool IsOver()
        {
            if (emptyLOC.Count > 0) return false; //有0 就继续
            ///有相同的继续
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (map[r, c] == map[r, c + 1] || map[c, r] == map[c + 1, r])
                        return false;
                }
            }
            return true;//游戏结束 
        }
    }
}









