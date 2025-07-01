using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportTo.position;
            other.transform.rotation = teleportTo.rotation;
            //TODO: move to the next scene instead !
            //TODO: check last scene -> End
        }
    }
}
