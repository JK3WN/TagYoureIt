using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isPlaying = true;
    public static int notCaught = 3;

    private float startTime;

    public TMPro.TextMeshProUGUI leftText, timerText;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            leftText.text = "Enemies Left: " + notCaught;

            float timeElapsed = Time.time - startTime;
            string min = ((int)timeElapsed / 60).ToString("00");
            string sec = (timeElapsed % 60).ToString("00.00");
            timerText.text = min + ":" + sec;
        }
    }
}
