using UnityEngine;

public enum MazeDirection {
	North,
	East,
	South,
	West,
	none
}

public static class MazeDirections {
	
	public const int Count = 4;

	private static Quaternion[] rotations = {
		Quaternion.Euler (0f, 0f, 270f),
		Quaternion.Euler (0f, 0f, 180f),
		Quaternion.Euler (0f, 0f, 90f),
		Quaternion.identity,
	};
	
	public static Quaternion  ToRotation (this MazeDirection direction) {
		return rotations[(int)direction];
	}


}