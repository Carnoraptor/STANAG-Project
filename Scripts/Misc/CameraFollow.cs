using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code inspired from https://iqcode.com/code/csharp/camera-follow-player-unity-2d

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    public Transform target;

    void Awake()
    {
        //GetComponent<Camera>().orthographicSize = Screen.height / 2;
    }
    
    void FixedUpdate ()   
    {     
        //Old ideas that weren't smooth at all 
        //transform.position = new Vector3(player.position.x + offset.x, /*player.position.y*/ -2 + offset.y, offset.z)
        //transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}