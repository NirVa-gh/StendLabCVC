using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TriggerMessage : MonoBehaviour
{

    public Material material;
    public Color originalColor;
    public Color hoverColor;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        originalColor = material.GetColor("_EmissionColor");
    }

    void OnMouseEnter()
    {
        material.SetColor("_EmissionColor", hoverColor);
    }

    void OnMouseExit()
    {
        material.SetColor("_EmissionColor", originalColor);
    }
}

