using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour
{
    [HideInInspector]
    public Light GreenLight;
    [HideInInspector]
    public Light RedLight;

    [Header("Placer ici les Waypoints du rail associé à ca réacteur")]
    public GameObject[] WayPoints = new GameObject[5];

    public int curWayPointNumber = 3;

    public void Start()
    {
        Light[] lights = this.GetComponentsInChildren<Light>();
        foreach (Light light in lights)
        {
            if (light.color == Color.green)
            {
                GreenLight = light;
            }
            else
            {
                RedLight = light;
            }
        }
    }
}
