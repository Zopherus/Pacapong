using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    class Score : IComparable<Score>
    {
        private string name;
        private int points;

        public Score(int points)
        {
            this.points = points;
        }

        public Score(string name, int points)
        {
            this.name = name;
            this.points = points;
        }

        public string Name
        {
            get { return name; }
        }

        public int Points
        {
            get { return points; }
        }

        public override string ToString()
        {
            return name + ": " + points.ToString();
        }

        public int CompareTo(Score other)
        {
            if (other == null)
                return 1;
            return other.points.CompareTo(this.points);
        }
    }
}
