using System;
using Microsoft.SPOT;

namespace GadgeteerApp1
{
    class Cell
    {
        public Cell North { get; set; }
        public Cell East { get; set; }
        public Cell South { get; set; }
        public Cell West { get; set; }
        public CellType celltype { get; set; }
        public Cell Parent{ get; set; }
        public int offX { get; private set; }
        public int offY { get; private set; }
        public int distance { get; set; }
        public Boolean distanceSet { get; set; }

        public Cell(CellType ct, int offX, int offY)
        {
            this.offX = offX;
            this.offY = offY;
            this.celltype = ct;
        }
        public Cell(CellType ct, int offX, int offY, Cell North, Cell East, Cell South, Cell West)
        {
            this.offX = offX;
            this.offY = offY;
            this.celltype = ct;
            this.North = North;
            this.East = East;
            this.South = South;
            this.West = West;
        }
    }
}
