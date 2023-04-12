using System;

namespace Lab1TrainSchedule
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            bool process = true;
            var s = new Solution();
            s.FastAdd("Lozova","Kharkiv",10,20,10,30);
            s.FastAdd("Kharkiv","Lozova",9,40,9,55);
            s.FastAdd("Dnipro","Kharkiv",7,20,7,50);
            s.FastAdd("Kharkiv","Dnipro",9,20,9,40);
            s.FastAdd("Odessa","Kharkiv",23,40,1,10);
            s.FastAdd("Kyiv","Sevastopol`",23,40,2,30);
            s.FastAdd("Kyiv","Sevastopol`",6,40,8,00);
            s.FastAdd("Odessa","Kyiv",10,10,10,40);
            s.FastAdd("Kharkiv","Kyiv",18,40,19,30);
            while (process)
            {
                Console.WriteLine("1.Print schedule\n2.Add train to schedule\n3.Check available trains\n4.Check the nearest train on the provided direction of departure");
                switch (Console.ReadLine())
                {
                    case "1":
                        s.PrintAll();
                        break;
                    case "2":
                        s.AddTrainToSchedule();
                        Console.Clear();
                        s.PrintAll();
                        break;
                    case "3":
                        s.Available();
                        break;
                    case "4":
                        s.Nearest();
                        break;
                    case "5":
                        process = false;
                        break;
                }
            }
        }
    }
}