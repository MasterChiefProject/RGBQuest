using UnityEngine;

public class PortalDoor : MonoBehaviour
{
    
    private Animator animator;
    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isOpen)
        {
            animator.SetBool("isOpen", true);
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            animator.SetBool("isOpen", false);
            isOpen = false;
        }
    }
}
