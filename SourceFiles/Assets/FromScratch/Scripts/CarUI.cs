using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarUI : MonoBehaviour
{
    [SerializeField] CanvasGroup playerUI;

    private void OnEnable()
    {
        playerUI.alpha = 0f;
        playerUI.interactable =false;
    }
    private void OnDisable()
    {
        playerUI.alpha = 1f;
        playerUI.interactable = true;
    }
}
