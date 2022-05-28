using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebuleuseSoundManager : MonoBehaviour
{
    public AudioClip[] YellowNebuleuseSound;
    public AudioClip[] PurpleNebuleuseSound;

    private void Awake()
    {
        GameObject.Find("LoopManager").GetComponent<NewLoopManager>().ReactedToNebuleuse += delegate (NebuleuseType NebuleuseType)
        {
            if (NebuleuseType == NebuleuseType.YELLOW)
                GetComponent<AudioSource>().clip = YellowNebuleuseSound[Random.Range(0, YellowNebuleuseSound.Length)];
            else
                GetComponent<AudioSource>().clip = PurpleNebuleuseSound[Random.Range(0, PurpleNebuleuseSound.Length)];
            GetComponent<AudioSource>().Play();

        };
    }
}
