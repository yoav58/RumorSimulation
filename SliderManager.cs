using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SliderManager : MonoBehaviour
{
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Slider slider4;
    public float[] stateProbalities;
    private void Start()
    {
        slider1.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        slider2.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        slider3.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        slider4.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    private void OnSliderValueChanged()
    {
        float totalValue = slider1.value + slider2.value + slider3.value + slider4.value;
        if (totalValue > 100f)
        {
            float overage = totalValue - 100f;
            float subtractValue = overage / 4f;
            slider1.value -= subtractValue;
            slider2.value -= subtractValue;
            slider3.value -= subtractValue;
            slider4.value -= subtractValue;
        }
        
    }
    /*********************************************************************
    * Function Name: getStateByUserProbality
    * Description: since this class has all the
     * probabilities of each state, this method responsible for return 
     * a state with the probabilities the user choose.
    *********************************************************************/  
    public int getStateByUserProbality()
    {
       // if (stateProbalities == null || stateProbalities.Length == 0)
        stateProbalities = new float[4] { slider1.value, slider2.value, slider3.value, slider4.value };
        Array.Sort(stateProbalities);
        float firstValue = slider1.value;
        float secondValue = slider2.value;
        float thirdValue = slider3.value;
        float fourthValue = slider4.value;
        float randomNumber = Random.Range(0f, 100f);
        // using cumulative probability.
        float sum = 0f;
        for (int i = 0; i < 4; i++)
        {
            sum += stateProbalities[i];
            if (randomNumber < sum)
            {
                if (stateProbalities[i] == firstValue) return 1;
                if (stateProbalities[i] == secondValue) return 2;
                if (stateProbalities[i] == thirdValue) return 3;
                if (stateProbalities[i] == fourthValue) return 4;
                
            }
        }

        return 1;
    }
    
}
