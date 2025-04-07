using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectSkull : MonoBehaviour
{
    //public Transform player;
    public Collider collider;


    private SkullsManager skullsManager;

    // Start is called before the first frame update
    void Start()
    {
        skullsManager = transform.parent.GetComponent<SkullsManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //TREES MIGHT CAUSE THIS BE CAREFUL
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("This worked firsy");
        skullsManager.createNewSkull();
    }
}
