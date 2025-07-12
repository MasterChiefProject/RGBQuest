using System.Collections;
using UnityEngine;

public class Pole : MonoBehaviour
{
    [Header("Respawn settings")]
    public GameObject ropePrefab;
    public float respawnDelay = 5f;
    public string watchForChild = "YellowCubeRope";

    bool respawning;

    void Update()
    {
        // find the _root_ rope object by name
        var rope = transform.Find(watchForChild);

        // if it’s gone and we’re not already waiting…
        if (rope == null && !respawning)
        {
            respawning = true;
            StartCoroutine(RespawnSelf());
        }
    }

    IEnumerator RespawnSelf()
    {
        yield return new WaitForSeconds(respawnDelay);

        // in case there _is_ some leftover, destroy it
        var old = transform.Find(watchForChild);
        if (old != null)
            Destroy(old.gameObject);

        // spawn the new rope as a child
        var go = Instantiate(
            ropePrefab,
            transform.position,
            transform.rotation,
            transform
        );
        go.name = watchForChild;

        respawning = false;
    }
}
