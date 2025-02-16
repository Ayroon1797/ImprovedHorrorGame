using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    [Header("Stamina Bar Values")]
    public float regenSpeed;
    public float regenDelay;
    public float flashingSpeed;


    [Header("References")]
    public Image staminaBar;
    public Image border;
    public Image stamIcon;

    private Coroutine stamBarCoRef;
    private bool savedSprinting;
    private Color[] stamColors = {new Color(0f,1f,0f), new Color(1f, 1f, 0f), new Color(1f, 0f, 0f) };

    // Start is called before the first frame update
    void Start()
    {
        stamBarCoRef = null;
        savedSprinting = false;
    }

    // Update is called once per frame
    void Update()
    {
       /*if(staminaBar.fillAmount == 0)
        {

            InputFlags.sprintFlag = false;
            StartCoroutine(staminaRegen());
            StartCoroutine(exhaustionFlasher());

        }*/

        if(savedSprinting != PlayerStates.isSprinting)
        {
            savedSprinting = PlayerStates.isSprinting;

            if (stamBarCoRef != null)
            {
                StopCoroutine(stamBarCoRef);
                stamBarCoRef = null;
            }
                

            if (savedSprinting)
            {
                stamBarCoRef = StartCoroutine(staminaDrain());
            }
            else
            {
                stamBarCoRef = StartCoroutine(staminaRegen());
            }
        }
    }

    private IEnumerator staminaRegen() 
    {

        yield return new WaitForSeconds(regenDelay);

        while (staminaBar.fillAmount < 1)
        {
            staminaBar.fillAmount += regenSpeed * Time.deltaTime;

            iconColorManager();

            yield return null;
        }

        staminaBar.fillAmount = 1;
        stamBarCoRef = null;
        yield break;
    }

    private IEnumerator staminaDrain()
    {

        while (staminaBar.fillAmount > 0)
        {
            staminaBar.fillAmount -= regenSpeed * Time.deltaTime;

            iconColorManager();

            yield return null;

        }

        staminaBar.fillAmount = 0;
        stamBarCoRef = null;

        InputFlags.sprintFlag = false;
        stamBarCoRef = StartCoroutine(staminaRegen());
        StartCoroutine(exhaustionFlasher());
        yield break;
    }

    private void iconColorManager()
    {
        if (staminaBar.fillAmount >= 0.5)
        {
            float colorRef = (staminaBar.fillAmount - 0.5f) * 2;
            staminaBar.color = Color.Lerp(stamColors[1], stamColors[0], colorRef);
        }
        else
        {
            staminaBar.color = Color.Lerp(stamColors[2], stamColors[1], staminaBar.fillAmount * 2);
        }
    }

    private IEnumerator exhaustionFlasher() 
    {
        float totalTime = 0;

        while(staminaBar.fillAmount < 1)
        {
            float lerpFactor = Mathf.PingPong(totalTime, 1);
            Color iconColor = Color.Lerp(Color.white, Color.red, lerpFactor);

            border.color = iconColor;
            stamIcon.color = iconColor;

            totalTime += Time.deltaTime * flashingSpeed;
            yield return null;
        }

        border.color = Color.white;
        stamIcon.color = Color.white;
        InputFlags.sprintFlag = true;
    }
}
