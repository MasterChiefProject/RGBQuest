using UnityEngine;

/*───────────────────────────────────────────────────────────────
 *  Hold‑to‑Pickup v2.1 · single HoldPoint version (July 2025)
 *  ─────────────────────────────────────────────────────────────
 *  – Press & hold <E> to grab the nearest grabbable rigid‑body.
 *  – Grabbed item turns *kinematic* and is parented to a **HoldPoint
 *    that lives on the PLAYER, not the camera** so it sits correctly in
 *    both first‑ and third‑person views.
 *  – <Mouse‑0> throws; release <E> to drop; hold <R> to rotate.
 *  – LateUpdate clamps the item against walls or floor so it never
 *    tunnels.
 *
 *  QUICK SET‑UP (Option A layout)
 *  1. Under your *Player* object (the one that carries the capsule /
 *     CharacterController) → Create Empty → rename **HoldPoint**.
 *     Place it roughly where the character’s hands should carry items
 *     (e.g. local (0 m, 1.2 m, 0.45 m)).
 *  2. Drag that HoldPoint into the **Hold Point** field below.
 *  3. Tag each pick‑up object with `Grabbable` (or change GRAB_TAG).
 *  4. Put them on the **Grabbable** layer (or adjust grabbableMask).
 *  5. Add this script to **MainCamera** and drag the Player root (capsule
 *     object) into **Player Root**.
 *  Done – item sits in front of the avatar in FP and TP, never inside
 *  walls.
 *──────────────────────────────────────────────────────────────*/

public class HoldToPickup : MonoBehaviour
{
    /*───────────────────── Inspector ▸ References ─────────────────────*/
    [Header("References")]
    [SerializeField] private GameObject playerRoot;   // object with the player collider
    [SerializeField] private Transform holdPoint;     // HoldPoint now lives on the PLAYER
    [SerializeField] private string grabTag;          // tag used on pick‑up objects

    /*────────────────────── Inspector ▸ Tuning ───────────────────────*/
    [Header("Tuning")]
    [SerializeField] private float maxGrabDistance = 5f;
    [SerializeField] private float throwForce = 500f;
    [SerializeField] private float rotateSensitivity = 1f;
    [SerializeField] private LayerMask grabbableMask = ~0;   // layers we can grab



    /*──────────────────── Obstruction clamp settings ─────────────────*/
    [SerializeField] private LayerMask obstructionMask = ~0; // layers that block the held item
    [SerializeField] private float surfaceOffset = 0.03f;    // gap to keep from wall

    /*────────────────────────── Runtime data ─────────────────────────*/
    private Rigidbody heldBody;
    private Collider heldCol;
    private bool canDrop = true;
    private Collider playerCol;  // player collider we ignore

    /*────────────────────────────── UNITY ─────────────────────────────*/
    private void Awake()
    {
        if (!playerRoot) playerRoot = transform.root.gameObject;
        playerCol = playerRoot.GetComponent<Collider>();
        if (!playerCol)
            Debug.LogWarning("[HoldToPickup] Player root has no collider – will not ignore collision with held item.");
    }

    private void Update()
    {
        /* Grab / Drop ---------------------------------------------------*/
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldBody)
            {
                if (canDrop) Drop();
            }
            else TryGrab();
        }

        if (!heldBody) return;

        /* Maintain position ------------------------------------------------*/
        heldBody.transform.position = holdPoint.position;
        heldBody.transform.rotation = holdPoint.rotation;

        /* Throw ---------------------------------------------------------*/
        if (Input.GetMouseButtonDown(0) && canDrop) Throw();

        /* Rotate mode ---------------------------------------------------*/
        if (Input.GetKey(KeyCode.R))
        {
            canDrop = false;
            float xRot = Input.GetAxis("Mouse X") * rotateSensitivity;
            float yRot = Input.GetAxis("Mouse Y") * rotateSensitivity;
            heldBody.transform.Rotate(Vector3.down, xRot, Space.World);
            heldBody.transform.Rotate(Vector3.right, yRot, Space.World);
        }
        else canDrop = true;
    }

    /*─────────────────── Clamp against walls / floor ───────────────────*/
    private void LateUpdate()
    {
        if (!heldBody) return;

        Vector3 camPos = transform.position;
        Vector3 desiredPos = holdPoint.position;
        Vector3 dir = desiredPos - camPos;
        float dist = dir.magnitude;

        if (dist > 0.0001f && Physics.Raycast(camPos, dir.normalized, out RaycastHit hit,
                                              dist, obstructionMask, QueryTriggerInteraction.Ignore))
        {
            desiredPos = hit.point - dir.normalized * surfaceOffset;
        }

        heldBody.transform.position = desiredPos;
    }

    /*─────────────────────────── GRAB / DROP ───────────────────────────*/
    private void TryGrab()
    {
        // OLD (camera distance)  ❌
        //if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,
        //                      maxGrabDistance, grabbableMask, QueryTriggerInteraction.Ignore)) return;

        // NEW (camera ray, but distance checked against the player)  ✅
        float camToPlayer = Vector3.Distance(transform.position, playerRoot.transform.position);
        float rayLength = camToPlayer + maxGrabDistance;          // extend ray so it reaches past the avatar
        if (!Physics.Raycast(transform.position, transform.forward,
                             out RaycastHit hit, rayLength, grabbableMask, QueryTriggerInteraction.Ignore))
            return;

        Debug.DrawLine(transform.position, hit.point, Color.green, 1f);

        // make sure the *player* can actually reach the hit-point
        if (Vector3.Distance(playerRoot.transform.position, hit.point) > maxGrabDistance)
            return;



        if (!hit.transform.CompareTag(grabTag)) return;
        if (!hit.rigidbody)
        {
            Debug.LogWarning($"[HoldToPickup] Grabbable object '{hit.transform.name}' lacks Rigidbody.");
            return;
        }

        heldBody = hit.rigidbody;
        heldCol = heldBody.GetComponent<Collider>();

        heldBody.isKinematic = true;
        heldBody.interpolation = RigidbodyInterpolation.Interpolate;

        heldBody.transform.SetParent(holdPoint, false);
        heldBody.transform.localPosition = Vector3.zero;
        heldBody.transform.localRotation = Quaternion.identity;

        if (playerCol && heldCol)
            Physics.IgnoreCollision(heldCol, playerCol, true);
    }

    private void Drop()
    {
        if (!heldBody) return;
        RestoreCollision();
        heldBody.isKinematic = false;
        heldBody.transform.SetParent(null, true);
        heldBody = null;
        heldCol = null;
    }

    private void Throw()
    {
        if (!heldBody) return;
        RestoreCollision();
        heldBody.isKinematic = false;
        heldBody.transform.SetParent(null, true);
        heldBody.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        heldBody = null;
        heldCol = null;
    }

    private void RestoreCollision()
    {
        if (playerCol && heldCol)
            Physics.IgnoreCollision(heldCol, playerCol, false);
    }
}
