[System.Serializable] //So Unity accepts and can use this custom struct
public struct IntVector2 {
	
	public int x, y;
	
	public IntVector2 (int x, int y) { //takes in two integer values
		this.x = x;//sets x
		this.y = y;//sets y
	}
}

