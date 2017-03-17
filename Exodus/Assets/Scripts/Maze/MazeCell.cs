using UnityEngine;
using System.Collections;


public class MazeCell : MonoBehaviour {

    public Sprite reg;
    public Sprite gold;

    public IntVector2 coordinates;

	public bool visited;

    public MazeCell parentCell;

	public Walls walls = new Walls ();

    public SpriteRenderer Sr;

    public void ChangeSprite(string name)
    {
        Sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        if (name == "gold")
            Sr.sprite = gold;
        if (name == "reg")
            Sr.sprite = reg;
    }

}

public class Walls
    {   //boolean representation of four walls of each cell

    public bool n;
    public bool e;
    public bool s;
    public bool w;

    }