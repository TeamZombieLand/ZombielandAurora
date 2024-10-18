using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthInfo : MonoBehaviour
{
    #region singleton
    public static PlayerHealthInfo Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    [SerializeField] Image health_fillbar;

    [Header("Health Effect UI")]
    [SerializeField] CanvasGroup HitFadeObject;
    [SerializeField] Image FadeImage;
    [SerializeField] float effectTimer;
    [SerializeField] Color red_color;
    [SerializeField] Color green_color;
    private void OnEnable()
    {
        Health.OnPlayerHealthChange += onPlayerHealthChanged; 
    }
    private void OnDisable()
    {
        Health.OnPlayerHealthChange -= onPlayerHealthChanged;
    }

    private void Start()
    {
        health_fillbar.fillAmount = 1;
    }
    int id = -1;
    public void ResetHealthSlider()
    {
        health_fillbar.fillAmount = 1;
    }
    private void onPlayerHealthChanged(Health health,bool regainHealth)
    {
        if (id != -1)
        {
            LeanTween.cancel(id);
        }

        id=LeanTween.value(health_fillbar.fillAmount, health.currentHealth / health.maxHealth, 0.1f).setOnUpdate((value)=> {
            health_fillbar.fillAmount = value;
        }).id;

        LeanTween.cancel(HitFadeObject.gameObject);
        if (regainHealth)
        {
            FadeImage.color = green_color;
            LeanTween.alphaCanvas(HitFadeObject, 1, effectTimer).setEase(LeanTweenType.easeInQuad).setFrom(0).setOnComplete(() =>
            {
                LeanTween.alphaCanvas(HitFadeObject, 0, effectTimer/2).setEase(LeanTweenType.easeOutQuad).setFrom(1).setDelay(effectTimer );
            });

        }
        else
        {
            FadeImage.color = red_color;
            LeanTween.alphaCanvas(HitFadeObject, 1, effectTimer).setEase(LeanTweenType.easeInQuad).setFrom(0).setOnComplete(() =>
            {
                LeanTween.alphaCanvas(HitFadeObject, 0, effectTimer / 2).setEase(LeanTweenType.easeOutQuad).setFrom(1).setDelay(effectTimer);
            });
        }
    }
}
