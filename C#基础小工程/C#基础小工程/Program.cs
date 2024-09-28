using System.Numerics;

namespace Csharp基础项目
{
    enum E_player//提供玩家和电脑类型
    {
        player,
        computer,
    }
   enum E_gridType//表示格子类型 以便结构体来调取
    {
        normal,
        boom,
        tunnel,
        pause,
    }
   enum E_gameScence//表示游戏场景的枚举
    {
        /// <summary>
        /// 游戏开始场景
        /// </summary>
        gameStart,
        /// <summary>
        /// 游戏进行场景
        /// </summary>
        gamePlaying,
        /// <summary>
        /// 结束画面
        /// </summary>
        gameEnd,
    }

    struct Map//画地图的
    {
        public Grid[] gridArr;
        public int length;
        public Random r;
        public int content;//存取随机数的整形值
        public int posX, posY;
        public int step;
        public int ten_nine;
        public bool cycbroken;
        public int count;
        public Map(int length)
        {
            cycbroken = false;
            r = new Random();
            this.length = length;
            gridArr = new Grid[length];
            for (int i = 0; i < length; i++)
            {
                content = r.Next(1,101);
                if (content < 80)
                {
                    gridArr[i].gridType = E_gridType.normal;
                }
                else if (content >=80 && content < 90)
                {
                    gridArr[i].gridType = E_gridType.pause;
                }
                else if (content >= 90 && content < 95)
                {
                    gridArr[i].gridType = E_gridType.boom;
                }
                else
                {
                    gridArr[i].gridType = E_gridType.tunnel;
                }
            }
        }


        public void DrawMap(int x ,int y)
        {
            count = 0;
            ten_nine = 0;
            step = 2;
            while (true) 
            {
                switch (ten_nine)
                {
                    case 0:
                        
                        for (int i = 0; i < length; i++)
                        {
                            if (count == length)
                            {
                                return;
                            }
                            if (posX < 10)
                            {

                                x += step;
                                posX++;
                                if (posY >= 2)
                                {
                                    posY = 0;
                                }
                            }
                            else
                            {
                                if (posY < 2)
                                {
                                    y += 1;
                                    posY++;
                                }
                                else
                                {
                                    step = -step;
                                    posX = 0;
                                    ten_nine = 1;
                                    cycbroken = true;
                                }
                            }
                            count++;
                            gridArr[i].pos.x = x;
                            gridArr[i].pos.y = y;
                            Grid grid = new Grid(x, y, gridArr[i].gridType);
                            grid.DrawGrid();
                            if (cycbroken)
                            {
                                break;
                            }
                        }
                        break;
                    case 1:
                        
                        for (int i = 0; i < length; i++)
                        {
                            if (count == length)
                            {
                                return;
                            }
                            if (posX < 9)
                            {
                                x += step;
                                posX++;
                                if (posY >= 2)
                                {
                                    posY = 0;
                                }
                            }
                            else
                            {
                                if (posY < 2)
                                {
                                    y += 1;
                                    posY++;
                                }
                                else
                                {
                                    step = -step;
                                    posX = 0;


                                }
                            }
                            count++;
                            gridArr[12+i].pos.x = x;
                            gridArr[12+i].pos.y = y;
                            Grid grid = new Grid(x, y, gridArr[i].gridType);
                            grid.DrawGrid();


                            




                        }
                        break;
                }


            }
            
           
            
        }
    }
    struct Pos//表示格子的位置
    {
        public int x;
        public int y;

        public Pos(int x, int y) 
        { 
            this.x = x; 
            this.y = y; 
        }
    }

    struct Player//根据不同玩家类型 
    {
        public E_player player;
        public int nowindex;
        public bool isPause;
        



        public Player(int x,E_player e_Player)
        {
            nowindex = x;
            player = e_Player;
            isPause = false;



        }

        public void DrawPlayer( Map infor)
        {
            Grid grid = infor.gridArr[nowindex];
            Console.SetCursorPosition(grid.pos.x, grid.pos.y);
            switch (player)
            {
                case E_player.player:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("&");
                    break;
                case E_player.computer:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("▲");
                    break;
            }

        }
    }

    struct Grid //提供了格子类型和位置的集合 也包含绘画函数
    {
        public E_gridType gridType;
        public Pos pos;

        public Grid(int x, int y, E_gridType type)
        {
            this.gridType = type;
            pos = new Pos(x, y);
        }

