using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    public void SetClickable(bool isClickable);

    public Action GetOnClicked { get; set; }
}
