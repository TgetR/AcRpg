using UnityEngine;
using UnityEngine.SceneManagement;

public class FailAndWinScenes : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }
}
