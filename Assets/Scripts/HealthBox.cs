using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBox : MonoBehaviour
{

    private bool isAvailable = true;

    public AudioSource healthSound;
    public float volume = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isAvailable)
        {
            isAvailable = false;
            Globals.health++;
            StartCoroutine(PlayAndDestroy());
        }
    }

    IEnumerator PlayAndDestroy()
    {
        healthSound.PlayOneShot(healthSound.clip, volume);
        yield return new WaitForSeconds(healthSound.clip.length);
        Destroy(gameObject);
    }

}
