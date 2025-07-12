using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void YesButtonHandle()
    {
        Globals.resetGlobalsToDefaults();
        SceneManager.LoadScene("Level1");
    }

    public void NoButtonHandle()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
