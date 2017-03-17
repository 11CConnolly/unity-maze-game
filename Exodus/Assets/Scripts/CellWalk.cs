using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CellWalk : MonoBehaviour {

    private GameManager gmInstance;
    private LevelManager lmInstance;

    void Start()
    {
        gmInstance = GameObject.Find("GameManager").GetComponent<GameManager>();
        lmInstance = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gmInstance.playerUse.playerCell = CheckAdjCells(gmInstance.playerUse.playerCell);//Find the player's current cell by checking the cells adjacent to the current cell
        }
        if (transform.position == gmInstance.mazeInstance.EndCellCoords && other.tag == "Player")//if player is on top right corner cell
        {
            if (gmInstance.keys == 3) //if the level is completed
            {
                lmInstance.LoadNextLevel();
            }
        }
    }

    MazeCell CheckAdjCells (MazeCell checkCell)
    {
        //up
        if (checkCell.coordinates.y != gmInstance.mazeInstance.size.y - 1)
        {
            if (this.transform.position == gmInstance.mazeInstance.cells[checkCell.coordinates.x, checkCell.coordinates.y + 1].transform.position)
            {
                return gmInstance.mazeInstance.cells[checkCell.coordinates.x, checkCell.coordinates.y + 1];
            }
        }
        //right
        if (checkCell.coordinates.x != gmInstance.mazeInstance.size.x - 1)
        {
            if (this.transform.position == gmInstance.mazeInstance.cells[checkCell.coordinates.x + 1, checkCell.coordinates.y].transform.position)
            {
                return gmInstance.mazeInstance.cells[checkCell.coordinates.x + 1, checkCell.coordinates.y];
            }
        }
        //down
        if (checkCell.coordinates.y != 0)
        {
            if (this.transform.position == gmInstance.mazeInstance.cells[checkCell.coordinates.x, checkCell.coordinates.y - 1].transform.position)
            {
                return gmInstance.mazeInstance.cells[checkCell.coordinates.x, checkCell.coordinates.y - 1];
            }
        }
        //left
        if (checkCell.coordinates.x != 0)
        {
            if (this.transform.position == gmInstance.mazeInstance.cells[checkCell.coordinates.x - 1, checkCell.coordinates.y].transform.position)
            {
                return gmInstance.mazeInstance.cells[checkCell.coordinates.x - 1, checkCell.coordinates.y];
            }
        }
        //check diagonals
        for (int i = -1; i < 1; i += 2)
        {
            for (int j = -1; j < 1; j += 2)
            {
                if (this.transform.position == gmInstance.mazeInstance.cells[checkCell.coordinates.x + i, checkCell.coordinates.y + j].transform.position)
                {
                    return gmInstance.mazeInstance.cells[checkCell.coordinates.x + i, checkCell.coordinates.y + j];
                }
            }
        }
        //else return cell to reduce errors
        return gmInstance.mazeInstance.cells[checkCell.coordinates.x, checkCell.coordinates.y];
    }
}