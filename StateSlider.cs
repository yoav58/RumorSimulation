using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateSlider : MonoBehaviour
{
    public TMP_Text text;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /***************************************************
    * Function Name: OnSliderChanget
    * Description: when the user change the sliddr,
     * this method update the text that show that
     * percentage.
    **************************************************/
    public void OnSliderChanget(float value)
    {
        text.text = value.ToString();

    }
}
