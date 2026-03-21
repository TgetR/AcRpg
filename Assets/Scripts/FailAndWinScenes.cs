using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailAndWinScenes : MonoBehaviour
{

    [SerializeField] private TMP_Text AttamptsText;
    [SerializeField] private TMP_Text HoursPlayedText;
    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Start()
    {
        AttamptsText.text = "Attempts: " + PlayerPrefs.GetInt("Attempts", 0);
        HoursPlayedText.text = "Hours Played: " + PlayerPrefs.GetFloat("HoursPlayed", 0);
    }
}
