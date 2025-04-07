using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SkullsManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    private GameObject[] skullObjects;
    private Vector3[] skullPositions;

    private int currentSkull = 0;


    // Start is called before the first frame update
    void Start()
    {
        textMeshPro.text = "You have found " + currentSkull + " / 6 skulls";

        skullObjects = new GameObject[6];
        skullPositions = new Vector3[6];
        populateValues();

        for (int i = 1; i < skullObjects.Length; i++) { 
            skullObjects[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void populateValues()
    {
        for(int i = 0; i < skullPositions.Length; i++)
        {
            skullObjects[i] = transform.GetChild(i).gameObject;
            skullPositions[i] = skullObjects[i].transform.position;
        }
    }

    public void createNewSkull()
    {
        skullObjects[currentSkull].SetActive(false);
        currentSkull++;

        textMeshPro.text = "You have found " + currentSkull + " / 6 skulls";
        //new skull created

        if(currentSkull != 6)
        {
            skullObjects[currentSkull].SetActive(true);
        }
           
    }

}
