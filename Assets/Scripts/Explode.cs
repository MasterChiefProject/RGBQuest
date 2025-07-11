using System.Security.Cryptography;
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

    [Header("Ground impact settings")]
    public float minImpactForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet") && intactObject != null)
        {
            Destroy(other.gameObject);
            intactObject.SetActive(false);
            fracturedObject.SetActive(true);

            Vector3 explosionPos = transform.position;
            ExplodeAt(explosionPos);

            DecreaseHealth();

            Destroy(intactObject, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.impulse.magnitude;

        if (impactForce < minImpactForce || !intactObject) return;

        intactObject.SetActive(false);
        fracturedObject.SetActive(true);

        Vector3 explosionPos = collision.contacts[0].point;
        ExplodeAt(explosionPos);

        if (!collision.gameObject.CompareTag("Bullet"))
        {
            DecreaseHealth();
        }

        Destroy(intactObject, 0f);
    }


    private void ExplodeAt(Vector3 explosionPos)
    {
        foreach (var rb in fracturedObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddExplosionForce(
                explosionForce,
                explosionPos,
                explosionRadius,
                upwardsModifier,
                ForceMode.Impulse
            );
        }
    }

    void DecreaseHealth()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            Globals.health--;
        }
    }
}
