using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    [SerializeField] GameObject pc_tutorial;
    [SerializeField] GameObject mobile_tutorial;
    
    private void OnEnable()
    {
        if (ControlSwitcher.Instance.isMobileControls)
        {
            pc_tutorial.SetActive(false);
            mobile_tutorial.SetActive(true);
        }
        else
        {
            pc_tutorial.SetActive(true);
            mobile_tutorial.SetActive(false);
        }
    }
}
