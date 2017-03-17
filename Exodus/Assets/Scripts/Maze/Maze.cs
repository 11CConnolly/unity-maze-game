using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

    //Get the cell and wall prefabs so they look nice
    public MazeCell cellPrefab;
    public MazeWall wallPrefab;
    
    public IntVector2 size; //Custom struct which defines maze size
	public int randWallBreak; //Defines how many walls get randomly destoyed
	public MazeCell[,] cells; //2D Array of maze cells for easy referencing

    public void Generate () // Generates grid of cells necessary for algorithm
    {
		cells = new MazeCell[size.x, size.y];
		for (int x = 0; x < size.x; x++)
        {
			for (int y = 0; y < size.y; y++)
            {
				CreateCell(new IntVector2(x,y));
			}
		}
	}    	
	
	private MazeCell GetCell (IntVector2 coordinates) { //Gets the cell array position for a particular intvector2 position
		return cells[coordinates.x, coordinates.y];
    }

    private MazeDirection directionMake = new MazeDirection(); //New Direction to use in the following script

    private void CreateCell (IntVector2 coordinates) { //Creates cell and places in appropraite location
		MazeCell newCell = Instantiate (cellPrefab) as MazeCell;		
		MazeCell neighbour = GetCell (coordinates);
		cells[coordinates.x, coordinates.y] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.y;
		newCell.visited = false; //Mark all cells as false to begin with
		newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, coordinates.y - size.y * 0.5f + 0.5f, 0f);
		directionMake = MazeDirection.North; //Goes through north, east, south and west cell sides
		newCell.walls.n = true; //sets the bool to true for that side
		CreateWall(newCell, neighbour, directionMake);//and creates a wall in that direction in the cell
		directionMake = MazeDirection.East;
		newCell.walls.e = true;
		CreateWall(newCell, neighbour, directionMake);
		directionMake = MazeDirection.South;
		newCell.walls.s = true;
		CreateWall(newCell, neighbour, directionMake);
		directionMake = MazeDirection.West;
		newCell.walls.w = true;
		CreateWall(newCell, neighbour, directionMake);
	}

    

    private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate (wallPrefab) as MazeWall;
		wall.Initialize (cell, otherCell, direction);
	}

    public Vector3 StartCellCoords = new Vector3();
    public Vector3 EndCellCoords = new Vector3();

    private Stack<MazeCell> stackDFS = new Stack<MazeCell>();//A Stack used for my Depth First Generation
    private List<MazeCell> options = new List<MazeCell>(); // list of possible cells to go to next

    public void StartMakeMaze () {
		MazeCell OriginCell = cells [0, 0];
        StartCellCoords = OriginCell.transform.position;
        StartCellCoords.z = StartCellCoords.z - 0.5f;
        EndCellCoords = cells[size.x - 1, size.y - 1].transform.position;
        stackDFS.Push (OriginCell);
		GenerateMazeDFS (OriginCell, MazeDirection.none);
	}


	private void GenerateMazeDFS(MazeCell currentCell, MazeDirection direction) {
	if (stackDFS.Count > 0) {
            if (!currentCell.visited)   {
                currentCell.visited = true; // mark the current cell so it won't be visited again
            }
            // sets the appropriate maze wall bool to false			
			if (direction == MazeDirection.North)
				currentCell.walls.n = false;
			if (direction == MazeDirection.East)
				currentCell.walls.e = false;
			if (direction == MazeDirection.South)
				currentCell.walls.s = false;
			if (direction == MazeDirection.West)
				currentCell.walls.w = false;
			options = new List<MazeCell> (); // clear the options list for new options for each new cell
			GetOptions (currentCell); // adds cells to options list for current cell
			if (options.Count == 0) { // if there are no more cells to visit
                GenerateMazeDFS(stackDFS.Pop (), MazeDirection.none); // the functions calls itself and backtracks
			}
			while (options.Count > 0) {
				int index = Random.Range (0, options.Count); // pick a random cell in the options list
				MazeCell nextCell = options [index]; // and assings it to the next cell
		        
				if (currentCell.coordinates.y > nextCell.coordinates.y) { //if the current cell is above the next cell
					currentCell.walls.s = false; // clear the apprpriate bool representing the 
                    GenerateMazeDFS(nextCell, MazeDirection.North); // go to next cell, say you're coming from above (north)
				}
                else if (currentCell.coordinates.x < nextCell.coordinates.x)
                {
					currentCell.walls.e = false;
                    GenerateMazeDFS(nextCell, MazeDirection.West);
				}
                else if (currentCell.coordinates.y < nextCell.coordinates.y) {
					currentCell.walls.n = false;
                    GenerateMazeDFS(nextCell, MazeDirection.South);
				} else if (currentCell.coordinates.x > nextCell.coordinates.x) {
					currentCell.walls.w = false;
                    GenerateMazeDFS(nextCell, MazeDirection.East);
				}

				GetOptions (currentCell);
			}			
			stackDFS.Push (currentCell); // knocks the current cell off and backtracks until stack is empty
		}
	}

    //Finds all the adjacent maze cells if they exist 
	public void GetOptions (MazeCell cell) {

		MazeCell otherCell = cells [cell.coordinates.x, cell.coordinates.y];
		if (cell.coordinates.x != size.x-1) {
			otherCell = cells [cell.coordinates.x+1, cell.coordinates.y];
			CheckOption (otherCell);
		}

		if (cell.coordinates.y != size.y-1) {
			otherCell = cells [cell.coordinates.x, cell.coordinates.y + 1];
			CheckOption (otherCell); 
		}

		if (cell.coordinates.x != 0) {
			otherCell = cells [cell.coordinates.x - 1, cell.coordinates.y];
			CheckOption (otherCell);
		}

		if (cell.coordinates.y != 0) {
			otherCell = cells [cell.coordinates.x, cell.coordinates.y - 1];
			CheckOption (otherCell);
		}
	}

    //If the cells haven't been visited yet, It adds them to the list of next possible options to visit
	public void CheckOption (MazeCell otherCell)
    {
		if (!otherCell.visited) 
			options.Add(otherCell);		
	}
    

	public void FindWalls ()
    {        
		foreach(MazeCell cell in cells)
        {
            BreakWalls(cell);
            cell.visited = false;
        }
	}

	private GameObject WallObject;

	public void BreakWalls (MazeCell cell) {
		if (!cell.walls.n || ((Random.Range(0, randWallBreak) == 1) && (cell.coordinates.y!=size.y-1))) {
			WallObject = (GameObject.Find ("Wall of " + cell.ToString () + " " + MazeDirection.North.ToString ()));
			Destroy (WallObject);
		}
		if (!cell.walls.e || ((Random.Range(0, randWallBreak) == 1) && (cell.coordinates.x!=size.x-1))) {
			WallObject = GameObject.Find("Wall of " + cell.ToString() + " " + MazeDirection.East.ToString());
			Destroy(WallObject);
		} 
		if (!cell.walls.s || ((Random.Range(0, randWallBreak) == 1) && (cell.coordinates.y!=0))) {
			GameObject WallObject = GameObject.Find("Wall of " + cell.ToString() + " " + MazeDirection.South.ToString());
			Destroy(WallObject);
		}
		if (!cell.walls.w || ((Random.Range(0, randWallBreak) == 1) && (cell.coordinates.x!=0))) {
			GameObject WallObject = GameObject.Find("Wall of " + cell.ToString() + " " + MazeDirection.West.ToString());
			Destroy(WallObject);
		}
    }

    private List<MazeCell> path = new List<MazeCell>();
    public MazeCell[] InstructionPath;
    private void Backtrace(MazeCell start, MazeCell end)
    {
        if (start != end)
        {
            path.Add(start);
            Backtrace(start.parentCell, end);
        }
        InstructionPath = path.ToArray();
    }

    private Queue<MazeCell> cellQueue = new Queue<MazeCell>();
    public void FindPath(MazeCell playerC)
    {
        
        MazeCell playerCell = cells[playerC.coordinates.x, playerC.coordinates.y];
        //Push the start cell onto the queue
        cellQueue.Enqueue(playerCell);
        //While queue isn't empty so I can view all possible routes in the Maze/Tree
        bool f = false;
        //To exit the maze early if I need to
        while (cellQueue.Count > 0)
        {
            //Dequeue to check the cell
            MazeCell C = cellQueue.Dequeue();
            //Mark it so I do not visit it again
            C.visited = true;
            //If the current cell is the goal cell (Top Right) End 
            if ((C.coordinates.x == size.x - 1) && (C.coordinates.y == size.y - 1))
            {
                Backtrace(C, playerCell);
                f = true;
            }
            if (f) break; //exit
            //Find Cells that are adjcaent to the current cell
            if (C.coordinates.y != size.y - 1)
            {
                if ((!cells[C.coordinates.x, C.coordinates.y + 1].visited) && (!C.walls.n))
                {
                    MazeCell nextCell = cells[C.coordinates.x, C.coordinates.y + 1];
                        nextCell.parentCell = C; //set parent cell for backtracking
                    cellQueue.Enqueue(nextCell); //and enqueue to visit later
                }
            }
            if (C.coordinates.x != size.x - 1)
            {
                if ((!cells[C.coordinates.x + 1, C.coordinates.y].visited) && (!C.walls.e))
                {
                    MazeCell nextCell = cells[C.coordinates.x + 1, C.coordinates.y];
                        nextCell.parentCell = C;
                    cellQueue.Enqueue(nextCell);
                }
            }
            if (C.coordinates.y > 0)
            {
                if ((!cells[C.coordinates.x, C.coordinates.y - 1].visited) && (!C.walls.s))
                {
                    MazeCell nextCell = cells[C.coordinates.x, C.coordinates.y - 1];
                        nextCell.parentCell = C;
                    cellQueue.Enqueue(nextCell);
                }
            }
            if (C.coordinates.x > 0)
            {
                if ((!cells[C.coordinates.x - 1, C.coordinates.y].visited) && (!C.walls.w))
                {
                    MazeCell nextCell = cells[C.coordinates.x - 1, C.coordinates.y];
                        nextCell.parentCell = C;
                    cellQueue.Enqueue(nextCell);
                }
            }
        }
    }

    bool CheckCell(MazeCell check) //So I don't go over the same cell many times in a row
    {
        if (!check.visited)
        {
            check.visited = true;
            return true;
        }
        return false;
    }
}
