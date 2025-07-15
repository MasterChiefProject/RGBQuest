using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerSpawn : MonoBehaviour
{
    void Awake()
    {
        if (Globals.respawnPoint == null)
        {
            var holder = new GameObject("RespawnPoint").transform;
            holder.position = transform.position;
            Globals.respawnPoint = holder;
        }
    }

    [SerializeField] private Transform foreignSpawn;

    public void TeleportToSpawn()
    {
        var cc = GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        Transform chosen = (Random.value < 0.5f) ? Globals.respawnPoint : foreignSpawn;

        transform.position = chosen.position;

        if (cc) cc.enabled = true;
    }
}
