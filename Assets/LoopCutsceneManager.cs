using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoopCutsceneManager : MonoBehaviour
{
    [SerializeField] private string loopCutsceneScene = "LoopCutscene";

    private TimeLoopRecorder timeLoopRecorder = null;
    private TimeManager timeManager = null;

    private List<TimeLoopKey> keys = null;
    private float loopDuration = 60;

    private string previousSceneName;

    // Start is called before the first frame update
    void Start()
    {
        timeLoopRecorder = FindObjectOfType<TimeLoopRecorder>();
        if (!timeLoopRecorder) Debug.LogError("LoopCutsceneManager needs a TimeLoopRecorder to function");

        timeManager = FindObjectOfType<TimeManager>();
        if (!timeManager) Debug.LogError("LoopCutsceneManager needs a TimeManager to function");
        else
        {
            timeManager.OnLoopFinished += LoopFinished;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoopFinished()
    {
        if(timeLoopRecorder && timeManager)
        {
            DontDestroyOnLoad(gameObject);

            keys = timeLoopRecorder.EndLoop();

            loopDuration = timeManager.LoopDuration;

            previousSceneName = SceneManager.GetActiveScene().name;

            SceneManager.activeSceneChanged += CutsceneSceneLoaded;

            SceneManager.LoadScene(loopCutsceneScene);
        }
    }

    private void CutsceneSceneLoaded(Scene previousScene, Scene newScene)
    {
        SceneManager.activeSceneChanged -= CutsceneSceneLoaded;
        LoopCutscenePlayer cutscenePlayer = FindObjectOfType<LoopCutscenePlayer>();
        cutscenePlayer.OnFinishedPlaying += CutsceneFinishedPlaying;
        cutscenePlayer.PlayRecordedLoop(keys, loopDuration);
    }

    private void CutsceneFinishedPlaying()
    {
        

        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        SceneManager.LoadScene(previousSceneName);
    }
}
