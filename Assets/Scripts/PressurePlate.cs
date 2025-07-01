using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public string matchColor;
    
    private bool isPressed = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other && other.CompareTag(matchColor) && !isPressed) { 
            animator.SetBool("isPressed", true);
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other && other.CompareTag(matchColor) && isPressed)
        {
            animator.SetBool("isPressed", false);
            isPressed = false;
        }
    }
    
}
