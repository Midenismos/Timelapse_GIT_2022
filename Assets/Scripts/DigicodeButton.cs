using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigicodeButton : Button
{
    // A changer pour déterminer son ordre dans la succession de bouton à appuyer
    [Header("A changer pour déterminer son ordre dans la succession de bouton à appuyer")]
    public int ButtonOrderNumber = 0;
    //[HideInInspector]
    public bool CheckedButton = false;
}
