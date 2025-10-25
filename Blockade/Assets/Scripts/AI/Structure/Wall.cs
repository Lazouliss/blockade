using blockade.Blockade_common;

namespace blockade.AI.Structure
{
    /// <summary>
    /// The class repesents a wall.
    /// It is used to easily go from X/Y coordinates to grid position (conversion is automatic)
    /// The orientation is handeled automatically too.
    /// By Gabin Maury
    /// </summary>
    public class Wall //temporarily public, to be changed to internal
    {
        public enum Orientation
        {
            UP,
            LEFT,
            RIGHT,
            DOWN,
            UNPLACED,
        }

        public Orientation orientation;

        public int position1; //pos 1 is the square to the left if the wall is horizontal, the square at the bottom if the wall is vertical
        public int position2; //pos 2 is the square to the right if the wall is horizontal, the square at the top if the wall is vertical

        public int pos1X;
        public int pos1Y;

        public int pos2X;
        public int pos2Y;

        public Wall(Wall other)
        {
            this.orientation = other.orientation;
            this.position1 = other.position1;
            this.pos1X = other.pos1X;
            this.pos1Y = other.pos1Y;

            this.pos2X = other.pos2X;
            this.pos2Y = other.pos2Y;
        }

        public Wall(Orientation orientation, int pos1, int pos2)
        {
            this.orientation = orientation;
            this.position1 = pos1;
            this.position2 = pos2;
            this.pos1X = pos1 % Common.MAP_WIDTH;
            this.pos1Y = (int)(pos1 / Common.MAP_WIDTH);

            this.pos2X = pos2 % Common.MAP_WIDTH;
            this.pos2Y = (int)(pos2 / Common.MAP_WIDTH);

        }

        public Wall(Orientation orientation, BoardPos bpos1, BoardPos bpos2)
        {
            int pos1 = bpos1.position;
            int pos2 = bpos2.position;
            this.orientation = orientation;
            this.position1 = pos1;
            this.position2 = pos2;
            this.pos1X = pos1 % Common.MAP_WIDTH;
            this.pos1Y = (int)(pos1 / Common.MAP_WIDTH);

            this.pos2X = pos2 % Common.MAP_WIDTH;
            this.pos2Y = (int)(pos2 / Common.MAP_WIDTH);
        }

            public Wall(Orientation orientation, int pos1) //pos2 can be infered from pos1 and orientation. We consider pos1 is always left or bottom.
            {
            if (pos1 == -1) //empty wall
            {
                this.position1 = -1;
                this.position2 = -1;
                this.pos1X = -1;
                this.pos1Y = -1;
                this.pos2X = -1;
                this.pos2Y = -1;
            }
            else
            {

                int pos2 = 0;

                //no checks, we assume that the wall will be valid
                switch (orientation)
                {
                    case Orientation.UP:
                    case Orientation.DOWN:
                        pos2 = pos1 + 1;
                        break;
                    case Orientation.LEFT:
                    case Orientation.RIGHT:
                        pos2 = pos1 + Common.MAP_WIDTH;
                        break;
                }


                this.orientation = orientation;
                this.position1 = pos1;
                this.position2 = pos2;
                this.pos1X = pos1 % Common.MAP_WIDTH;
                this.pos1Y = (int)(pos1 / Common.MAP_WIDTH);

                this.pos2X = pos2 % Common.MAP_WIDTH;
                this.pos2Y = (int)(pos2 / Common.MAP_WIDTH);
            }

        }

        public static bool operator ==(Wall a, Wall b)
        {
            //boilerplate to check existance of the objects
            if (object.ReferenceEquals(a, b)) { return true; }
            if (((object)a == null) || ((object)b == null)) { return false; }

            if(a.orientation == b.orientation)
            {
                return (a.position1 == b.position1) && (a.position2 == b.position2);
            }

            if((a.orientation == Orientation.UP || a.orientation == Orientation.DOWN) && (b.orientation == Orientation.UP || b.orientation == Orientation.DOWN)) // if one up and one down
            {
                if(a.orientation == Orientation.UP) //if a is up, b is down becuase we already eliminated a == b 
                {
                    return (a.position1 + Common.MAP_WIDTH == b.position1) && (a.position2 + Common.MAP_WIDTH == b.position2);
                }
                else // a is down and b is up
                {
                    return (a.position1 - Common.MAP_WIDTH == b.position1) && (a.position2 - Common.MAP_WIDTH == b.position2);
                }
            }
            else if ((a.orientation == Orientation.RIGHT || a.orientation == Orientation.LEFT) && (b.orientation == Orientation.RIGHT || b.orientation == Orientation.LEFT)) // if one left and one right
            {
                if (a.orientation == Orientation.RIGHT) //if a is right, b is left becuase we already eliminated a == b 
                {
                    return (a.position1 + 1 == b.position1) && (a.position2 + 1 == b.position2);
                }
                else // a is left and b is right
                {
                    return (a.position1 - 1 == b.position1) && (a.position2 - 1 == b.position2);
                }
            }

            return false;
        }

        public static bool operator !=(Wall a, Wall b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Wall other = (Wall)obj;
            // You can adjust this comparison to suit your needs
            return position1 == other.position1 && position2 == other.position2;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            // Compute hash for orientation
            switch (orientation)
            {
                case Orientation.UP:
                    hash |= 0b0001;
                    break;
                case Orientation.RIGHT:
                    hash |= 0b0010;
                    break;
                case Orientation.DOWN:
                    hash |= 0b0100;
                    break;
                case Orientation.LEFT:
                    hash |= 0b1000;
                    break;
            }
            hash |= (position1 & 0xFF) << 4;
            hash |= (position2 & 0xFF) << 12;

            return hash;
        }


    }
}
