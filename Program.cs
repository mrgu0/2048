using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048
{
    class Program
    {
       
        static void Main()
        {
           
            GameCore g = new GameCore();
            Location x;
            int y;
            string str;
           
            g.GenerateNumber(out x, out y);
            g.GenerateNumber(out x, out y);
            while (true )
            {
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine();
                    Console.Write("                          ");
                    for (int c = 0; c < 4; c++)
                    {
                        if(g.Map [i,c]==0)
                            Console.Write(  "          ");
                        else 
                        Console.Write(g.Map[i, c] + "         ");
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                }
                
                str = Console.ReadLine();
                if(str=="w")
                    g.Move(MoveDirection.Up);
                if (str == "s")
                    g.Move(MoveDirection.Down);
                if (str == "a")
                    g.Move(MoveDirection.Left );
                if (str == "d")
                    g.Move(MoveDirection.Right );





                if (g.IsChange ==true   )
                    g.GenerateNumber(out x, out y);
                Console.Clear(); 
            }

        }
    }
}
