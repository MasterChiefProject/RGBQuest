using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool spent = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rope"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (spent) return;

        if (!other.CompareTag("Ghost")) return;

        spent = true;

        GetComponent<Collider>().enabled = false;

        Destroy(other.transform.gameObject);

        Destroy(gameObject);
    }
}
