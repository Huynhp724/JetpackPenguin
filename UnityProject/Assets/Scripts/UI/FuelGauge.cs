using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelGauge : MonoBehaviour
{
    [SerializeField] Image fill;

    public void OnValueChange(float value)
    {
        float r = 1 - value;
        float g = value;

        Color fillColor = new Color(r, g, .1f);
        print("Changing color to: " + fillColor);
        fill.color = fillColor;
    }
}
