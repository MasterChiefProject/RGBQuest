using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [Header("Next Scene")]
    public string nextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Globals.checkAllPressurePlatesActive())
        {
            Globals.resetCubesForNextLevel();
            SceneManager.LoadScene(nextScene);
        }
    }
}
