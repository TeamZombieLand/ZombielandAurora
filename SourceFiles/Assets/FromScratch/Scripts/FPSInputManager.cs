using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInputManager : MonoBehaviour
{
    
    private FPSActionsAsset fpsActionAsset;
    // Start is called before the first frame update
    private void Awake()
    {
        fpsActionAsset = new FPSActionsAsset();
    }
    private void OnEnable()
    {
        fpsActionAsset.Enable();
    }
    private void OnDisable()
    {
        fpsActionAsset.Disable();
    }


    #region Handle Input
    public Vector2 GetPlayerMovement()
    {
        if (ControlSwitcher.Instance.isMobileControls)
        {
            return new Vector2(CommonReferences.Instance.playerMovementJoystick.Horizontal, CommonReferences.Instance.playerMovementJoystick.Vertical);
        }
        else
        {
            return fpsActionAsset.Player.Movement.ReadValue<Vector2>();
        }
    }
    public Vector2 GetMouseDelta()
    {
        if (ControlSwitcher.Instance.isMobileControls)
        {
            Vector2 amount = (CommonReferences.Instance.fixedTouchField.TouchDist);

            return amount;
        }
        else
        {
            return fpsActionAsset.Player.Look.ReadValue<Vector2>();
        }
       
    }
    public bool PlayerJumpedThisFrame()
    {
        if (ControlSwitcher.Instance.isMobileControls)
        {
            if (CommonReferences.Instance.isPressedJumped)
            {
                CommonReferences.Instance.isPressedJumped = false;
                return true;
            }
            return false;
        }
        else
        {
            return fpsActionAsset.Player.Jump.triggered;
        }

        
    }
    public bool Reloaded()
    {
        if (ControlSwitcher.Instance.isMobileControls)
        {
            if (CommonReferences.Instance.isPressedReload)
            {
                CommonReferences.Instance.isPressedReload = false;
                return true;
            }
        }


        return fpsActionAsset.Player.Reload.triggered;
    }
    public bool ISSpriting()
    {
        return fpsActionAsset.Player.Sprint.IsPressed();
    }
    public bool ShootPressed()
    {
        return fpsActionAsset.Player.Shoot.WasPressedThisFrame();
    }
    public void FireCancelled()
    {
        CommonReferences.Instance.isFireButtonReleased = true;
    }
    public bool ShootCancelled()
    {
        if (ControlSwitcher.Instance.isMobileControls)
        {
            if (!IsShooting()) return true;
            if (CommonReferences.Instance.isFireButtonReleased)
            {
                CommonReferences.Instance.isFireButtonReleased = false;
                return true;
            }
            return false;
        }
        return fpsActionAsset.Player.Shoot.WasReleasedThisFrame();
    }


    public bool Power1Pressed()
    {
        return fpsActionAsset.Player.Power1.WasPressedThisFrame();
    }
    public bool Power2Pressed()
    {
        return fpsActionAsset.Player.Power2.WasPressedThisFrame();
    }
    public bool Power3Pressed()
    {
        return fpsActionAsset.Player.Power3.WasPressedThisFrame();
    }


    public bool IsShooting()
    {
        if (ControlSwitcher.Instance.isMobileControls)
        {
            return CommonReferences.Instance.isFiring;
        }
        return fpsActionAsset.Player.Shoot.IsPressed();
    }

    public float GetMouseScroll()
    {
        return fpsActionAsset.Player.WeaponChange.ReadValue<float>();
    }

    public bool LocatedCar()
    {
        return fpsActionAsset.Player.LocateCar.triggered;
    }

    internal bool GetNextWeapon()
    {
        return fpsActionAsset.Player.NextWeapon.WasPressedThisFrame();
    }

    internal bool GetPreviouWeepon()
    {
        return fpsActionAsset.Player.PrevWeapon.WasReleasedThisFrame();
    }

    public bool GetAimButton()
    {
        return fpsActionAsset.Player.Aim.IsPressed();
    }

    public bool Paused()
    {
        return fpsActionAsset.Player.Escape.triggered;
    }

    internal bool GetFButton()
    {
        if (ControlSwitcher.Instance.isMobileControls)
        {
            if (CommonReferences.Instance.isPressedEnterCarBTN)
            {
                CommonReferences.Instance.isPressedEnterCarBTN = false;
                return true;
            }
            return false;
        }

        return fpsActionAsset.Player.F.triggered;
    }

    #endregion
}
