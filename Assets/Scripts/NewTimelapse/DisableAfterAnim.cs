using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterAnim : MonoBehaviour
{
    private void Update()
    {
        if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length && this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("border_Disappear"))
            gameObject.SetActive(false);
    }
}
