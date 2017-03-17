using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public static int CurrentLevel = 0;
    public static int maxLevel = 7;

    public void LoadNextLevel ()
    {
        if (CurrentLevel < maxLevel)
        {
            CurrentLevel++;            
            SceneManager.LoadScene(CurrentLevel);//Uses unity's built in function to load the next levels
        }
    }
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
        CurrentLevel = 3;
    }
    public void Quit ()
    {
        Application.Quit();
    }

}
