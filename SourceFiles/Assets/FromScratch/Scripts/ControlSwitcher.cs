using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSwitcher : MonoBehaviour
{
    #region Singleton
    public static ControlSwitcher Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public bool isMobileControls = false;


}
