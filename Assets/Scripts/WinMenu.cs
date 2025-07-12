using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public void BackButtonHandle()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
