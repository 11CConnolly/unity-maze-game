using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    private static MusicPlayer instance = null;

	// Use this for initialization
	void Start () {
        if (instance != null)//If a music player exists 
        {
            Destroy(gameObject);//destory this one
        } else    {
            instance = this;//if no such music player exists, say it does
            GameObject.DontDestroyOnLoad(gameObject);//and dont destory this one
        }
	}
}
