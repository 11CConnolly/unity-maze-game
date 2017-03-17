using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour {
    
    private GameManager gmInstance;

    public MazeCell playerCell;
    
    public static int ShownPaths = 0;
    private float MoveSpeed = 2f;
    private bool MadePath;

    void Start()
    {
        MadePath = false; //So only one path can be made per level, not wasting shown paths
        gmInstance = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update ()
    {
        if (Input.GetKey(KeyCode.UpArrow)) //Moves player up
        {
            transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow)) //Moves player down
        {
            transform.Translate(Vector3.down * MoveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) //Moves player left
        {
            transform.Translate(Vector3.left * MoveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow)) //Moves player right
        {
            transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space)) //Shows Path to the end
        {
            ShowRoute();
        }
        if (Input.GetKeyDown(KeyCode.P)) //With help from Unity freezes game time
        {
            if (Time.timeScale == 1)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }

    void ShowRoute() 
    {
        if (ShownPaths < 2 || MadePath) //
        {
            ShownPaths++;
            gmInstance.StartMakeRoute(playerCell);
        }
    }
}
