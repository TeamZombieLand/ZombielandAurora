using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class DailyRewardManager : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject errorPanel;
    [SerializeField] GameObject collectPanel;


    [SerializeField] Transform[] all7DaysItems;

    bool firstTime = true;
    private async void OnEnable()
    {
        loadingPanel.SetActive(true);
        collectPanel.SetActive(false);
        errorPanel.SetActive(false);

        long currentTime =  CoreChainManager.Instance.GetCurrentTime();
        string _lastClaimedString = DatabaseManager.Instance.GetLocalData().lastClaimedTime;
        long lastClaimedTime;
        bool parsedSuccess = long.TryParse(_lastClaimedString, out lastClaimedTime);

        Debug.Log("PARSED " + parsedSuccess);
        bool enableOnStart = false;

    
        if (!parsedSuccess ||  string.IsNullOrEmpty(_lastClaimedString) || _lastClaimedString == "0")
        {

            Debug.Log("SETTING HERE 2");
            loadingPanel.SetActive(false);
            collectPanel.SetActive(true);
            SetCollectable(DatabaseManager.Instance.GetLocalData().dayLoginStreak,true);
            enableOnStart = true;

            if (firstTime)
            {
                firstTime = false;
                if (enableOnStart)
                {
                    Debug.Log("SETTING HERE");
                    this.GetComponent<CanvasGroup>().alpha = 1f;
                    this.GetComponent<CanvasGroup>().interactable = true;
                    this.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }

            return;
        }


        Debug.Log("LAST LOGIN Time: " + lastClaimedTime);
        Debug.Log("Current LOGIN Time: " + currentTime);


        long difference = currentTime - lastClaimedTime;





        if (difference >= 86400 && difference < (86400 *2))
        {
            Debug.Log("CAN CLAIM");
            SetCollectable(DatabaseManager.Instance.GetLocalData().dayLoginStreak, true);
            enableOnStart = true;
        }
        if (difference >= (86400 * 2))
        {
            Debug.Log("RESET STREAK");
            DatabaseManager.Instance.GetLocalData().dayLoginStreak = 0;
            DatabaseManager.Instance.UpdateData();
            SetCollectable(0, true);
            enableOnStart = true;
        }
        if (difference < 86400)
        {
            Debug.Log("CANNOT CLAIM YET");
            SetCollectable(DatabaseManager.Instance.GetLocalData().dayLoginStreak, false);
            enableOnStart = false;
        }
        loadingPanel.SetActive(false);
        collectPanel.SetActive(true);

        if (firstTime)
        {
            firstTime = false;
            if (enableOnStart)
            {
                this.GetComponent<CanvasGroup>().alpha = 1f;
                this.GetComponent<CanvasGroup>().interactable = true;
                this.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            this.GetComponent<CanvasGroup>().alpha = 1f;
            this.GetComponent<CanvasGroup>().interactable  = true;
            this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        
    }

    private void SetCollectable(int dayLoginStreak, bool _canClaim)
    {
        for (int i = 0; i < all7DaysItems.Length; i++)
        {
            all7DaysItems[i].GetChild(3).gameObject.SetActive(false);
            all7DaysItems[i].GetComponent<CanvasGroup>().alpha = 1f;
            all7DaysItems[i].GetChild(4).gameObject.SetActive(false);
        }

        for (int i = 0; i < dayLoginStreak; i++)
        {
            all7DaysItems[i].GetChild(4).gameObject.SetActive(true);
        }

        if (_canClaim)
        {
            all7DaysItems[dayLoginStreak].GetChild(3).gameObject.SetActive(true);
            for (int i = dayLoginStreak + 1; i < all7DaysItems.Length; i++)
            {
                all7DaysItems[i].GetComponent<CanvasGroup>().alpha = 0.5f;
            }

        }

        else
        {
            for (int i = dayLoginStreak ; i < all7DaysItems.Length; i++)
            {
                all7DaysItems[i].GetComponent<CanvasGroup>().alpha = 0.5f;
            }

        }

     
    }

    public void ClaimGift(int _dayIndex)
    {
        
        DatabaseManager.Instance.GetLocalData().dayLoginStreak = (DatabaseManager.Instance.GetLocalData().dayLoginStreak + 1) % 7;


       

        switch (_dayIndex)
        {
            case 0:
                {
                    DatabaseManager.Instance.GetLocalData().coins += 500;                    
                    break;
                }
            case 1:
                {
                    DatabaseManager.Instance.GetLocalData().power_freezeZombies += 2;
                    break;
                }
            case 2:
                {
                    DatabaseManager.Instance.GetLocalData().coins += 1500;
                    break;
                }
            case 3:
                {
                    DatabaseManager.Instance.GetLocalData().power_invicible += 5;
                    break;
                }
            case 4:
                {
                    DatabaseManager.Instance.GetLocalData().power_speedbooster += 5;
                    break;
                }
            case 5:
                {
                    DatabaseManager.Instance.GetLocalData().coins += 5000;
                    break;
                }
            case 6:
                {
                    CoreChainManager.Instance.getDailyToken();
                    break;
                }
        }

        CoreChainManager.Instance.SaveNewClaimTime();
        DatabaseManager.Instance.UpdateData();
        SetCollectable(DatabaseManager.Instance.GetLocalData().dayLoginStreak, false);
        AudioManager.insta?.PlayDailyCollect();
    }


}
