using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionSequence : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private bool startWithIntroduction = true;
    [SerializeField] private float ascensionDuration = 5;
    [SerializeField] private int finalRotationAxisNumber = 3;
    [Header("References")]
    [SerializeField] private PlayerAxisScript player = null;
    [SerializeField] private Transform mainCamera = null;
    [SerializeField] private Transform startingPoint = null;
    [SerializeField] private GameObject floor = null;

    private Vector3 endPoint = new Vector3();
    private float counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(startWithIntroduction)
        {
            floor.SetActive(false);
            endPoint = mainCamera.position;
            mainCamera.position = startingPoint.position;
            player.canMove = false;
        } else
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.position = Vector3.Lerp(startingPoint.position, endPoint, counter / ascensionDuration);
        counter += Time.deltaTime;

        if(counter >= ascensionDuration)
        {
            floor.SetActive(true);
            player.canMove = true;
            //player.RotateToAxis(finalRotationAxisNumber);
            enabled = false;
        }
    }
}
