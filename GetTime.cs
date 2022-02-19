using System;

namespace ShootingGallery
{
    public class GetTime
    {
        public static void Clock()
        {
            Console.Write(
                MakeNormalTime(Program.Finaltime, "Сумма затраченного времени: ") + 
                MakeNormalTime(Program.ShootingTime, "Длительность стрельб: ")
            );
        }

        private static string MakeNormalTime(int time, string forWhat)
        {
            int min = 0;
            while (time>60)
            {
                time -= 60;
                min++;
            }
            return forWhat + $"минут:{min}, секунд:{time}. ";
        }
    }
}