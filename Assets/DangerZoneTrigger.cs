using UnityEngine;
using UnityEngine.SceneManagement;
public class DangerZoneTrigger : MonoBehaviour
{
    [SerializeField] int damage = 1;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Globals.health -= damage;
        Globals.health = Mathf.Max(Globals.health, 0);

        var spawner = other.GetComponent<PlayerSpawn>();

        if (spawner)
        {
            if (Globals.health == 0)
            {
                SceneManager.LoadScene("DeathMenu");
            }
            else
            {
                spawner.TeleportToSpawn();
            }
        }
    }
}
