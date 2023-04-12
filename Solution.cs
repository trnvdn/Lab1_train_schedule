using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Lab1TrainSchedule
{
    public struct Train
    {
        public int Number { get; set; }
        public string Dispatch { get; set; }
        public string Destination { get; set; }
        public int ArrivalHour { get; set; }
        public int ArrivalMinute { get; set; }
        public int DepartureHour { get; set; }
        public int DepartureMinute { get; set; }
        public int Parking { get; set; }
    }
    public class Solution
    {
        private int LastElement = 0;
        private Train[] schedule = new Train[10];
        private Train train = new Train();

        void Resize()
        {
            Array.Resize(ref schedule,schedule.Length + 5);
        }
        
        public void AddTrainToSchedule()
        {
            var t = new Train();
            t.Number = LastElement + 1;
            Console.WriteLine("Enter dispatch");
            t.Dispatch = StringValidation();
            Console.WriteLine("Enter destination");
            t.Destination = StringValidation();
            Console.WriteLine("Enter arrival hour");
            t.ArrivalHour = HoursValidation();
            Console.WriteLine("Enter arrival minute");
            t.ArrivalMinute = MinutesValidation();

            t.ArrivalHour = iSSixtySeconds(t.ArrivalHour, t.ArrivalHour);

            Console.WriteLine("Enter departure hour");
            t.DepartureHour = HoursValidation();
            Console.WriteLine("Enter departure minute");
            t.DepartureMinute = MinutesValidation();
            t.DepartureHour = iSSixtySeconds(t.ArrivalHour, t.ArrivalMinute);
            if (TimeInMinutes(t.ArrivalHour, t.ArrivalMinute) < TimeInMinutes(t.DepartureHour, t.DepartureMinute))
            {
                t.Parking = TimeInMinutes(t.DepartureHour, t.DepartureMinute) - TimeInMinutes(t.ArrivalHour, t.ArrivalMinute);
            }
            else
            {
                t.Parking = 1440 - TimeInMinutes(t.ArrivalHour, t.ArrivalMinute) +
                        TimeInMinutes(t.DepartureHour, t.DepartureMinute);
            }
            if (LastElement >= schedule.Length)
            {
                Resize();
            }
            schedule[LastElement] = t;
            LastElement++;
        }

        public void FastAdd(string disp,string dist,int aH,int aM,int dH,int dM)
        {
            var t = new Train();
            t.Number = LastElement + 1;
            t.Dispatch = disp;
            t.Destination = dist;
            t.ArrivalHour = aH;
            t.ArrivalMinute = aM;

            t.ArrivalHour = iSSixtySeconds(t.ArrivalMinute, t.ArrivalHour);

            t.DepartureHour = dH;
            t.DepartureMinute = dM;
            t.DepartureHour = iSSixtySeconds(t.DepartureMinute, t.DepartureHour);
            
            if (TimeInMinutes(t.ArrivalHour, t.ArrivalMinute) < TimeInMinutes(t.DepartureHour, t.DepartureMinute))
            {
                t.Parking = TimeInMinutes(t.DepartureHour, t.DepartureMinute) - TimeInMinutes(t.ArrivalHour, t.ArrivalMinute);
            }
            else
            {
                t.Parking = 1440 - TimeInMinutes(t.ArrivalHour, t.ArrivalMinute) +
                            TimeInMinutes(t.DepartureHour, t.DepartureMinute);
            }
            
            if (LastElement >= schedule.Length)
            {
                Resize();
            }
            schedule[LastElement] = t;
            LastElement++;
        }


        public void Available()
        {
            bool success = false;
            Console.WriteLine("Enter hours");
            int h = HoursValidation();
            Console.WriteLine("Enter minutes");
            int m = MinutesValidation();
            if (LastElement != 0)
            {
                for (int i = 0; i <= LastElement; i++)
                {
                    int ArriveTimeInMinutes = TimeInMinutes(schedule[i].ArrivalHour, schedule[i].ArrivalMinute);
                    int DepartureTimeInMinutes = TimeInMinutes(schedule[i].DepartureHour, schedule[i].DepartureMinute);
                    int UserTimeInMinutes = TimeInMinutes(h, m);
                    if (ArriveTimeInMinutes < DepartureTimeInMinutes)
                    {
                        if (ArriveTimeInMinutes <= UserTimeInMinutes && UserTimeInMinutes <= DepartureTimeInMinutes)
                        {
                            Print(schedule[i]);
                            success = true;
                        }
                    }
                    else
                    {
                        if (UserTimeInMinutes <= DepartureTimeInMinutes  && UserTimeInMinutes <= ArriveTimeInMinutes)
                        {
                            Print(schedule[i]);    
                            success = true;
                        }
                    }
                }

                if (!success)
                {
                    Console.WriteLine("In that time, station is empty");
                }
            }
        }

        public void Nearest()
        {
            bool success = false;
            Console.WriteLine("Enter hours");
            int h = HoursValidation();
            Console.WriteLine("Enter minutes");
            int m = MinutesValidation();
            Console.WriteLine("Enter destination");
            string destination = StringValidation();
            var TrainWithThatDistination =
                schedule.Select(x=>x).Where(x => x.Destination == destination).ToArray();
            if (TrainWithThatDistination.Length != 0)
            {
                TrainWithThatDistination =
                    TrainWithThatDistination.OrderBy(x => x.DepartureHour).ThenBy(x => x.DepartureMinute).ToArray();
                for (int i = 0; i < TrainWithThatDistination.Length; i++)
                {
                    int ArriveTimeInMinutes = TimeInMinutes(TrainWithThatDistination[i].ArrivalHour, TrainWithThatDistination[i].ArrivalMinute);
                    int DepartureTimeInMinutes = TimeInMinutes(TrainWithThatDistination[i].DepartureHour, TrainWithThatDistination[i].DepartureMinute);
                    int UserTimeInMinutes = TimeInMinutes(h, m);
                    if (ArriveTimeInMinutes < DepartureTimeInMinutes)
                    {
                        if (UserTimeInMinutes <= DepartureTimeInMinutes)
                        {
                            Print(TrainWithThatDistination[i]);
                            success = true;
                            break;
                        }
                    }
                    else
                    {
                        if (UserTimeInMinutes >= DepartureTimeInMinutes)
                        {
                            Print(TrainWithThatDistination[i]);    
                            success = true;
                            break;
                        }
                    }
                }

                if (!success)
                {
                    Console.WriteLine("In that time, station is empty");
                }
            }
            else
            {
                Console.WriteLine($"There is no {destination} destination");
            }
        }
        
        
        int TimeInMinutes(int h, int m) => (h * 60) + m;
        public void Print(Train t)
        {
            Console.WriteLine();
            Console.WriteLine("№{0} : {1} - {2}   {3} - {4} || {5}", t.Number,t.ArrivalHour +":"+ t.ArrivalMinute,t.DepartureHour +":"+ t.DepartureMinute,t.Dispatch, t.Destination,t.Parking);
            Console.WriteLine();
        }

        public void PrintAll()
        {
            foreach (var t in schedule.OrderBy(x=>x.Parking))
            {
                if (t.Number != 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("№{0} : {1} - {2}   {3} - {4} || {5}", t.Number,t.ArrivalHour +":"+ t.ArrivalMinute,t.DepartureHour +":"+ t.DepartureMinute,t.Dispatch, t.Destination,t.Parking);
                    Console.WriteLine();
                }
            }
        }
        private string StringValidation()
        {
            string input = Console.ReadLine();
            while (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input can`t be empty");
                input = Console.ReadLine();
            }

            return input;
        }

        private int IntValidation()
        {
            string input = StringValidation();
            while (!int.TryParse(input,out _))
            {
                Console.WriteLine("Input must be only on integers");
                input = StringValidation();
            }
            return int.Parse(input);
        }

        private int HoursValidation()
        {
            int input = IntValidation();
            while (input < 0 && input > 24)
            {
                Console.WriteLine("Hours can`t be negative or more than 24");
                input = IntValidation();
            }

            return input == 24 ? 0 : input;
        }private int MinutesValidation()
        {
            int input = IntValidation();
            while (input < 0 && input > 60)
            {
                Console.WriteLine("Hours can`t be negative or more than 60");
                input = IntValidation();
            }

            return input == 60 ? 0 : input;
        }

        private int iSSixtySeconds(int m,int h) => m == 60 ? h + 1 : h;
    }
}