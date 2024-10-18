using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsInfo : MonoBehaviour
{
    #region Singleton
    public static PlayerWeaponsInfo Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }        
    }
    #endregion

    public List<WeaponDetails> all_weapon_details=new List<WeaponDetails>();
    public WeaponInfoSystem weaponInfoSystem;
    public WeaponSystem weaponSystem;
    public List<WeaponDetails> defaultWeaponData = new List<WeaponDetails>();

    [SerializeField] bool generatedUI = false;
       

    public void SetWeaponsData(bool firstTime=false)
    {
        LocalData data= DatabaseManager.Instance.GetLocalData();
        if (firstTime) { data.weaponDetails = PlayerWeaponsInfo.Instance.defaultWeaponData; }

       

        

        for (int i = 0; i < data.weaponDetails.Count; i++)
        {

            PlayerWeaponsInfo.Instance.all_weapon_details[i].weaponInfo = data.weaponDetails[i].weaponInfo;
            
        }
        
       

        DatabaseManager.Instance.UpdateData(data);


    }


}

