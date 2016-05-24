using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    //Used for timing multiple things
    class Timer
    {
        //Time in milliseconds
        private int timeMilliseconds = 0;

        //The interval at which to do things
        private int interval;

        public Timer(int interval)
        {
            this.interval = interval;
        }

        public Timer(int timeMilliseconds, int interval)
        {
            this.timeMilliseconds = timeMilliseconds;
            this.interval = interval;
        }

        public int TimeMilliseconds
        {
            get { return timeMilliseconds; }
            set
            {
                if (value >= 0)
                    timeMilliseconds = value;
            }
        }

        public int Interval
        {
            get { return interval; }
            set
            {
                if (value >= 0)
                    interval = value;
            }
        }

        //Add the time in milliseconds since the last frame to the timer using the GameTime class
        public void tick(GameTime gameTime)
        {
            timeMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
        }

        //Resets the time to 0
        public void reset()
        {
            timeMilliseconds = 0;
        }
    }
}
