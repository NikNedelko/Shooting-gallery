using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace ShootingGallery
{
    class Program
    {
        public static int Finaltime =0;
        public static int ShootingTime =0;
        
        static void Main(string[] args)
        {
            var group = new CreateGroup().HowMany(13);
            new ShootingRanche(group).Start();
            GetTime.Clock();
        }
    }
}