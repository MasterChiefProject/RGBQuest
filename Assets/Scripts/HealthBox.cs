using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBox : MonoBehaviour
{

    private bool isAvailable = true;

    [Header("Text Display")]
    public TextMeshProUGUI displayText;

    [Header("Health Sound")]
    public AudioSource healthSound;
    public float volume = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isAvailable)
        {
            if (Globals.health < 3)
            {
                isAvailable = false;
                Globals.health++;
                StartCoroutine(PlayAndDestroy());
            } else
            {
                Debug.Log("help me please");
                displayText.text = "Full health";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("omg");
            displayText.text = "";
        }
    }

    IEnumerator PlayAndDestroy()
    {
        healthSound.PlayOneShot(healthSound.clip, volume);
        yield return new WaitForSeconds(healthSound.clip.length);
        Destroy(gameObject);
    }

}
