using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public Maze mazePrefab;
    public Player playerPrefab;

    public Maze mazeInstance;
    public GameObject playerInstance;

    public KeyFull Key1;
    public KeyFull Key2;
    public KeyFull Key3;
    public Text TimeLeftText;
    public Text KeysText;
    public Text ScoreText;
    public Player playerUse;

    public int keys;
    public bool ShouldBuild = false;//if the level should build a maze
    public bool Timed = false;
    public bool Scored = false;
    public float TimeLeft;
    public static float Score = 0;

    private LevelManager level;

    // Use this for initialization
    void Start()
    {
        LoadLevelSettings(SceneManager.GetActiveScene().buildIndex);
        if (ShouldBuild) //So I do not build on form levels
        {
            keys = 0; //reset the number of keys
            BuildMaze();
            PlacePlayer();
            PlaceKeyShards();
            level = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            TimeLeftText = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();//Get time text
            KeysText = GameObject.FindGameObjectWithTag("Keys").GetComponent<Text>(); // get key text
            Score += TimeLeft;//Added onto the score is the TimeLeft for a level
        }
        if (Scored) //If last level and I need to show the score
        {
            ScoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>(); //Get score textbox
            ScoreText.text = SignInScript.FirstName + " " + SignInScript.Surname + ", your score is : " + Mathf.Round(Score);
        }
    }

    void Update () //Called every frame
    {
        if (Timed)
            reduce();
    }

    void reduce() // every frame I reduce the TimeLeft
    {
        TimeLeft -= Time.deltaTime; //time.deltaTime is the time that has passed in that scene
        Score -= Time.deltaTime;
        TimeLeftText.text = "Time Left:" + Mathf.Round(TimeLeft);
        KeysText.text = "Keys: " + keys;
        if (TimeLeft < 0) //If the player runs out of time
        {
            level.LoadLevel("Game_Over_Scene");
            Player.ShownPaths = 0; //Reset shown paths
        }
    }

    public void LoadLevelSettings(int level)
    {
        switch (level)
        {
            case 3:
                mazePrefab.size = new IntVector2(10, 10); //build settings for level 1
                mazePrefab.randWallBreak = 0;
                ShouldBuild = true;
                Timed = true;
                TimeLeft = 60f;
                break;
            case 4:
                mazePrefab.size = new IntVector2(13, 11); //buld settings for level 2
                mazePrefab.randWallBreak = 0;
                ShouldBuild = true;
                Timed = true;
                TimeLeft = 75f;
                break;
            case 5:
                mazePrefab.size = new IntVector2(17, 13); //build settings for level 3
                mazePrefab.randWallBreak = 4;
                ShouldBuild = true;
                Timed = true;
                TimeLeft = 100f;
                break;
            case 6:
                mazePrefab.size = new IntVector2(22, 16); // build setitngs for level 4
                mazePrefab.randWallBreak = 3;
                ShouldBuild = true;
                Timed = true;
                TimeLeft = 120f;
                break;
            case 7:
                ShouldBuild = false; //build settings for Win Scene
                Scored = true;
                break;
            default:
                ShouldBuild = false; //Build setting for all other scenes
                break;

        }
    }

    private void BuildMaze()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.Generate();
        mazeInstance.StartMakeMaze();
        mazeInstance.FindWalls();
    }

    private void PlacePlayer()
    {
        playerInstance = Instantiate(Resources.Load("Player"), mazeInstance.StartCellCoords, Quaternion.identity) as GameObject;
        playerUse = playerInstance.GetComponent<Player>();
        playerUse.playerCell = mazeInstance.cells[0, 0];
    }

    private void PlaceKeyShards()
    {
        Key1 = Instantiate(Resources.Load("KeyFull"), MakeRandomCoords(), Quaternion.identity) as KeyFull;
        Key2 = Instantiate(Resources.Load("KeyFull"), MakeRandomCoords(), Quaternion.identity) as KeyFull;
        Key3 = Instantiate(Resources.Load("KeyFull"), MakeRandomCoords(), Quaternion.identity) as KeyFull;
    }

    private Vector3 vec;

    private Vector3 MakeRandomCoords()
    {
        vec = Vector3.zero;
        vec.x = Random.Range(1, mazePrefab.size.x);
        vec.y = Random.Range(1, mazePrefab.size.y);
        vec = mazeInstance.cells[(int)vec.x, (int)vec.y].transform.position;
        vec.z = -0.5f;
        return vec;
    }

    public void StartMakeRoute(MazeCell playerCell)
    {
        mazeInstance.FindPath(playerCell);
        foreach (MazeCell cell in mazeInstance.InstructionPath)
        {
            if (cell.tag != "Trap")
                cell.ChangeSprite("gold");
        }
    }
}
