using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    private bool isAlive = false;
    private bool tempIsAlive = false;
    private Color prevColor;

    public bool IsAlive
    {
        get { return isAlive; }
        set
        {
            isAlive = value;
            GetComponent<Renderer>().material.color = prevColor = isAlive ? Color.green : Color.white;
        }
    }

    void OnMouseEnter()
    {
        prevColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.red;
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = prevColor;
    }

    void OnMouseDown()
    {
        IsAlive = !isAlive;
    }

    public void SetIsAlive(bool value)
    {
        tempIsAlive = value;
    }

    public void ApplyIsAlive()
    {
        IsAlive = tempIsAlive;
    }
}
