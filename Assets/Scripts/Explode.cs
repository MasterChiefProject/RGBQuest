using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Explode : MonoBehaviour
{

    [Header("Intact & Fractured meshes")]
    [SerializeField] GameObject intactObject;
    [SerializeField] GameObject fracturedObject;

    [Header("Explosion")]
    [SerializeField] float explosionForce = 500f;
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] float upwardsModifier = 2f;

    [Header("Ground impact")]
    [SerializeField] float minImpactForce = 10f;
    [SerializeField] string bulletTag = "Bullet";
    [SerializeField] string groundTag = "Ground";

    [Header("Reset")]
    [SerializeField] bool shouldReset = true;
    [SerializeField] float resetDelay = 5f;

    /* ─────────────────────────── internals ────────────────────────── */
    bool exploded;
    Collider col;
    Rigidbody rb;
    Coroutine resetRoutine;
    Vector3 startPos;
    Quaternion startRot;
    Vector3 intactLocalPos;
    Quaternion intactLocalRot;
    readonly Dictionary<Transform, (Vector3 pos, Quaternion rot)> shardPose =
        new Dictionary<Transform, (Vector3, Quaternion)>();

    void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        startRot = transform.rotation;
        intactLocalPos = intactObject.transform.localPosition;
        intactLocalRot = intactObject.transform.localRotation;

        // includeInactive = true because fracturedObject starts inactive
        foreach (var t in fracturedObject.GetComponentsInChildren<Transform>(true))
            shardPose[t] = (t.localPosition, t.localRotation);

        fracturedObject.SetActive(false);
    }

    /* ────────────── collisions ────────────── */
    void OnCollisionEnter(Collision c)
    {
        if (exploded) return;

        if (c.gameObject.CompareTag(bulletTag))
        {
            Destroy(c.gameObject);
            ExplodeAt(c.GetContact(0).point);
        }
        else if (c.gameObject.CompareTag(groundTag) &&
                 c.impulse.magnitude >= minImpactForce)
        {
            ExplodeAt(c.GetContact(0).point);
        }
    }

    /* ────────────── main explode logic ────────────── */
    void ExplodeAt(Vector3 pos)
    {
        exploded = true;

        col.enabled = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;

        intactObject.SetActive(false);
        fracturedObject.SetActive(true);

        foreach (var frag in fracturedObject.GetComponentsInChildren<Rigidbody>())
        {
            frag.linearVelocity = Vector3.zero;
            frag.angularVelocity = Vector3.zero;
            frag.AddExplosionForce(explosionForce, pos, explosionRadius,
                                   upwardsModifier, ForceMode.Impulse);
        }

        if (shouldReset)
        {
            if (resetRoutine != null) StopCoroutine(resetRoutine);
            resetRoutine = StartCoroutine(ResetAfterDelay());
        }
    }

    /* ────────────── reset coroutine ────────────── */
    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = startPos;
        rb.rotation = startRot;
        rb.Sleep();

        intactObject.transform.localPosition = intactLocalPos;
        intactObject.transform.localRotation = intactLocalRot;

        foreach (var kvp in shardPose)
        {
            kvp.Key.localPosition = kvp.Value.pos;
            kvp.Key.localRotation = kvp.Value.rot;
        }

        fracturedObject.SetActive(false);
        intactObject.SetActive(true);

        col.enabled = true;
        rb.useGravity = true;
        exploded = false;
    }
}
