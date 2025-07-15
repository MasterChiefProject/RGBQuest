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
            if (nextScene.Equals("Level2"))
            {
                Globals.ammo = 1;
                Globals.hasGun = true;
                Globals.gunActive = true;
            }

            Globals.resetCubesForNextLevel();
            SceneManager.LoadScene(nextScene);
        }
    }
}
