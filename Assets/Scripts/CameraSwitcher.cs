using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;          // ⬅️  NEW  (was “Cinemachine”)

public class CameraSwitcher : MonoBehaviour
{
    /* ----------------  Inspector fields  ---------------- */

    [Header("Virtual Cameras")]
    [SerializeField] private CinemachineVirtualCamera thirdPersonCam;
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;

    [Header("Character Controllers")]
    [SerializeField] private MonoBehaviour thirdPersonController;   // 3rd-person script
    [SerializeField] private MonoBehaviour firstPersonController;   // 1st-person script

    [Header("Input (optional)")]
    [Tooltip("Drag an InputActionReference that listens to 'SwitchView' (keyboard V).")]
    [SerializeField] private InputActionReference switchViewAction;

    /* ----------------  Private state  ---------------- */
    private bool _isFirstPerson;

    /* ----------------  Lifecycle  ---------------- */

    private void OnEnable()
    {
        if (switchViewAction) switchViewAction.action.performed += OnSwitchView;
    }

    private void OnDisable()
    {
        if (switchViewAction) switchViewAction.action.performed -= OnSwitchView;
    }

    private void Start() => ActivateThirdPerson();   // default

    /* ----------------  Input handlers  ---------------- */

    private void OnSwitchView(InputAction.CallbackContext ctx) => ToggleView();

    private void Update()
    {
        // Fallback in case you didn't wire an InputActionReference
        if (switchViewAction == null && Keyboard.current.vKey.wasPressedThisFrame)
            ToggleView();
    }

    /* ----------------  View logic  ---------------- */

    private void ToggleView()
    {
        if (_isFirstPerson) ActivateThirdPerson();
        else ActivateFirstPerson();
    }

    private void ActivateFirstPerson()
    {
        _isFirstPerson = true;

        firstPersonCam.Priority = 20;
        thirdPersonCam.Priority = 10;

        firstPersonController.enabled = true;
        thirdPersonController.enabled = false;
    }

    private void ActivateThirdPerson()
    {
        _isFirstPerson = false;

        firstPersonCam.Priority = 10;
        thirdPersonCam.Priority = 20;

        firstPersonController.enabled = false;
        thirdPersonController.enabled = true;
    }
}
