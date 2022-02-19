using System;
using System.Collections.Generic;

namespace ShootingGallery
{
    public class CreateGroup
    {
        public Queue<Shooter> HowMany(int value)
        {
            if (value<10)
            {
                throw new ArgumentOutOfRangeException();
            }
            List<Shooter> group = new List<Shooter>();
            for (int i = 1; i <= value; i++)
            {
                group.Add(new Shooter()
                {
                    Name = $"Shooter{i}"
                });
            }

            var queue = new Queue<Shooter>();
            foreach (var shooter in group)
            {
                queue.Enqueue(shooter);
            }
            return queue;
        }
    }
}