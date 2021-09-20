using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitSlider : MonoBehaviour
{
    public Slider strengthSlider;
    public bool isHit;
    [SerializeField] Image fillImage;
    [SerializeField] Color maxFillColor;
    [SerializeField] float maxHitGapLength = 0.1f;

    public GameObject instructionsText;
    public GameObject reloadText;

    float timeSinceNotHit = 0f;
    // Start is called before the first frame update
    void Start()
    {
        ResetSlider();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isHit)
        {

            strengthSlider.value = Mathf.PingPong(Time.time - timeSinceNotHit, strengthSlider.maxValue);
        }
        else
        {
            timeSinceNotHit = Time.time;
        }

        if(fillImage)
        {
            // Debug.Log("Slider value is : " + strengthSlider.value);
            if((strengthSlider.value >= 0f && strengthSlider.value < 0.2f) || (strengthSlider.value > 0.8f && strengthSlider.value <= 1.0f))
            {
                fillImage.color = Color.green;
            }
            else if ((strengthSlider.value >= 0.2f && strengthSlider.value < 0.45f) || (strengthSlider.value > 0.55f && strengthSlider.value <= 0.8f))
            {
                fillImage.color = new Color(1, 0.64f, 0, 1);
            }
            else if(strengthSlider.value >= 0.45f && strengthSlider.value <= 0.55f)
            {
                fillImage.color = maxFillColor;
            }
        }
            
    }

    public void Strike()
    {
        isHit = true;
        // instructionsText.SetActive(false);
        // reloadText.SetActive(true);
    }

    public float GetHitValue()
    {
        float strengthValue = 0f;
        // isHit = true;

        
        if((strengthSlider.value >= 0f && strengthSlider.value < 0.45f))
        {
            // To get a perfect interpolation from 0 to 1 but it becomes difficult to distinguish it from the best hit
            //strengthValue = strengthSlider.value / 0.4f;

            strengthValue = strengthSlider.value * 2;
        }
        else if((strengthSlider.value > 0.55f && strengthSlider.value <= 1.0f))
        {
            // To get a perfect interpolation from 0 to 1 but it becomes difficult to distinguish it from the best hit
            // strengthValue = (1 - strengthSlider.value) / 0.4f;

            strengthValue = (1 - strengthSlider.value) * 2;
        }
        else
        {
            strengthValue = 1;
        }

        // Round values to first decimal place
        return Mathf.Round(strengthValue * 10f) / 10f;

        // Get the exact value of strength
        // return strengthValue;
    }

    public void ResetSlider()
    {
        isHit = false;
        timeSinceNotHit = Time.time;
        // instructionsText.SetActive(true);
        // reloadText.SetActive(false);
    }
}
