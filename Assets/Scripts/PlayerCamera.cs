using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera cam;
    public float xSensitivity;
    public float ySensitivity;

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void look(Vector2 direction) {

        Quaternion currentRotation = cam.transform.localRotation;

        //currentRotation.x -= (direction.y * Time.deltaTime) * ySensitivity;
        xRotation -= (direction.y * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

         
        cam.transform.localRotation = Quaternion.Euler(xRotation,0,0);

        transform.Rotate(0, (direction.x * Time.deltaTime) * xSensitivity, 0);
    }
}
