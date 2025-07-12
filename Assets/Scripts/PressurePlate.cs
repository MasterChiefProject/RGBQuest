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
            updatePressurePlateState(isPressed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other && other.CompareTag(matchColor) && isPressed)
        {
            animator.SetBool("isPressed", false);
            targetLamp.LightDown();
            isPressed = false;
            updatePressurePlateState(isPressed);
        }
    }
    
    private void updatePressurePlateState(bool state)
    {
        switch(matchColor)
        {
            case "Yellow":
                Globals.yellowPressurePlateActive = state;
                break;
            case "Blue":
                Globals.bluePressurePlateActive = state;
                break;
            case "Red":
                Globals.redPressurePlateActive = state;
                break;
            case "Purple":
                Globals.purplePressurePlateActive = state;
                break;
        }
    }
}
