using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject aboutPanel;

    public void ExitButtonHandle()
    {
        Application.Quit();
    }

    public void StartButtonHandle()
    {
        SceneManager.LoadScene("Level1");
    }

    public void AboutButtonHandle()
    {
        aboutPanel.SetActive(true);
    }

    public void BackButtonHandle()
    {
        aboutPanel.SetActive(false);
    }
}
