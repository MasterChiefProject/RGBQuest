using System.Security.Cryptography;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [Header("Intact cube and fractured cube")]
    public GameObject intactCube;
    public GameObject fracturedCube;

    [Header("Explosion settings")]
    public float explosionForce = 500f;
    public float explosionRadius = 3f;
    public float upwardsModifier = 2f;

    [Header("Ground impact settings")]
    public float minImpactForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet") && intactCube != null)
        {
            intactCube.SetActive(false);
            fracturedCube.SetActive(true);

            Vector3 explosionPos = transform.position;
            ExplodeAt(explosionPos);

            Destroy(intactCube, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.impulse.magnitude;

        if (impactForce < minImpactForce || !intactCube) return;

        intactCube.SetActive(false);
        fracturedCube.SetActive(true);

        Vector3 explosionPos = collision.contacts[0].point;
        ExplodeAt(explosionPos);

        Destroy (intactCube, 0f);
    }


    private void ExplodeAt(Vector3 explosionPos)
    {
        foreach (var rb in fracturedCube.GetComponentsInChildren<Rigidbody>())
        {
            rb.velocity = Vector3.zero;
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
}
