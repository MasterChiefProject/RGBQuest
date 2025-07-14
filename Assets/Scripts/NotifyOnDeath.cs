using UnityEngine;

public class NotifyOnDeath : MonoBehaviour
{
    private PoleResetter resetter;   // cached reference

    // Runs as soon as the object is enabled, before gameplay starts
    private void Awake()
    {
        resetter = FindObjectOfType<PoleResetter>();   // <- automatic lookup

        // Optional safety check
        if (resetter == null)
            Debug.LogError("NotifyOnDeath: No PoleResetter found in the scene!");
    }

    private void OnDestroy()
    {
        // Will still work even if *this* object is being destroyed
        if (resetter != null)
            resetter.DespawnAndRespawn();
    }
}
