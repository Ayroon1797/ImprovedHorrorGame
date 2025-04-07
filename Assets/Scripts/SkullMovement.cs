using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullMovement : MonoBehaviour
{
    public int rotationAmount;
    public int heightTranslationSpeed;
    public float heightTranslation;


    // private int spinDegrees;
    // private int currentTranslation;

    private float currentHeight;
    // Start is called before the first frame update
    void Start()
    {
        //spinDegrees = 0;
        //currentTranslation = 0;
        currentHeight = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationAmount);
        transform.position = new Vector3(transform.position.x, currentHeight + heightTranslation * Mathf.Sin(2* Mathf.PI/ heightTranslationSpeed  * Time.time ), transform.position.z);
    }
}
