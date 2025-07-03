using UnityEngine;

/// <summary>
/// Hold‑to‑pickup script (100 % jitter‑free version).
/// • Press <E> → raycast → if we hit a Rigidbody, move it to `holdPoint`,
///   make it kinematic, parent it to `holdPoint` **without** keeping world
///   position, and zero its local transform → it now follows the player.
/// • Release <E> → unparent, restore original parent (optional), toggle
///   `isKinematic` off, re‑enable collisions.
/// </summary>
public class HoldToPickup : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] Transform holdPoint;          // assign via Inspector
    [SerializeField] float maxGrabDistance = 3f;
    [SerializeField] LayerMask grabbableMask = ~0; // everything by default

    /* cached state */
    Rigidbody heldBody;
    Collider heldCol;
    Transform originalParent;
    Collider playerCol;

    void Awake()
    {
        // try find a player collider to ignore collisions with
        playerCol = GetComponent<Collider>() ?? GetComponentInChildren<Collider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) TryGrab();
        if (Input.GetKeyUp(KeyCode.E)) Drop();
    }

    void TryGrab()
    {
        if (heldBody) return; // already holding something

        // centre‑screen raycast (replace with own origin if needed)
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (!Physics.Raycast(ray, out var hit, maxGrabDistance, grabbableMask)) return;

        var rb = hit.rigidbody;
        if (!rb) return;

        // move object to hold point immediately
        rb.transform.position = holdPoint.position;
        rb.transform.rotation = holdPoint.rotation;

        // make it kinematic & parent so it follows perfectly
        originalParent = rb.transform.parent;
        rb.isKinematic = true;
        rb.transform.SetParent(holdPoint, false);  // adopt holdPoint space
        rb.transform.localPosition = Vector3.zero;
        rb.transform.localRotation = Quaternion.identity;

        // ignore collisions with the player while held (optional)
        heldCol = rb.GetComponent<Collider>();
        if (playerCol && heldCol)
            Physics.IgnoreCollision(playerCol, heldCol, true);

        heldBody = rb;
    }

    void Drop()
    {
        if (!heldBody) return;

        // restore parent, physics, and collisions
        heldBody.transform.SetParent(originalParent, true);
        heldBody.isKinematic = false;

        if (playerCol && heldCol)
            Physics.IgnoreCollision(playerCol, heldCol, false);

        heldBody = null;
        heldCol = null;
    }
}
