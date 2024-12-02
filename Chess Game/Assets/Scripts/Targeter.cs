using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public Renderer renderer;
    private Color originalColor;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        // Store the original material color
        if (renderer != null && renderer.material != null)
        {
            originalColor = renderer.material.color; //this is where we get the material of the game object
        }
    }

    private void OnMouseEnter()
    {
        if (renderer != null && renderer.material != null)
        {
            renderer.material.color = Color.green;
        }
    }   
    private void OnMouseExit()
    {
        if (renderer != null && renderer.material != null)
        {
            renderer.material.color = originalColor;
        }
    }
}
