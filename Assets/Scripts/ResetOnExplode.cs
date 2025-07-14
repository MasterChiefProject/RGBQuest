using UnityEngine;
using System.Linq;         // for FirstOrDefault, optional but nice

[RequireComponent(typeof(Explode))]
public class ResetOnExplode : MonoBehaviour
{
    [Header("Which pole should I reset?")]
    [Tooltip("Exact GameObject name of the pole’s PoleResetter, "
           + "e.g.  \"YellowPoleResetter\"  or  \"RedPoleResetter\".")]
    [SerializeField] private string resetterName;      // ← type it in Inspector

    private Explode explode;
    private PoleResetter resetter;

    /* ─────────────────────────────── */
    private void Awake()
    {
        explode = GetComponent<Explode>();

        // 1️⃣  Find the object by name (scene-wide, one time only)
        if (!string.IsNullOrEmpty(resetterName))
        {
            var go = GameObject.Find(resetterName);
            if (go) resetter = go.GetComponent<PoleResetter>();
        }

        // 2️⃣  Fallback: same-hierarchy search (optional)
        if (!resetter)
            resetter = GetComponentInParent<PoleResetter>();

        // 3️⃣  Subscribe to explosion event
        if (explode) explode.Exploded += HandleExploded;
    }

    /* ─────────────────────────────── */
    private void HandleExploded()
    {
        if (resetter) resetter.DespawnAndRespawn();
    }

    private void OnDestroy()
    {
        if (resetter) resetter.DespawnAndRespawn();
        if (explode) explode.Exploded -= HandleExploded;
    }
}
