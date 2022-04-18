using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class CamScreenScript : MonoBehaviour
{
    public int WhichPurple;
    [SerializeField] private MeshRenderer _interactFeedBack;


    [Header("Sinon mettre la vidéo dans le VideoPlayer")]
    [Header("0: Violet, 1: Sans influence, 2: Vert, 3: Bleu")]
    [Header("Si cette cam réagit aux nébuleuses, mettre les vidéo dans l'ordre")]

    [SerializeField] public VideoClip[] Videos = new VideoClip[5];


    public OnOffButton _onOffButton = null;
    private void OnMouseOver()
    {
        _interactFeedBack.enabled = true;
    }

    private void OnMouseExit()
    {
        _interactFeedBack.enabled = false;
    }

    private void Awake()
    {
        GameObject.Find("LoopManager").GetComponent<NewLoopManager>().ReactedToNebuleuse += delegate (NebuleuseType NebuleuseType)
        {
            VideoPlayer player = GetComponent<VideoPlayer>();
            float time = (float)GetComponent<VideoPlayer>().time;
            if (NebuleuseType == NebuleuseType.PURPLE1)
            {
                if (WhichPurple == 1)
                    player.clip = Videos[0];
                else
                    player.clip = Videos[1];
            }
            else if (NebuleuseType == NebuleuseType.PURPLE2)
            {
                if (WhichPurple == 2)
                    player.clip = Videos[0];
                else
                    player.clip = Videos[1];
            }
            /*else if (NebuleuseType == NebuleuseType.GREEN)
            {
                if (_videos[2] != null)
                    player.clip = _videos[2];
                else
                    player.clip = _videos[1];
            }
            else if (NebuleuseType == NebuleuseType.BLUE)
            {
                if (_videos[3] != null)
                    player.clip = _videos[3];
                else
                    player.clip = _videos[1];
            }*/
            else if (NebuleuseType == NebuleuseType.YELLOW)
                player.clip = Videos[1];

            /*if(player.clip = Videos[1])
                player.time = time;*/
            GameObject.Find("Console").GetComponent<ConsoleManager>().ChangeTime();
            if (!_onOffButton.IsActivated)
                player.Stop();
        };
        if (WhichPurple == 1)
            GetComponent<VideoPlayer>().clip = Videos[0];
        else
            GetComponent<VideoPlayer>().clip = Videos[1];

        OnOff();
    }


    public void OnOff()
    {
        if (_onOffButton.IsActivated == false)
        {
            GetComponent<VideoPlayer>().Stop();
            GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().HowManyMachineActivated -= 1;
        }
        else
        {
            GetComponent<VideoPlayer>().time = GameObject.Find("SliderVideo").GetComponent<Slider>().value;
            GetComponent<VideoPlayer>().Play();
            GameObject.Find("EnergyMetter").GetComponent<EnergyMetterScript>().HowManyMachineActivated += 1;
        }
    }

    private void Update()
    {

        /*if(Movie.clip != null)
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

}
