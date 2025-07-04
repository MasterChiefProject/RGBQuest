using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public string matchColor;
    public CubeGlow targetLamp;
    
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
            targetLamp.LightUp();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other && other.CompareTag(matchColor) && isPressed)
        {
            animator.SetBool("isPressed", false);
            targetLamp.LightDown();
            isPressed = false;
        }
    }
    
}
