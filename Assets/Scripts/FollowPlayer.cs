using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 v3;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + v3;
        if (transform.position.y < -1) {
            transform.position = new Vector3(transform.position.x, -1, -10);
        }
    }
}
