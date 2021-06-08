using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to track passed time
public class TimeManager : MonoBehaviour
{
    private float time;
    private bool active = false; //Used to track if the timer should run
    private bool topDownActive = false; //Used to track if the time should be spet up

    #region Singleton
    [HideInInspector]
    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (active && time > 0)
        {
            //Set the time based on difficulty and active camera
            if(topDownActive)
                time -= Time.deltaTime * GameManager.Instance.GetDifficulty().timerModifier * GameManager.Instance.GetDifficulty().penaltyModifier;
            else
                time -= Time.deltaTime * GameManager.Instance.GetDifficulty().timerModifier;

            //Tell the GameManager the new score(time)
            GameManager.Instance.SetScore((int)time);
        }
    }

    //Start counting down the clock
    public void StartTimer()
    {
        time = GameManager.Instance.GetScore();
        active = true;
    }

    //Start counting down the clock
    public void StopTimer()
    {
        active = false;
    }

    //Set the state of the topDownCam
    public void SetTopDownActive(bool state)
    {
        topDownActive = state;
    }
}
