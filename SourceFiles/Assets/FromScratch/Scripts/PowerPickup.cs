using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPickup : MonoBehaviour
{

    public bool isFuel;

    private void OnEnable()
    {
        LeanTween.rotateAround(this.gameObject,Vector3.up, 360, 40).setLoopClamp();
    }
    internal void Picked()
    {
        this.gameObject.SetActive(false);        
    }
}
