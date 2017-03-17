using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    private float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    private GameManager gmInstance;

    void Start()
    {
        gmInstance = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    bool ShouldIRun ()
    {
        if (gmInstance.ShouldBuild)
        {
            Camera.main.orthographicSize = 1.5f;
            return true;
        }
        else return false;
    }

    void Update()
    {
        if (ShouldIRun())
        {
            if (target)
                FollowPlayer();
            else
                target = GameObject.FindWithTag("Player").transform;
        }
    }
    
    void FollowPlayer()
    {
        target = GameObject.FindWithTag("Player").transform;
        Vector3 goalPos = target.position;
        goalPos.z = -10;
        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, dampTime);//slows down the camera for smooth movement
    }
}
