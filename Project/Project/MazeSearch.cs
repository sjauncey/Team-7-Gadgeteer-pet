using System;
using Microsoft.SPOT;

namespace GadgeteerApp1
{
    enum CellType
    {
        Wall,
        Unvisited,
        Unfinished,
        Finished,
        Target
    }
    class MazeSearch
    {
        private Cell root;
        private Cell current;
        private System.Collections.Hashtable cells;
        private int facing;
        private Cell currentMove;
        private Program program;
        private Cell target;
        public MazeSearch(Program program)
        {
            this.program = program;
            root = new Cell(CellType.Unfinished, 0, 0);
            cells = new System.Collections.Hashtable();
            target = null;
        }
        private void insertCell(Cell cell)
        {
            cells.Add(cell.offX.ToString()+cell.offY.ToString(), cell);
        }
        private Cell getCell(int offx, int offy)
        {
            if(cells.Contains(offx.ToString() + offy + ToString())){
                return (Cell) cells[offx.ToString() + offy + ToString()];
            } else {
                Cell t = new Cell(CellType.Unvisited, current.offX, current.offY);
                cells.Add(offx.ToString() + offy + ToString(),t);
                return t;
            }
            
        }
        public void initalStep()
        {
            program.mode = Mode.Solver;
            initCell(root);
            root.Parent = null;
            current = root;
            facing = 0;
            Advance(); //We are facing north, so start exploring north;
            currentMove = root.North;
        }

        private void Advance()
        {
            program.addSPORK(new SPORK(Instruction.FORWARD,1));
        }
        public void nextStep(CellType c)
        {
            if (c == CellType.Wall)
            {
                //Undo currentMove
                if (current.North == currentMove)
                {
                    Rotate(2);
                    Advance();
                } else if (current.East == currentMove)
                {
                    Rotate(3);
                    Advance();
                } else if (current.South == currentMove)
                {
                    Rotate(0);
                    Advance();
                } else if (current.West == currentMove)
                {
                    Rotate(1);
                    Advance();
                }
                currentMove.celltype = CellType.Wall;
                //new instruction generated once returned and this function is called with false on old square
            }
            else if (c == CellType.Unfinished || c == CellType.Target)
            {
                    if (c == CellType.Target && target == null)
                {
                    target = currentMove;
                }

                currentMove.Parent = current; //Prev move successful, so we point the new cell's parent to the previous one
                current = currentMove; // We aren't on a wall, so the previous move was successful.
                initCell(current);
                if (current.North.celltype == CellType.Unvisited)
                {
                    Rotate(0);
                    Advance();
                    currentMove = current.North;
                }
                else if (current.East.celltype == CellType.Unvisited)
                {
                    Rotate(1);
                    Advance();
                    currentMove = current.East;
                } else if (current.South.celltype == CellType.Unvisited)
                {
                    Rotate(2);
                    Advance();
                    currentMove = current.South;
                } else if (current.West.celltype == CellType.Unvisited)
                {
                    Rotate(3);
                    Advance();
                    currentMove = current.West;
                } else if(current.Parent == null){
                    //All children have been visited, and our parent is null, so we must have finished!
                    //TODO: Finished actions
                    if (target == null)
                    {
                        //no solution found
                    }
                    else
                    {
                        program.mode = Mode.Directed;
                        target.distance = 0;
                        target.distanceSet = true;
                        calculateDistances(target);
                        //we are at the start, so we can now just repeatedly go down the smallest number to find fastest solution to the maze.
                    }
                }
                else if (current.Parent == current.North)
                {
                    current.celltype = CellType.Finished;
                    Rotate(0);
                    Advance();
                    currentMove = current.North;
                }
                else if (current.Parent == current.South)
                {
                    current.celltype = CellType.Finished;
                    Rotate(2);
                    Advance();
                    currentMove = current.South;
                }
                else if (current.Parent == current.West)
                {
                    current.celltype = CellType.Finished;
                    Rotate(3);
                    Advance();
                    currentMove = current.West;
                }
                else if (current.Parent == current.East)
                {
                    current.celltype = CellType.Finished;
                    Rotate(1);
                    Advance();
                    currentMove = current.East;
                }
            }
            
        }

        private void Rotate(int i)
        {
            if (i != facing)
            {
                if (((facing + 1) % 4) == i)
                {
                    program.addSPORK(new SPORK(Instruction.RIGHT,0));
                }
                else if (((facing + 2) % 4) == i)
                {
                    program.addSPORK(new SPORK(Instruction.RIGHT, 0));
                    program.addSPORK(new SPORK(Instruction.RIGHT, 0));
                } else if (((facing + 3) % 4) == i)
                {
                    program.addSPORK(new SPORK(Instruction.LEFT,0));
                }
                facing = i;
            }
        }
        private void shortestDistanceWalk()
        {
            while (current != target)
            {
                if (current.North.distanceSet && current.North.distance < current.distance)
                {
                    Rotate(0);
                    Advance();
                    current = current.North;
                } else if (current.East.distanceSet && current.East.distance < current.distance)
                {
                    Rotate(1);
                    Advance();
                    current = current.East;
                } else if (current.South.distanceSet && current.South.distance < current.distance)
                {
                    Rotate(2);
                    Advance();
                    current = current.South;
                }
                else if (current.West.distanceSet && current.West.distance < current.distance)
                {
                    Rotate(3);
                    Advance();
                    current = current.West;
                }
                else
                {
                    //We are soo stuck right now
                }
            }
            //we should now have issued the correct instructions to get to the end of the maze
        }

        //Recursivley calculates the distances to all surrounding cells.
        private void calculateDistances(Cell c)
        {
            updateCell(c.North, c.distance + 1);
            updateCell(c.East, c.distance + 1);
            updateCell(c.South, c.distance + 1);
            updateCell(c.West, c.distance + 1);
        }

        private void updateCell(Cell c, int d)
        {
            if (c.celltype != CellType.Wall)
            {
                if (!c.distanceSet || c.distance > d)
                {
                    c.distance = d;
                    c.distanceSet = true;
                    calculateDistances(c);
                }
            }
        }
        private void initCell(Cell current)
        {
            //Gets each cell from the hash table
            if (current.North == null)
            {
                current.North = getCell(current.offX, current.offY + 1);
            }
            if (current.South == null)
            {
                current.South = getCell(current.offX, current.offY - 1);
            }
            if (current.East == null)
            {
                current.East = getCell(current.offX + 1, current.offY);
            }
            if (current.West == null)
            {
                current.West = getCell(current.offX - 1, current.offY);
            }
        }


    }
}
