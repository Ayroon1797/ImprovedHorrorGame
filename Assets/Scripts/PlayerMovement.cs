using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walking Settings")]
    public float walkSpeed;

    [Header("Sprinting Settings")]
    public float sprintSpeed;
    public float sprintAccel;

    [Header("Jump Settings")]
    public float jumpHeight;

    [Header("Crouch Settings")]
    public Camera cam;
    private float camNormPos;
    private float camCrouchPos;
    public float crouchMoveSpeed;
    public float crouchSpeed;
    public float crouchHeight;
    public float normalHeight;

    [SerializeField]
    private float currentSpeed;

    private Coroutine sprintCoRef = null;
    private Coroutine crouchCoRef = null;

    private float xMove = 0;
    private float zMove = 0;

    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        camNormPos = cam.transform.localPosition.y;
        camCrouchPos = camNormPos / (normalHeight / crouchHeight);

        characterController = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //if the stamina bar drains while player is sprinting, resets the speed
        if (!InputFlags.sprintFlag && PlayerStates.isSprinting)
        {
            if (sprintCoRef != null)
                StopCoroutine(sprintCoRef);

            PlayerStates.isSprinting = false;
            sprintCoRef = StartCoroutine(SprintCo(walkSpeed, currentSpeed));
        }

        Vector3 moveVector = new Vector3(xMove, 0, zMove);

        characterController.Move(transform.TransformDirection(moveVector) * Time.deltaTime * currentSpeed);

        xMove = 0;
        zMove = 0;
    }

    public void walk(Vector2 input)
    {
        Vector3 move = Vector3.zero;

        xMove += input.x;
        zMove += input.y;
    }

    public void sprint(InputAction.CallbackContext input)
    {
        if (InputFlags.sprintFlag && !PlayerStates.isCrouching)
        {
            if(sprintCoRef != null)
                StopCoroutine(sprintCoRef);

             if (input.performed)
             {
                PlayerStates.isSprinting = true;
                sprintCoRef = StartCoroutine(SprintCo(sprintSpeed, currentSpeed));
             }
             else if (input.canceled)
             {
                PlayerStates.isSprinting = false;
                sprintCoRef = StartCoroutine(SprintCo(walkSpeed, currentSpeed));
            }

          
        }
    }

    /*
     
     
     FOR STAMINA BAR MIGHT NEED SCRIPT. iDEA IS TO HAVE BLACK LIGHTNIG BOLT WITH BLACK BORDER AROUND THE STAMINA, AS THE STAMINA DECREASES THE BOX WILL UN FILL AND
    THE COLORS WILL BE LERPED FROM GOING FROM A RADIANT GREEN TO YELLOW TO RED. ONCE ALL STAMINA IS GONE THEN THE BACKGROUND BEHIND THE STAMINA BAR WILL BE ALL WHITE SO THE BLACK BORDER AND THE BLACK LIGHTNING BOLT
    WILL PULSE BETWEEN BLACK AND RED ON A LOOP UNTIL MAX STAMINA
     
     
     
     
     
     
     
     
     */



    private IEnumerator SprintCo(float targetSpeed, float nowSpeed) {
        float totalTime = 0;

        while (totalTime < sprintAccel)
        {
             currentSpeed = Mathf.Lerp(nowSpeed, targetSpeed, totalTime / sprintAccel);

            totalTime += Time.deltaTime;
            yield return null;
        }

        sprintCoRef = null;
        yield break;
    }

    public void crouch(InputAction.CallbackContext input) 
    {
        if (InputFlags.crouchFlag)
        { 
            InputFlags.crouchFlag = false;

            if (!PlayerStates.isCrouching)
            {
                //cancels the sprinting CoRoutine if crouch is selected
                if (sprintCoRef != null)
                {
                    StopCoroutine(sprintCoRef);
                    PlayerStates.isSprinting = false;
                    sprintCoRef = null;
                }
                   

                //start crouch coRoutine
                StartCoroutine(CrouchCo(crouchHeight, camCrouchPos, crouchMoveSpeed, characterController.height, cam.transform.localPosition.y, currentSpeed));
            }
            else
            {
                Ray ray = new Ray(transform.position, Vector3.up);

                if (Physics.Raycast(ray, (((normalHeight - crouchHeight) / 2) + normalHeight/2)))
                {
                    Debug.Log("You cannot uncrouch at the moment");
                    InputFlags.crouchFlag = true;
                }
                else
                {
                    //start uncrouch coRoutine
                    StartCoroutine(CrouchCo(normalHeight, camNormPos, walkSpeed, characterController.height, cam.transform.localPosition.y, currentSpeed));
                } 
            }
        }
    }


    private IEnumerator CrouchCo(float targetHeight, float targetCamHeight, float targetSpeed,float nowHeight, float nowCamHeight, float nowSpeed)
    {
        PlayerStates.isCrouching = !PlayerStates.isCrouching;
        float totalTime = 0;

        while (totalTime < crouchSpeed)
        {
            currentSpeed = Mathf.Lerp(nowSpeed, targetSpeed, totalTime/ crouchSpeed);
            characterController.height = Mathf.Lerp(nowHeight, targetHeight, totalTime / crouchSpeed);
            cam.transform.localPosition = new Vector3(0, Mathf.Lerp(nowCamHeight, targetCamHeight, totalTime / crouchSpeed), 0);

            characterController.Move(new Vector3(0, (((targetHeight - nowHeight) / 2) / (crouchSpeed / Time.deltaTime)), 0));

            totalTime += Time.deltaTime;
            yield return null;
        }

        //fixes the small lerp errors
        cam.transform.localPosition = new Vector3(0, targetCamHeight, 0);
        characterController.height = targetHeight;

        InputFlags.crouchFlag = true;
        yield break;
    }
}

