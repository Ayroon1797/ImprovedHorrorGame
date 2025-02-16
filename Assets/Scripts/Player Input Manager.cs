using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    //private fields
    private PlayerControls playerControls;
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;

    private void OnEnable()
    {
        //initializes fields
        playerControls = new PlayerControls();
        playerControls.Enable();

        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = GetComponent<PlayerCamera>();

    }

    // Start is called before the first frame update
    void Start()
    {

        playerControls.Normal.Sprint.performed += playerMovement.sprint;
        playerControls.Normal.Sprint.canceled += playerMovement.sprint;

        playerControls.Normal.Crouch.performed += playerMovement.crouch;

    }

    // Update is called once per frame
    void Update()
    {
        //sends input info to the walk method
        playerMovement.walk(playerControls.Normal.Movement.ReadValue<Vector2>());
        playerCamera.look(playerControls.Normal.Look.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
