using UnityEngine;

public class PortalDoor : MonoBehaviour
{

    public AudioClip openAudioClip;

    private AudioSource audioSource;
    private Animator animator;
    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isOpen)
        {
            animator.SetBool("isOpen", true);
            audioSource.PlayOneShot(openAudioClip);
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            animator.SetBool("isOpen", false);
            audioSource.PlayOneShot(openAudioClip);
            isOpen = false;
        }
    }
}
