using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace ShootingGallery
{
    public class ShootingRanche
    {
        private Queue<Shooter> FirstInstructorsQueue; //= new Queue<Shooter>();
        private Queue<Shooter> SecondInstructorsQueue;
        private List<Shooter> Completed = new List<Shooter>();
        private Queue<Shooter> queue;

        public ShootingRanche(Queue<Shooter> queue)
        {
            this.queue = queue;
        }

        public void Start()
        {
            var thread = new Thread(new ThreadStart(() => FirstInstructor("1", 1)));
            thread.Start();
            Thread.Sleep(1000);
            thread = new Thread(new ThreadStart(() => SecondInstructor("2", 4)));
            thread.Start();
            Thread.Sleep(10000);
            while (FirstInstructorsQueue.Count != 0 && SecondInstructorsQueue.Count != 0)
            {
                Thread.Sleep(1);
            }
        }

        #region Generator

        private void FirstInstructor(string name, int startLine)
        {
            if (queue.Count > 0)
            {
                Shooter[] firstGroup = new Shooter[3];
                int count = 0;
                if (queue.Count < firstGroup.Length)
                {
                    count = queue.Count;
                }
                else
                {
                    count = 3;
                }

                for (int i = 0; i < count; i++)
                {
                    firstGroup[i] = queue.Dequeue();
                    firstGroup[i].TargetLine = Convert.ToString($"{i + startLine}");
                }

                FirstInstructorsQueue = new Queue<Shooter>();
                foreach (var target in firstGroup)
                {
                    FirstInstructorsQueue.Enqueue(target);
                }

                Console.WriteLine($"instr`s {name} line is ready");
                ShootingFirstInstr(name);
            }
        }

        private void SecondInstructor(string name, int startLine)
        {
            if (queue.Count > 0)
            {
                Shooter[] secondGroup = new Shooter[3];
                int count = 0;
                if (queue.Count < secondGroup.Length)
                {
                    count = queue.Count;
                }
                else
                {
                    count = 3;
                }

                for (int i = 0; i < count; i++)
                {
                    secondGroup[i] = queue.Dequeue();
                    secondGroup[i].TargetLine = Convert.ToString($"{i + startLine}");
                }

                SecondInstructorsQueue = new Queue<Shooter>();
                foreach (var target in secondGroup)
                {
                    SecondInstructorsQueue.Enqueue(target);
                }

                Console.WriteLine($"instr`s {name} line is ready");
                ShootingSecondInstr(name);
            }
        }

        #endregion

        #region Shooting

        private void ShootingFirstInstr(string name)
        {
            string instrName = name;
            while (FirstInstructorsQueue.Count > 0)
            {
                var targetShooter = FirstInstructorsQueue.Dequeue();
                if (targetShooter.ShootCount > 0)
                {
                    targetShooter = ShootLine(targetShooter, instrName);
                }
                if (targetShooter.ShootCount > 0)
                    queue.Enqueue(targetShooter);
                else
                    Completed.Add(targetShooter);
                if (queue.Count > 0)
                {
                    var nextShooterOnThisLine = queue.Dequeue();
                    nextShooterOnThisLine.TargetLine = targetShooter.TargetLine;
                    FirstInstructorsQueue.Enqueue(nextShooterOnThisLine);
                }
            }
        }

        private void ShootingSecondInstr(string name)
        {
            string instrName = name;
            while (SecondInstructorsQueue.Count > 0)
            {
                var targetShooter = SecondInstructorsQueue.Dequeue();
                if (targetShooter.ShootCount > 0)
                {
                    targetShooter = ShootLine(targetShooter, instrName);
                }
                if (targetShooter.ShootCount > 0)
                    queue.Enqueue(targetShooter);
                else
                    Completed.Add(targetShooter);
                if (queue.Count > 0)
                {
                    var nextShooterOnThisLine = queue.Dequeue();
                    nextShooterOnThisLine.TargetLine = targetShooter.TargetLine;
                    SecondInstructorsQueue.Enqueue(nextShooterOnThisLine);
                }
            }
        }
        
        private Shooter ShootLine(Shooter targetShooter, string instrName)
        {
            var logic = new Logic();

            Thread.Sleep(logic.InitialRnd() * 10);
            Console.WriteLine(logic.LineForPrint(targetShooter, instrName, Logic.Initial));
            Thread.Sleep(logic.PrepareForShootRnd() * 10);
            Console.WriteLine(logic.LineForPrint(targetShooter, instrName, Logic.PrepareForShoot));
            Thread.Sleep(logic.ReadyForShootRnd() * 10);
            Console.WriteLine(logic.LineForPrint(targetShooter, instrName, Logic.ReadyForShoot));
            Thread.Sleep(logic.ShootRnd() * 10);
            Console.WriteLine(logic.LineForPrint(targetShooter, instrName, Logic.Shoot));
            Thread.Sleep(logic.EndRnd() * 10);
            Console.WriteLine(logic.LineForPrint(targetShooter, instrName, Logic.End));
            targetShooter.ShootCount--;
            return targetShooter;
        }
        #endregion

        #region Logic
        private class Logic
        {
            public const string Initial = "Занял направление!";
            public const string PrepareForShoot = "Подготовиться к стрельбе!";
            public const string ReadyForShoot = "К стрельбе готов!";
            public const string Shoot = "Произвести стрельбу!";
            public const string End = "Стрельбу окончил!";
            public Random rnd = new Random();
            public Program Program = new Program();
            public string LineForPrint(Shooter targetShooter, string instrName, string action) 
            {
                return $"Направление {targetShooter.TargetLine}, инструктор {instrName},стрелок {targetShooter.Name}:{action}";
            }

            public int InitialRnd()
            {
                int time = rnd.Next(3, 10);
                Program.Finaltime += time;
                return 1;
            }

            public int PrepareForShootRnd()
            {
                int time = rnd.Next(2, 16);
                Program.Finaltime += time;
                Program.ShootingTime += time;
                return 1;
            }

            public int ReadyForShootRnd()
            {
                int time = rnd.Next(1, 4);
                Program.Finaltime += time;
                Program.ShootingTime += time;
                return 1;
            }

            public int ShootRnd()
            {
                int time = rnd.Next(1, 2);
                Program.Finaltime += time;
                Program.ShootingTime += time;
                return 1;
            }

            public int EndRnd()
            {
                int time = rnd.Next(5, 15);
                Program.Finaltime += time;
                Program.ShootingTime += time;
                return 1;
            }
        }
        #endregion
    }
}