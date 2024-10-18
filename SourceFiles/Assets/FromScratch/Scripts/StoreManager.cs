using Defective.JSON;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager insta;
    [SerializeField] GameObject itemPanelUI;
    [SerializeField] GameObject CoinPanel;
    [SerializeField] GameObject WeaponPanel;
    [SerializeField] GameObject itemPurchaseUI;
    [SerializeField] GameObject LoadingImage;
    [SerializeField] GameObject ShopUI;
   // [SerializeField] GameObject[] ThemesItems;
    [SerializeField] GameObject ThemeSection;

    //item panel stuff
    //[SerializeField] Button[] itemButtons;


    //purcahse panel stuff
    [SerializeField] RawImage purchaseItemImg;
    [SerializeField] TMP_Text purchaseItemText;
    [SerializeField] TMP_Text purchaseItemCostText;

    [SerializeField] TMP_Text balanceText;
    [SerializeField] TMP_Text balanceTokenText;

    int currentSelectedItem = -1;

    [SerializeField] Transform itemParent;
    //item panel stuff
    [SerializeField] GameObject itemButtonPrefab;


    [SerializeField] GameObject prev_BTN;
    [SerializeField] GameObject next_BTN;


    public bool coinShop {get; set;  }

    private void Awake()
    {
        insta = this;

    }


    public void SetBalanceText()
    {
        balanceText.text = "Balance : " + CoreChainManager.userBalance.ToString();
        balanceTokenText.text = "Token : " + CoreChainManager.userTokenBalance.ToString();
    }


    [SerializeField] List<int> nonAvailableWeapons = new List<int>();
    int selectedWeaponinUI=0;
    [SerializeField] GameObject[] all_overlay_weapons;
    [SerializeField] GameObject weapon3DObject;

    [Header("Coin Texts")]
    [SerializeField] TMP_Text Txt_mycoins;
    [SerializeField] TMP_Text Txt_weaponcost;

    private void OnEnable()
    {
        CoinPanel.SetActive(coinShop);
        WeaponPanel.SetActive(!coinShop);
        selectedWeaponinUI = 0;
       // ClosePurchasePanel();

        balanceText.text = "Balance : " + CoreChainManager.userBalance.ToString();

        if (!coinShop)
        {
            weapon3DObject.SetActive(true);
            DisableOwnedItems();
        }
        else
        {
            setCoinPrice();
            //CoreChainManager.Instance.CheckUserBalance();
        }

        SetPlayerCoins();

        SetMaxHPINfo();

        SetBalanceText();
    }
    
        
        
    
    private void OnDisable()
    {
        weapon3DObject.SetActive(false);
    }
    [SerializeField] TMP_Text[] coin_prices;
    private void setCoinPrice()
    {
        for (int i = 0; i < coin_prices.Length; i++)
        {
            coin_prices[i].text = "ETH: " + CoreChainManager.coinCost[i].ToString("F5");
        }
    }

    public int lastSelectedButton = -1;
    public void DisaleLastSelectedButton()
    {
        Debug.Log(lastSelectedButton + 1);

        if (nonAvailableWeapons.Contains(lastSelectedButton + 1))
        {
            nonAvailableWeapons.Remove(lastSelectedButton + 1);
            if (nonAvailableWeapons.Count > 0)
            {
                selectedWeaponinUI = 0;
                Enable3DWeapon(nonAvailableWeapons[0]);
            }
            else
            {
                MessaeBox.insta.showMsg("All Weapons Purchased", true);
            }
        }
    }

    [SerializeField] Toggle[] toggles;
    [SerializeField] GameObject[] sections;
    public void ToggleValueChanged()
    {
        foreach (GameObject item in sections)
        {
            item.SetActive(false);
        }

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                sections[i].SetActive(true);
                break;
            }
        }
    }
    async public void DisableOwnedItems()
    {
        LoadingImage.SetActive(true);
        ShopUI.SetActive(false);
        await CoreChainManager.Instance.CheckPuzzleList();

        List<string> temp_list = CoreChainManager.Instance.nftList;

        nonAvailableWeapons = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
        selectedWeaponinUI = 0;

        if (CoreChainManager.Instance.nftList.Count > 0)
        {
            //JSONObject jsonObject = new JSONObject(result);
            for (int i = 0; i < temp_list.Count; i++)
            {
                if (temp_list[i].StartsWith("50") && temp_list[i].Length == 3)
                {
                   // ThemesItems[Int32.Parse(temp_list[i]) - 500].SetActive(false);

                    nonAvailableWeapons.Remove(Int32.Parse(temp_list[i]) - 499);
                }
            }


            for (int i = 0; i < nonAvailableWeapons.Count; i++)
            {
                Debug.Log("Not Purchased" + nonAvailableWeapons[i]);
            }
            bool all_purchased = true;
            /*for (int i = 0; i < ThemesItems.Length; i++)
            {
                if (ThemesItems[i].activeSelf)
                {
                    all_purchased = false;
                }
            }
            if (all_purchased)
            {
                ThemeSection.SetActive(false);
            }*/
        }

        prev_BTN.SetActive(false);
        if (nonAvailableWeapons.Count > 1)
        {            
            next_BTN.SetActive(true);
        }
        if (nonAvailableWeapons.Count > 0)
        {
            selectedWeaponinUI = 0;
            Enable3DWeapon(nonAvailableWeapons[0]);
        }


        LoadingImage.SetActive(false);
        ShopUI.SetActive(true);
    }
    #region Weapon info UI
   
    [SerializeField] SliderInfo[] all_slider;  //0-FireRate 1-Damage  2=Reload Time 3-Accuracy 
    [SerializeField] TMP_Text weapon_desc;

    void SetWeaponInfo(int weaponIndex)
    {
        List<WeaponDetails> weaponDetails=PlayerWeaponsInfo.Instance.defaultWeaponData;

        WeaponDetails w_details = weaponDetails.Find(x => x.weaponInfo.weaponIndex == weaponIndex);

        Debug.Log(w_details.name);
        weapon_desc.text = DatabaseManager.Instance.allMetaDataServer[weaponIndex-1].description;

        if (w_details == null) {
            for (int i = 0; i < all_slider.Length; i++)
            {
                all_slider[i].slider.gameObject.SetActive(false);
            }
            return; 
        }


        for (int i = 0; i < all_slider.Length; i++)
        {
            all_slider[i].slider.gameObject.SetActive(true);

            switch (i)
            {
                case 0:
                    {
                        if (w_details.weaponInfo.singleShot)
                        {
                            all_slider[0].slider.gameObject.SetActive(false);
                            break;
                        }

                        all_slider[i].slider.minValue = 15;
                        all_slider[i].slider.maxValue = w_details.weaponInfo.firerate_data.upgradeValues[w_details.weaponInfo.firerate_data.upgradeValues.Length - 1];


                        all_slider[i].min_value.text = "15";
                        all_slider[i].max_value.text = w_details.weaponInfo.firerate_data.upgradeValues[w_details.weaponInfo.firerate_data.upgradeValues.Length - 1].ToString();
                        all_slider[i].slider.value = w_details.weaponInfo.firerate_data.upgradeValues[0];

                        break;
                    }
                case 1:
                    {
                        all_slider[i].slider.minValue = 10;
                        all_slider[i].slider.maxValue = w_details.weaponInfo.damage_data.upgradeValues[w_details.weaponInfo.damage_data.upgradeValues.Length - 1];


                        all_slider[i].min_value.text = "10";
                        all_slider[i].max_value.text = w_details.weaponInfo.damage_data.upgradeValues[w_details.weaponInfo.damage_data.upgradeValues.Length - 1].ToString();
                        all_slider[i].slider.value = w_details.weaponInfo.damage_data.upgradeValues[0];

                        break;
                    }
                case 2:
                    {
                        all_slider[i].slider.maxValue = 6;
                        all_slider[i].slider.minValue = w_details.weaponInfo.reloadtime_data.upgradeValues[w_details.weaponInfo.reloadtime_data.upgradeValues.Length - 1];

                        all_slider[i].min_value.text = "6";
                        all_slider[i].max_value.text = w_details.weaponInfo.reloadtime_data.upgradeValues[w_details.weaponInfo.reloadtime_data.upgradeValues.Length - 1].ToString();
                        all_slider[i].slider.value = 3- w_details.weaponInfo.reloadtime_data.upgradeValues[0];
                        break;
                    }
                case 3:
                    {
                        if (w_details.weaponInfo.singleShot)
                        {
                            all_slider[3].slider.gameObject.SetActive(false);
                            break;
                        }

                        all_slider[i].slider.maxValue = w_details.weaponInfo.accuracy_data.upgradeValues[w_details.weaponInfo.accuracy_data.upgradeValues.Length - 1];
                        all_slider[i].slider.minValue = 40 ;

                        all_slider[i].min_value.text = "40";

                        all_slider[i].max_value.text = w_details.weaponInfo.accuracy_data.upgradeValues[w_details.weaponInfo.accuracy_data.upgradeValues.Length - 1].ToString();
                        all_slider[i].slider.value = w_details.weaponInfo.accuracy_data.upgradeValues[0];
                        break;
                    }
            }
            
            


        }
    }
    #endregion

    #region Player HP BUy
    [SerializeField] GameObject HP_Object;
    [SerializeField] TMP_Text current_hp_text;

    public void SetMaxHPINfo()
    {
        LocalData data = DatabaseManager.Instance.GetLocalData();

        HP_Object.GetComponent<Button>().interactable = data.max_hp < 5000;
        if (HP_Object.GetComponent<Button>().interactable)
        {
            HP_Object.transform.GetChild(0).GetComponent<TMP_Text>().color = new Color(88f / 255f, 12f / 255f, 12f / 255f, 1f);
            HP_Object.transform.GetChild(0).GetComponent<TMP_Text>().text = "Increase HP";
            HP_Object.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            HP_Object.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
            HP_Object.transform.GetChild(0).GetComponent<TMP_Text>().text = "Max HP";
            HP_Object.transform.GetChild(1).gameObject.SetActive(false);
        }
        current_hp_text.text = data.max_hp.ToString();
    }
    public void IncreaseMAX_HP()
    {
        LocalData data = DatabaseManager.Instance.GetLocalData();

        if (data.coins >= 5000)
        {
            data.coins -= 5000;
            data.max_hp += 100;
            SetMaxHPINfo();
            DatabaseManager.Instance.UpdateData(data);
        }
        else
        {
            MessaeBox.insta.showMsg("Not Enough Coins", true);
        }
    }
    #endregion

    #region UI Mathods
    public void NextWeapon()
    {

        if (selectedWeaponinUI >= nonAvailableWeapons.Count - 1) return;

        selectedWeaponinUI++;
        if (selectedWeaponinUI == nonAvailableWeapons.Count - 1) next_BTN.SetActive(false);


        Enable3DWeapon(nonAvailableWeapons[selectedWeaponinUI]);
        prev_BTN.SetActive(true);

        Debug.LogWarning("Change Weapon Stats UI HERE");

    }
    public void PreviousWeapon()
    {
        if (selectedWeaponinUI == 0) return;

        selectedWeaponinUI--;

        if (selectedWeaponinUI == 0) prev_BTN.SetActive(false);

        next_BTN.SetActive(true);
        Enable3DWeapon(nonAvailableWeapons[selectedWeaponinUI]);
        Debug.LogWarning("Change Weapon Stats UI HERE");
    }

    void Enable3DWeapon(int index)
    {
        for (int i = 0; i < all_overlay_weapons.Length; i++)
        {
            all_overlay_weapons[i].SetActive(false);
        }

        all_overlay_weapons[index].SetActive(true);
        SetWeaponInfo(index);

        SetCostText();
    }

    public void SelectItem(int _no, Texture _texture)
    {
        Debug.Log("Selected item " + _no);
        currentSelectedItem = _no;
        itemPanelUI.SetActive(false);
        itemPurchaseUI.SetActive(true);
        purchaseItemImg.texture = _texture;// itemButtons[_no].GetComponent<RawImage>().texture;
        purchaseItemText.text = DatabaseManager.Instance.allMetaDataServer[_no].description;
        purchaseItemCostText.text = DatabaseManager.Instance.allMetaDataServer[_no].cost.ToString();


    }
    public void DeductCoins(int amount)
    {
        LocalData data = DatabaseManager.Instance.GetLocalData();
        data.coins -= amount;
        DatabaseManager.Instance.UpdateData(data);
        SetPlayerCoins();
    }

    void SetCostText()
    {
        Txt_weaponcost.text = DatabaseManager.Instance.allMetaDataServer[nonAvailableWeapons[selectedWeaponinUI] - 1].cost.ToString();
    }
    public void SetPlayerCoins()
    {
        //Txt_mycoins.text= DatabaseManager.Instance.GetLocalData().coins.ToString();
        UIManager.insta.SetCoinsText();
    }

    public void purchaseCoins(int index)
    {
        CoreChainManager.Instance.CoinBuyOnSendContract(index);
    }
    public void purchaseItem()
    {
        Debug.Log("purchaseItem");
        LocalData data = DatabaseManager.Instance.GetLocalData();
      
        MetadataNFT meta = new MetadataNFT();
       

        lastSelectedButton = nonAvailableWeapons[selectedWeaponinUI] - 1;

        if (data.coins >= DatabaseManager.Instance.allMetaDataServer[lastSelectedButton].cost)
        {
            
            meta.itemid = DatabaseManager.Instance.allMetaDataServer[lastSelectedButton].itemid;
            meta.name = DatabaseManager.Instance.allMetaDataServer[lastSelectedButton].name;
            meta.description = DatabaseManager.Instance.allMetaDataServer[lastSelectedButton].description;
            meta.image = DatabaseManager.Instance.allMetaDataServer[lastSelectedButton].imageurl;
            //meta.itemid = SingletonDataManager.metanftlocalData[currentSelectedItem].
            // NFTPurchaser.insta.StartCoroutine(NFTPurchaser.insta.UploadNFTMetadata(Newtonsoft.Json.JsonConvert.SerializeObject(meta), SingletonDataManager.metanftlocalData[currentSelectedItem].cost, SingletonDataManager.metanftlocalData[currentSelectedItem].itemid));
            CoreChainManager.Instance.purchaseItem(lastSelectedButton, false);
            /*data.coins -= DatabaseManager.Instance.allMetaDataServer[currentSelectedItem].cost;
            DatabaseManager.Instance.UpdateData(data);
            UIManager.insta.UpdatePlayerUIData(true,data);*/
        }
        else
        {
            Debug.Log("not enough money");
            MessaeBox.insta.showMsg("No enough coins\nKill zombies to earn coins", true);
        }
    }


    public void ClosePurchasePanel()
    {
        itemPanelUI.SetActive(true);
        itemPurchaseUI.SetActive(false);

    }

    public void CloseItemPanel()
    {
        itemPanelUI.SetActive(false);
        itemPurchaseUI.SetActive(false);
        Debug.Log("close");
        ///if (!CovalentManager.loadingData) CovalentManager.insta.GetNFTUserBalance();
        /*foreach (Transform child in itemParent)
        {
            Destroy(child.gameObject);
        }*/
        gameObject.SetActive(false);
    }
    #endregion
    [SerializeField] TMP_Text text_infomsg;
    [SerializeField] GameObject infoPopup;
  
    public void ChangeInfoText(string msg)
    {
        text_infomsg.text = msg;
    }
    public void ShowInfoStore(RectTransform rt)
    {

        infoPopup.transform.position = rt.position + new Vector3(200, 50, 0);
        infoPopup.SetActive(true);
    }
    public void HidePopup()
    {
        infoPopup.SetActive(false);
    }

    public void ExchangeToken(int _value) {
        CoreChainManager.Instance.ExchangeToken(_value);
    }


    private void Update()
    {
        if (rotating)
        {
            Vector3 delta = Input.mousePosition - prevPos;
            if (Vector3.Dot(weaponRotater.up, Vector3.up) >= 0)
            {
                weaponRotater.Rotate(weaponRotater.up, -Vector3.Dot(delta, weaponCameraOverlay.right), Space.World);
            }
            else
            {
                weaponRotater.Rotate(weaponRotater.up, Vector3.Dot(delta, weaponCameraOverlay.right), Space.World);
            }

            weaponRotater.Rotate(weaponCameraOverlay.right, Vector3.Dot(delta, weaponCameraOverlay.up), Space.World);

            prevPos = Input.mousePosition;
        }
    }

    #region On Drag Events

    public Transform weaponRotater;
    public Transform weaponCameraOverlay;
    private bool rotating = false;
    Vector3 mousePos;
    Vector3 prevPos;
    public void BeginDrag()
    {
        prevPos = Input.mousePosition;
        rotating = true;
    }
    public void StopDrag()
    {
        rotating = false;
    }
    #endregion

}

[System.Serializable]
public class SliderInfo
{
    public Slider slider;
    public TMP_Text min_value;
    public TMP_Text max_value;
    public Button upgradeBTN;
    public TMP_Text upgrade_text;
}