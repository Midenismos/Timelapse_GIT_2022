using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScreenScript : MonoBehaviour
{
    [SerializeField] private MeshRenderer _interactFeedBack;

    private void OnMouseOver()
    {
        _interactFeedBack.enabled = true;


    }

    private void OnMouseExit()
    {
        _interactFeedBack.enabled = false;
    }




    /*private void Update()
    {
        if(Movie.clip != null)
        {
            //Change la vitesse du film en fonction du TimeManager
            if (!_timeManager.rewindManager.isRewinding)
                Movie.playbackSpeed = _timeManager.multiplier;
            else//Rewind le film
            {
                Movie.playbackSpeed = 0;
                Movie.time = Movie.time - 0.09f;
            }

            //Fait apparaître ou disparaître le boutton lorsque le film est en marche.
            if(!Movie.isPlaying)
                _button.SetActive(true);
            else if(Movie.time <=0.08)
            {
                _button.SetActive(true);
                if(_timeManager.rewindManager.isRewinding)
                    Movie.Stop();
            }
            if(Movie.isPlaying && Movie.time != 0.08)
            {
                _button.SetActive(false);

            }
        }
        else
            _button.SetActive(false);


    }*/

}
