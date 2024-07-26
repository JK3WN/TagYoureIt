using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isPlaying = true;
    public static int notCaught = 2;

    private float startTime;

    public TMPro.TextMeshProUGUI leftText, timerText, timerText2, ScoreText;
    public GameObject player, gameOverScreen;
    // Start is called before the first frame update
    void Start()
    {
        isPlaying = true;
        startTime = Time.time;
        notCaught = 2;
        if(player.GetComponent<Rigidbody>() == null) player.AddComponent<Rigidbody>();
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

            if (notCaught <= 0)
            {
                isPlaying = false;
                Destroy(player.GetComponent<Rigidbody>());
                timerText2.text = min + ":" + sec;
                if (timeElapsed < PlayerPrefs.GetFloat("5th"))
                {
                    if (timeElapsed < PlayerPrefs.GetFloat("4th"))
                    {
                        PlayerPrefs.SetFloat("5th", PlayerPrefs.GetFloat("4th"));
                        if (timeElapsed < PlayerPrefs.GetFloat("3rd"))
                        {
                            PlayerPrefs.SetFloat("4th", PlayerPrefs.GetFloat("3rd"));
                            if (timeElapsed < PlayerPrefs.GetFloat("2nd"))
                            {
                                PlayerPrefs.SetFloat("3rd", PlayerPrefs.GetFloat("2nd"));
                                if (timeElapsed < PlayerPrefs.GetFloat("1st"))
                                {
                                    PlayerPrefs.SetFloat("2nd", PlayerPrefs.GetFloat("1st"));
                                    PlayerPrefs.SetFloat("1st", timeElapsed);
                                }
                                else PlayerPrefs.SetFloat("2nd", timeElapsed);
                            }
                            else PlayerPrefs.SetFloat("3rd", timeElapsed);
                        }
                        else PlayerPrefs.SetFloat("4th", timeElapsed);
                    }
                    else PlayerPrefs.SetFloat("5th", timeElapsed);
                }
                min = ((int)PlayerPrefs.GetFloat("1st") / 60).ToString("00");
                min = (PlayerPrefs.GetFloat("1st") % 60).ToString("00.00");
                ScoreText.text = min + ":" + sec;
                min = ((int)PlayerPrefs.GetFloat("2nd") / 60).ToString("00");
                sec = (PlayerPrefs.GetFloat("2nd") % 60).ToString("00.00");
                ScoreText.text += "\n\n" + min + ":" + sec;
                min = ((int)PlayerPrefs.GetFloat("3rd") / 60).ToString("00");
                sec = (PlayerPrefs.GetFloat("3rd") % 60).ToString("00.00");
                ScoreText.text += "\n\n" + min + ":" + sec;
                min = ((int)PlayerPrefs.GetFloat("4th") / 60).ToString("00");
                sec = (PlayerPrefs.GetFloat("4th") % 60).ToString("00.00");
                ScoreText.text += "\n\n" + min + ":" + sec;
                min = ((int)PlayerPrefs.GetFloat("5th") / 60).ToString("00");
                sec = (PlayerPrefs.GetFloat("5th") % 60).ToString("00.00");
                ScoreText.text += "\n\n" + min + ":" + sec;
                gameOverScreen.SetActive(true);
            }
        }
    }

    public void RestartPressed()
    {
        isPlaying = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitPressed()
    {
        SceneManager.LoadScene(0);
    }
}
