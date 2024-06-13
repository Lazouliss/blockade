using UnityEngine;
using System;
using System.Collections.Generic;

namespace blockade.AI.Structure
{
    /// <summary>
    /// Represents a square of the grid. If we see the grid as a graph, this would be a node.
    /// By Gabin Maury
    /// </summary>
    public class Square //temporarily public, to be changed to internal
    {
        public int position;
        public bool hasPawn;


        /// <summary>
        /// Bitflag representing where there are walls around this square. Best used in binary!
        /// This value is used as arc representation for our graph (0 means there is an arc there, 1 means there is no arc)
        /// 
        /// Bits are in clockwise order: up, right, down, left
        /// Examples:
        ///     - 0b1000 (8) = wall above
        ///     - 0b0110 (6) = wall to the right, wall below
        ///     - 0b1111 (15) = completely surronded by walls
        ///     - 0b0000 (0) = no walls around the square (default value)
        ///     
        /// How to use:
        ///     - (surroundedBy & WALL_RIGHT) == 0          => is there no wall on the right
        ///     - (surroundedBy & WALL_RIGHT) == WALL_RIGHT => is there a wall on the right
        ///     - surroundedBy |= WALL_RIGHT => add a wall to the right (bitwise OR, if we just use + it will make errors if we have for example 0b1100 + 0b0110)
        ///  
        /// </summary>
        public int surroundedBy;
        public const int WALL_ABOVE = 0b1000;
        public const int WALL_RIGHT = 0b0100;
        public const int WALL_BELOW = 0b0010;
        public const int WALL_LEFT = 0b0001;

        public Square(Square other)
        {
            this.position = other.position;
            this.hasPawn = other.hasPawn;
            this.surroundedBy = other.surroundedBy;
        }

        public Square(int position, bool hasPawn)
        {
            this.position = position;
            this.hasPawn = hasPawn;
        }

        public bool hasWall(int wall_bitflag)
        {
            return (surroundedBy & wall_bitflag) == wall_bitflag;
        }

        public void addWall(int wall_bitflag)
        {
            surroundedBy |= wall_bitflag;
        }

        public bool hasWallAbove() { return hasWall(WALL_ABOVE); }
        public bool hasWallBelow() { return hasWall(WALL_BELOW); }
        public bool hasWallRight() { return hasWall(WALL_RIGHT); }
        public bool hasWallLeft()  { return hasWall(WALL_LEFT); }

        public void addWallAbove() { addWall(WALL_ABOVE); }
        public void addWallBelow() { addWall(WALL_BELOW); }
        public void addWallRight() { addWall(WALL_RIGHT); }
        public void addWallLeft()  { addWall(WALL_LEFT);  }




        public List<int> get_neighbors()
        {
            List<int> neighbors = new List<int>();
            //right neighbor
            if (position % 11 != 10 && !hasWallRight())
            {
                neighbors.Add(position + 1);
            }

            //left neighbor
            if (position % 11 != 0 && !hasWallLeft())
            {
                neighbors.Add(position - 1);
            }

            //top neighbor
            int topPosition = position + 11;
            if (topPosition < 11 * 14 && !hasWallAbove())
            {
                neighbors.Add(topPosition);
            }

            //bottom neighbor
            int bottomPosition = position - 11;
            if (bottomPosition >= 0 && !hasWallBelow())
            {
                neighbors.Add(bottomPosition);
            }

            return neighbors;
        }


    }
}
