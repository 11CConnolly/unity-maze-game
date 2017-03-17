using UnityEngine;
using System.Collections;

public class MazeCellEdge : MonoBehaviour { //assigns a wall to a cell with appropriate rotation

	public MazeDirection direction;

	public void Initialize (MazeCell cell, MazeCell otherCell, MazeDirection direction) {

		this.direction = direction;
		this.gameObject.name = "Wall of " + cell.ToString() + " " + direction.ToString ();
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation ();
	}
}
