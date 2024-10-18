using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCursonOnUI : MonoBehaviour
{
    public bool keepEnableOnActive;
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (keepEnableOnActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
