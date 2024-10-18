using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoSystem : MonoBehaviour
{


    [SerializeField] WeaponSystem wepon_system;
    [SerializeField] Transform UiParent;
    [SerializeField] GameObject GunInfoPrefab;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] List<GunUIInfo> gunInfoList = new List<GunUIInfo>();
    private int onWeapoonChange;

    private void OnEnable()
    {
        WeaponSystem.OnWeaponChanged += UpdateSelectedUI;
        Weapon.OnAmmoChanged+= UpdateUiInfo;
    }
    private void OnDisable()
    {
        WeaponSystem.OnWeaponChanged -= UpdateSelectedUI;
        Weapon.OnAmmoChanged -= UpdateUiInfo;
    }

    public void GenerateWeaponUI(List<GameObject> weapons)
    {
        gunInfoList.Clear();
        if (UiParent.childCount > 0)
        {
            for (int i = UiParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(UiParent.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < weapons.Count; i++)
        {
            int temp = i;

            GameObject go = Instantiate(GunInfoPrefab,UiParent);
            GunUIInfo uiInfo = new GunUIInfo();
            uiInfo.gunAmmoInfo = go.transform.GetChild(0).GetComponent<TMP_Text>();
            uiInfo.uiObject = go;
            uiInfo.weapon = weapons[i].GetComponent<Weapon>();

            uiInfo.SetAmmoInfo(uiInfo.weapon.currentAmmo, uiInfo.weapon.totalAmmo);            
            go.transform.GetChild(1).GetComponent<Image>().sprite= uiInfo.weapon.weaponImage;
            Debug.Log(uiInfo.weapon.weaponImage.name);
            uiInfo.index = i;

            go.GetComponent<Button>().onClick.AddListener(() => {

                wepon_system.SetActiveWeapon(temp);

            });


            go.transform.GetComponent<CanvasGroup>().alpha = 0.5f;
            gunInfoList.Add(uiInfo);
        }        
    }
    public void UpdateSelectedUI()
    {
        if(wepon_system==null)
        {
            if (CommonReferences.Instance.weaponSystem == null)
            {
                wepon_system = CommonReferences.Instance.staticWeaponSystem;
            }
            else
            {
                wepon_system = CommonReferences.Instance.weaponSystem;
            }
        }

        int selected_index = 0;
        Debug.Log("Update Weapon UI");
        for (int i = 0; i < gunInfoList.Count; i++)
        {
            if(gunInfoList[i].weapon == wepon_system.current_weapon)
            {
                selected_index = i;
                if (gunInfoList[i].uiObject.GetComponent<RectTransform>().localScale == Vector3.one)
                {
                    LeanTween.scale(gunInfoList[i].uiObject.GetComponent<RectTransform>(), Vector3.one * 1.12f, 0.15f).setEase(LeanTweenType.easeInQuad);
                    LeanTween.alphaCanvas(gunInfoList[i].uiObject.GetComponent<CanvasGroup>(), 1, 0.15f).setEase(LeanTweenType.easeInQuad);
                }
            }
            else
            {
                if (gunInfoList[i].uiObject.GetComponent<RectTransform>().localScale != Vector3.one)
                {
                    LeanTween.scale(gunInfoList[i].uiObject.GetComponent<RectTransform>(), Vector3.one, 0.15f).setEase(LeanTweenType.easeOutQuad);
                    LeanTween.alphaCanvas(gunInfoList[i].uiObject.GetComponent<CanvasGroup>(), 0.25f, 0.15f).setEase(LeanTweenType.easeInQuad);
                }
            }
        }

        LeanTween.value(scrollRect.verticalNormalizedPosition,1- ((float)selected_index / (float)gunInfoList.Count), 0.15f).setOnUpdate((float v) =>
        {
            scrollRect.verticalNormalizedPosition = v;
        });
       
        
    }

    public void UpdateUiInfo(Weapon weapon)
    {
       
        GunUIInfo uiInfo = gunInfoList.Find(x => x.weapon == weapon);
        if (uiInfo != null)
        {
            uiInfo.SetAmmoInfo(weapon.currentAmmo, weapon.totalAmmo);
        }
    }

}
[System.Serializable]
public class GunUIInfo
{
    public GameObject uiObject;
    public TMP_Text gunAmmoInfo;    
    public bool selected = false;
    public Weapon weapon;
    public int index;

    public void SetAmmoInfo(int currentAmmo,int totalAmmo)
    {

        if (weapon.weapon_index == 0)
        {
            gunAmmoInfo.text = (currentAmmo == 0) ? ("<color=red>" + currentAmmo + "</color>/" + "X") :
                                                    (currentAmmo + "/" + "X");
        }
        else
        {
            gunAmmoInfo.text = (currentAmmo == 0) ? ("<color=red>" + currentAmmo + "</color>/" + ((totalAmmo == 0) ? ("<color=red>" + totalAmmo + "</color>") : (totalAmmo.ToString()))) :
                                                    (currentAmmo + "/" + ((totalAmmo == 0) ? ("<color=red>" + totalAmmo + "</color>") : (totalAmmo.ToString())));
        }
    }
    
}
