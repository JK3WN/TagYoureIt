using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagement : MonoBehaviour
{
    public TMPro.TextMeshProUGUI ButtonText, ScoreText;
    public GameObject LeaderBoard, ControlBoard;

    private bool leaderBoardOut = true;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("1st")) PlayerPrefs.SetFloat("1st", 5999.99f);
        if (!PlayerPrefs.HasKey("2nd")) PlayerPrefs.SetFloat("2nd", 5999.99f);
        if (!PlayerPrefs.HasKey("3rd")) PlayerPrefs.SetFloat("3rd", 5999.99f);
        if (!PlayerPrefs.HasKey("4th")) PlayerPrefs.SetFloat("4th", 5999.99f);
        if (!PlayerPrefs.HasKey("5th")) PlayerPrefs.SetFloat("5th", 5999.99f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        string min = ((int)PlayerPrefs.GetFloat("1st") / 60).ToString("00");
        string sec = (PlayerPrefs.GetFloat("1st") % 60).ToString("00.00");
        ScoreText.text = min + ":" + sec;
        min = ((int)PlayerPrefs.GetFloat("2nd") / 60).ToString("00");
        sec = (PlayerPrefs.GetFloat("2nd") % 60).ToString("00.00");
        ScoreText.text +=  "\n\n" + min + ":" + sec;
        min = ((int)PlayerPrefs.GetFloat("3rd") / 60).ToString("00");
        sec = (PlayerPrefs.GetFloat("3rd") % 60).ToString("00.00");
        ScoreText.text += "\n\n" + min + ":" + sec;
        min = ((int)PlayerPrefs.GetFloat("4th") / 60).ToString("00");
        sec = (PlayerPrefs.GetFloat("4th") % 60).ToString("00.00");
        ScoreText.text += "\n\n" + min + ":" + sec;
        min = ((int)PlayerPrefs.GetFloat("5th") / 60).ToString("00");
        sec = (PlayerPrefs.GetFloat("5th") % 60).ToString("00.00");
        ScoreText.text += "\n\n" + min + ":" + sec;
    }

    public void StartPressed()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ControlsPressed()
    {
        leaderBoardOut = !leaderBoardOut;
        if (leaderBoardOut)
        {
            LeaderBoard.SetActive(true);
            ControlBoard.SetActive(false);
            ButtonText.text = "Controls";
        }
        else
        {
            LeaderBoard.SetActive(false);
            ControlBoard.SetActive(true);
            ButtonText.text = "High Scores";
        }
    }

    public void QuitPressed()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
}
