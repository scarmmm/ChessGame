using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public Renderer renderer;
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    private void OnMouseEnter()
    {
        renderer.material.color = Color.green;
    }   
    private void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
}
