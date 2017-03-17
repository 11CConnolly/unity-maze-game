using UnityEngine;
using System.Collections;

public class KeyFull : MonoBehaviour {

    private GameManager gmInstance;

    void Awake ()
    {
        gmInstance = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D (Collider2D other) //if key bumps into player, incremment keys and destory self
    {
        if (other.tag == "Player")
        {
            gmInstance.keys++;
            Destroy(this.gameObject); 
        }
    }

}
