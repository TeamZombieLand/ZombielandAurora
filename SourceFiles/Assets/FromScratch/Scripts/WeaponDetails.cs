using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "WeaponDetail", menuName ="Weapons/Weapon Details")]
public class WeaponDetails : ScriptableObject
{
    public WeaponInfo weaponInfo;
    [System.Serializable]
    public class WeaponInfo
    {
        public int magCapacity;
        public int ammoCount;
        public Accuracy accuracy_data;
        public Damage damage_data;
        public FireRate firerate_data;          
        public ReloadTime reloadtime_data;
        public bool singleShot;
        public bool isBought;
        public int weaponIndex;
    }
    [System.Serializable]
    public class Accuracy :UpgradeInfo
    {
        public float accuracy;
    }
    [System.Serializable]
    public class FireRate : UpgradeInfo
    {
        public float fireRate;
    }

    [System.Serializable]
    public class Damage : UpgradeInfo
    {
        public float damageAmount;
    }
   
    [System.Serializable]
    public class ReloadTime : UpgradeInfo
    {
        public float reloadTime;
    }

    public abstract class UpgradeInfo
    {
        public int currentLevel;
        public int minLevel;
        public int maxLevel;
        public float[] upgradeValues;
        public int[] upgradeCosts;
    }
}

