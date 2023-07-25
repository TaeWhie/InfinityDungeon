using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PushStartButton()
    {
        SceneManager.LoadScene("InGame");
    }
}
