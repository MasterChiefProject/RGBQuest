using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [Header("Intact cube and fractured cube")]
    public GameObject intactObject;
    public GameObject fracturedObject;

    [Header("Explosion settings")]
    public float explosionForce = 500f;
    public float explosionRadius = 3f;
    public float upwardsModifier = 2f;
    public bool shouldReset = true;

    [Header("Ground impact settings")]
    public float minImpactForce = 10f;
    public string bulletTag = "Bullet";
    public string groundTag = "Ground";

    [Header("Seconds to wait before resetting the cube")]
    public float resetDelay = 5f;

    private bool hasExploded;
    private Collider parentCol;
    private Rigidbody parentRb;
    private Coroutine resetRoutine;
    private Vector3 savedPos;
    private Quaternion savedRot;
    private Vector3 intactSavedLocalPos;
    private Quaternion intactSavedLocalRot;
    private Dictionary<Transform, (Vector3, Quaternion)> shardData;

    void Awake()
    {
        parentCol = GetComponent<Collider>();
        parentRb = GetComponent<Rigidbody>();
        savedPos = transform.position;
        savedRot = transform.rotation;
        intactSavedLocalPos = intactObject.transform.localPosition;
        intactSavedLocalRot = intactObject.transform.localRotation;
        shardData = new Dictionary<Transform, (Vector3, Quaternion)>();
        foreach (var t in fracturedObject.GetComponentsInChildren<Transform>())
            shardData[t] = (t.localPosition, t.localRotation);
        fracturedObject.SetActive(false);
    }

    void OnCollisionEnter(Collision c)
    {
        if (hasExploded) return;
        if (c.gameObject.CompareTag(bulletTag))
        {
            Destroy(c.gameObject);
            ExplodeAt(c.contacts[0].point);
        }
        else if (c.gameObject.CompareTag(groundTag) && c.impulse.magnitude >= minImpactForce)
        {
            ExplodeAt(c.contacts[0].point);
        }
    }

    void ExplodeAt(Vector3 pos)
    {
        hasExploded = true;
        parentCol.enabled = false;
        parentRb.velocity = Vector3.zero;
        parentRb.angularVelocity = Vector3.zero;
        parentRb.useGravity = false;
        intactObject.SetActive(false);
        fracturedObject.SetActive(true);
        foreach (var rb in fracturedObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddExplosionForce(explosionForce, pos, explosionRadius, upwardsModifier, ForceMode.Impulse);
        }
        if (shouldReset)
        {
            if (resetRoutine != null) StopCoroutine(resetRoutine);
            resetRoutine = StartCoroutine(ResetAfterDelay());
        }
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        parentRb.velocity = Vector3.zero;
        parentRb.angularVelocity = Vector3.zero;
        parentRb.MovePosition(savedPos);
        parentRb.MoveRotation(savedRot);
        parentRb.Sleep();
        intactObject.transform.localPosition = intactSavedLocalPos;
        intactObject.transform.localRotation = intactSavedLocalRot;
        intactObject.SetActive(true);
        foreach (var kvp in shardData)
        {
            var t = kvp.Key;
            t.localPosition = kvp.Value.Item1;
            t.localRotation = kvp.Value.Item2;
        }
        fracturedObject.SetActive(false);
        parentCol.enabled = true;
        parentRb.useGravity = true;
        hasExploded = false;
    }
}
