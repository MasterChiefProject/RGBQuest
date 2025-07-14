using UnityEngine;

[RequireComponent(typeof(CharacterController))]   // or whatever you move with
public class PlayerSpawn : MonoBehaviour
{

    void Awake()
    {
        // record the player’s starting position the very first time
        if (Globals.respawnPoint == null)
        {
            var holder = new GameObject("RespawnPoint").transform;
            holder.position = transform.position;
            Globals.respawnPoint = holder;
        }
    }

    public void TeleportToSpawn()
    {
        // disable controller during teleport to avoid unwanted physics
        var cc = GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        transform.position = Globals.respawnPoint.position;

        if (cc) cc.enabled = true;
    }
}
