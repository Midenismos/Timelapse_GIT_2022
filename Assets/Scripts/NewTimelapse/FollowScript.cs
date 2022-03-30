using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public Transform toFollow;
    private Vector3 offset;

    void Start()
    {
        offset = toFollow.localPosition - transform.localPosition;
    }
    void Update()
    {
        transform.localPosition = toFollow.localPosition - offset;
    }
}