        public void DrawGrid()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            switch (gridType)
            {
                case E_gridType.normal:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("□");
                    break;
                case E_gridType.boom:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("●");
                    break;
                case E_gridType.tunnel:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("¤");
                    break;
                case E_gridType.pause:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("||");
                    break;

            }
        }
    }
    internal class Program
    {
        #region 扔色子逻辑
        static bool RandomMove(int x, int y, ref Player p1, ref Player p2, Map map)
        {
            if (p1.isPause)
            {
                p1.isPause = false;
                return true;

            }
            Grid grid = map.gridArr[p1.nowindex];
            Random move = new Random();
            if (p1.nowindex >= 80)
            {
                p1.nowindex = 80;
            }
            else
            {
                p1.nowindex += move.Next(1, 6);
            }
            switch (grid.gridType)
            {
                case E_gridType.normal:
                    break;
                case E_gridType.boom:
                    if (p1.nowindex <= 0)
                    {
                        p1.nowindex = 0;
                    }
                    else
                    {
                        p1.nowindex -= 5;
                    }
                    break;
                case E_gridType.tunnel:
                    int r = move.Next(1,4);
                    switch (r)
                    {
                        case 1:
                            p1.isPause = true;
                            break;
                        case 2:
                            if (p1.nowindex <= 0)
                            {
                                p1.nowindex = 0;
                            }
                            else
                            {
                                p1.nowindex -= 5;
                            }
                            break;
                        case 3:
                            Player tem = p2;
                            p1 = p2;
                            p2 = tem;
                            break;
                    }
                    break;
                case E_gridType.pause:
                    p1.isPause = true;
                    break;
                
            }
            return true;
        }
        #endregion
        #region 绘画玩家电脑函数
        static void DrawPlayerComputer(Player player , Player computer , Map infor)
        {
            if (player.nowindex == computer.nowindex)
            {
                Grid grid = infor.gridArr[player.nowindex];
                Console.SetCursorPosition(grid.pos.x, grid.pos.y);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("◎");

            }
            else
            {
                player.DrawPlayer(infor);
                computer.DrawPlayer(infor);
            }

        }
        #endregion
        #region 控制台相关基础设置 包含游戏主场景
        static void ConsoleSetting()
        {
            bool cyc = false;//开始菜单跳出循环的标志
            int w = 50;//代表宽度
            int h = 30;//代表高度
            Console.SetWindowSize(w, h);//设置窗口大小
            Console.SetBufferSize(w, h);//设置缓存区大小
            Console.CursorVisible = false;//关闭鼠标显示
            #region 显示相关
            E_gameScence GameScence = E_gameScence.gameStart;
            
            int SlectColor = 0;//关于颜色变化
            Console.SetCursorPosition(w/2-3, 2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("飞行棋");
            

            //以下为switch语句来判断处于哪个游戏场景
            while (true)
            {
                switch (GameScence)
                {
                    case E_gameScence.gameStart:
                        Console.Clear();
                        Console.SetCursorPosition(w / 2 - 3, 2);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("飞行棋");
                        //以下的switch语句来判断开始界面的选择
                        while (true) 
                        {
                            Console.SetCursorPosition(w / 2 - 4, 8);
                            Console.ForegroundColor = SlectColor == 0 ? ConsoleColor.Red : ConsoleColor.White;
                            Console.Write("开始游戏");
                            Console.SetCursorPosition(w / 2 - 4, 10);
                            Console.ForegroundColor = SlectColor == 1 ? ConsoleColor.Red : ConsoleColor.White;
                            Console.Write("退出游戏");
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.W:
                                    SlectColor = 0;
                                    break;
                                case ConsoleKey.S:
                                    SlectColor = 1;
                                    break;
                                case ConsoleKey.J:
                                    if(SlectColor == 0)
                                    {
                                        GameScence = E_gameScence.gamePlaying;
                                        cyc = true;
                                    }
                                    else
                                    {
                                        Environment.Exit(0);
                                    }
                                    break;
                            }
                            if (cyc)//用于跳出循环
                            {
                                break;
                            }

                        }
                        break;
                    case E_gameScence.gamePlaying:
                        Console.Clear();
                        DrawWall(w, h);
                        Map map = new Map(80);
                        map.DrawMap(14, 4);
                        Player player = new Player(0, E_player.player);
                        Player computer = new Player(0, E_player.computer);
                  
                        DrawPlayerComputer(player, computer, map);
                        Console.ReadKey(true);
                        while (true)
                        {
                            RandomMove(w,h,ref player,ref computer,map);
                            DrawPlayerComputer(player, computer, map);
                            Console.ReadKey(true);
                        }
                        
                        
                        break;
                    case E_gameScence.gameEnd:
                        Console.Clear();
                        break;

                }

            }

            #endregion


        }
        #endregion
        
        #region 不变的红墙(包含主要固定信息)
        #region 不变的红墙 红墙部分
        static void DrawWall(int w,int h)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < w; i += 2)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, h-1);
                Console.Write("■");
                Console.SetCursorPosition(i, h-1-5);
                Console.Write("■");
                Console.SetCursorPosition(i, h - 1 - 5 - 5);
                Console.Write("■");
            }
            for (int i = 0; i < h; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(48, i);
                Console.Write("■");

            }

            #region 文字部分
            Console.SetCursorPosition(2, h - 1 - 5 - 4);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("□:普通格子");
            Console.SetCursorPosition(2, h - 1 - 5 - 3);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("||:暂停,一回合不动");
            Console.SetCursorPosition(2 + 24, h - 1 - 5 - 3);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("●:炸弹,倒退五格");
            Console.SetCursorPosition(2, h - 1 - 5 - 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("¤:时空隧道,随机暂停,倒退,换位置");
            Console.SetCursorPosition(2, h - 1 - 5 - 1);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("&:玩家");
            Console.SetCursorPosition(2 + 10, h - 1 - 5 - 1);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("▲:电脑");
            Console.SetCursorPosition(2 + 10 + 10, h - 1 - 5 - 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("◎:电脑玩家重合");
            Console.SetCursorPosition(2 , h - 1 - 4);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("按任意键开始扔色子");
            #endregion

        }
        #endregion






        #endregion
        #region 主函数
        //游戏主运行语句块
        private static void Main(string[] args)
        {
            
                ConsoleSetting();
                
               
            
        }
        #endregion

    }
}
