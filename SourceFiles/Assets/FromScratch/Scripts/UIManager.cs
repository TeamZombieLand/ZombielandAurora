using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class UIManager : MonoBehaviour
{
    public static UIManager insta;

    [Header("GameplayMenu")]
    public GameObject StartUI;
    public GameObject usernameUI;

    [SerializeField] GameObject LoadingPanel;
    [SerializeField] GameObject extraBTNS;

    public TMP_Text usernameText;

    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_Text statusText;

    [Header("No Coins Info")]
    [SerializeField] GameObject NoCoinsUI;


    public static string username;


    [Header("GameplayMenu")]
    public GameObject GameplayUI;
    [SerializeField] TMP_Text scoreTxt;


    [Header("StoreAndCollection")]
    [SerializeField] GameObject myCollectionUI;
    [SerializeField] TMP_Text TxtHeaderCollection;
    [SerializeField] GameObject LoadingMyCollection;
    [SerializeField] GameObject MyCollectionObject;




    [Header("Tutorial")]
    [SerializeField] GameObject TutorialUI;
    [SerializeField] int currentTutorial = 0;
    [SerializeField] GameObject[] tutorialObjects;

    [Header("On Play Game")]
    [SerializeField] GameObject gamePlayObjects;
    [SerializeField] GameObject[] environements;
    [SerializeField] Color[] fogColors;
    [SerializeField] Color[] ambientColors;
    [SerializeField] float[] fogAmount;
    [SerializeField] Material[] skyboxMaterials;
    [SerializeField] GameObject[] toDisableObjectsOnGameStart;
    [SerializeField] GameObject[] toEnableObjectsOnGameStart;

    [SerializeField] WeaponInfoSystem weaponInfoSystem;
    [SerializeField] WeaponSystem weaponSystem;

    [SerializeField] TMP_Text balanceTxt;
    [SerializeField] TMP_Text tokenBalTxt;
    [SerializeField] TMP_Text coinBalTxt;


    [Space]
    [Header("Invetory Collection UI")]
    [SerializeField] Transform collectionUIParent;
    [SerializeField] GameObject collectionUiItem;
    [SerializeField] Sprite[] itemSprites;


    #region Show Player Game Running Data
    [SerializeField] TMP_Text txt_survivalTime;
    [SerializeField] TMP_Text txt_score;
    [SerializeField] TMP_Text txt_zombieKilled;
    public void ShowPlayerData(float sur_time, int score)
    {
        txt_survivalTime.text = "Time : " + string.Format("{0:00}:{1:00}", (int)sur_time / 60, (int)sur_time % 60);
        txt_score.text = "Coins : " + score.ToString();
        txt_zombieKilled.text = "Zombies Killed : " + ScoreManager.Instance.zombiesKilled.ToString();
    }


    #endregion

    [SerializeField] LevelDetails currentLevelData;
    [SerializeField] TMP_Text text_levelTask;
    [SerializeField] TMP_Text text_currentTaskStatus;

    [SerializeField] Transform FindObjectParent;


    [SerializeField] TMP_Text _versionText;
    [SerializeField] TMP_Text _ChangeChainText;
    public bool isMainnet;

    [SerializeField] GameObject[] nonMobileObjects;

    public bool missionMode { get; set; }
    private void Awake()
    {
        insta = this;

    }
    // Start is called before the first frame update
    void Start()
    {

        if (ControlSwitcher.Instance.isMobileControls)
        {
            for (int i = 0; i < nonMobileObjects.Length; i++)
            {
                nonMobileObjects[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < nonMobileObjects.Length; i++)
            {
                nonMobileObjects[i].SetActive(true);
            }

        }

        if (isMainnet)
        {
            _versionText.text = "v1.00 Aurora Mainnet";
            _ChangeChainText.text = "Go To Aurora Testnet Version";

        }
        else
        {
            _versionText.text = "v1.00 Aurora Testnet";
            _ChangeChainText.text = "Go To Aurora Mainnet Version";

        }

        GameplayUI.SetActive(false);

        start_combo_pos = combo_obj.GetComponent<RectTransform>().anchoredPosition;

        if (PlayerPrefs.GetInt("init", 0) == 0)
        {
            PlayerPrefs.SetInt("init", 1);
            //  EditUserProfile();
        }

        UpdateBalance();

        for (int i = 0; i < environements.Length; i++)
        {
            environements[i].SetActive(false);
        }

        int levelIndex = 2;

        environements[levelIndex].SetActive(true);
        RenderSettings.skybox = skyboxMaterials[levelIndex];
        RenderSettings.fogColor = fogColors[levelIndex];
        RenderSettings.fogDensity = fogAmount[levelIndex];

    }

    public void AppDownload()
    {
        Application.OpenURL("https://zombieland.online/ZombieLand_V4_Mainnet.apk");
    }

    public void ChangeChain()
    {
        if (isMainnet)
        {
            Application.OpenURL("https://testnet.zombieland.online/");
        }
        else
        {
            Application.OpenURL("https://zombieland.online/");
        }
    }

    [Space]
    [Header("CAR UI")]
    [SerializeField] GameObject carUI;
    [SerializeField] GameObject locateCarBTN;
    [SerializeField] Image car_fuel_slider;
    [SerializeField] CarController carController;
    [SerializeField] Transform carMenuPoz;
    [SerializeField] Transform carStartPoz;


    [Header("Multiplayer UI")]
    [SerializeField] GameObject endlessMultiplayerUI;
    [SerializeField] GameObject private1v1MultiplayerUI;
    [SerializeField] TMP_InputField roomName_Input;

    private void OnEnable()
    {
        CommonReferences.OnPlayerStateChange += HandlePlayerStateChange;
        PlayerInventory.OnItemCollected += ItemCollected;
    }
    private void OnDisable()
    {
        CommonReferences.OnPlayerStateChange -= HandlePlayerStateChange;
        PlayerInventory.OnItemCollected -= ItemCollected;
    }



    private void HandlePlayerStateChange(PlayerState state)
    {
        if (state == PlayerState.CAR)
        {
            CommonReferences.Instance.marker.SetTarget(null);
            carUI.SetActive(true);
            locateCarBTN.SetActive(false);
        }
        else
        {
            CommonReferences.Instance.marker.SetTarget(null);
            carUI.SetActive(false);
            locateCarBTN.SetActive(true);
        }
    }

    Coroutine locateCoroutine;
    public void LocateCar()
    {
        CommonReferences.Instance.marker.SetTarget(CommonReferences.Instance.myCar.transform);
        if (locateCoroutine != null)
        {
            StopCoroutine(locateCoroutine);
        }
        locateCoroutine = StartCoroutine(disableMarker(5));
    }
    IEnumerator disableMarker(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        CommonReferences.Instance.marker.SetTarget(null);
    }


    private void LateUpdate()
    {
        if (carUI.activeInHierarchy)
        {
            car_fuel_slider.fillAmount = Mathf.Clamp(carController.carFuel / carController.maxFuel, 0, 1);
        }
    }
    public void UpdateBalance()
    {
        balanceTxt.text = "Balance : " + CoreChainManager.userBalance;
        tokenBalTxt.text = "Token : " + CoreChainManager.userTokenBalance;
        SetCoinsText();
    }

    public List<string> theme_list = new List<string>();


    async public void ShowMyThemes(bool _show)
    {
        if (_show)
        {

            LoadingMyCollection.SetActive(true);
            MyCollectionObject.SetActive(false);
            myCollectionUI.SetActive(true);

            MyNFTCollection.insta.DestroyItems();

            await CoreChainManager.Instance.CheckPuzzleList();
            List<string> temp_list = new List<string>();
            temp_list = CoreChainManager.Instance.nftList;
            if (temp_list.Count > 0)
            {
                for (int i = 0; i < temp_list.Count; i++)
                {
                    if (temp_list[i].StartsWith("50") && temp_list[i].Length == 3)
                    {
                        MyNFTCollection.insta.GenerateItem(Int32.Parse(temp_list[i]));
                    }

                }
                //TxtHeaderCollection.text = "Themes";                

                LoadingMyCollection.SetActive(false);
                MyCollectionObject.SetActive(true);
            }
            else
            {
                MessaeBox.insta.showMsg("Nothing in collection", true);
                myCollectionUI.SetActive(false);
            };

            if (!MyNFTCollection.insta.hasItems())
            {
                MessaeBox.insta.showMsg("No Themes purchased!\nGo to shop and buy themes!", true);
                myCollectionUI.SetActive(false);
            }




        }
        else
        {
            myCollectionUI.SetActive(false);
        }

    }


    [Header("SINGLE 1 v 1 Data")]
    [SerializeField] float total1v1Timer;
    [SerializeField] float current1v1Timer;
    [SerializeField] TMP_Text currentTimer1v1UI;

    IEnumerator startGame(LevelDetails levelData = null)
    {
        if (levelData != null)
        {
            CommonReferences.Instance.gameState = GameState.SINGLEPLAYER;
        }
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();




        currentTimer1v1UI.gameObject.SetActive(CommonReferences.is1v1);

        isGameRunning = true;

        endlessMultiplayerUI.SetActive(false);
        private1v1MultiplayerUI.SetActive(false);
        for (int i = FindObjectParent.childCount; i > 0; i--)
        {
            DestroyImmediate(FindObjectParent.GetChild(i - 1).gameObject);
        }

        for (int i = collectionUIParent.childCount; i > 0; i--)
        {
            DestroyImmediate(collectionUIParent.GetChild(i - 1).gameObject);
        }
        collectionUIParent.gameObject.SetActive(false);

        HandlePlayerStateChange(PlayerState.WORLD);

        Time.timeScale = 1f;

        CommonReferences.Instance.powersManagement.ResetPowers();
        if (CommonReferences.Instance.gameState == GameState.SINGLEPLAYER && levelData == null)
        {
            CommonReferences.Instance.ToggleStaticPlayer(true);
        }
        else
        {
            CommonReferences.Instance.ToggleStaticPlayer(false);
        }

        zombiesKilled = 0;
        headshots = 0;
        PlayerInventory.Instance.ClearInventory();


        if (CommonReferences.Instance.gameState == GameState.MULTIPLAYER)
        {
            for (int i = 0; i < environements.Length; i++)
            {
                environements[i].SetActive(false);
            }

            int levelIndex = (int)PhotonNetwork.CurrentRoom.CustomProperties["levelIndex"];
            environements[levelIndex].SetActive(true);
            RenderSettings.skybox = skyboxMaterials[levelIndex];
            RenderSettings.fogColor = fogColors[levelIndex];
            RenderSettings.ambientLight = ambientColors[levelIndex];
            RenderSettings.fogDensity = fogAmount[levelIndex];
        }
        else
        {
            for (int i = 0; i < environements.Length; i++)
            {
                environements[i].SetActive(false);
            }

            int levelIndex = PlayerPrefs.GetInt("selectedLevel", 0);

            environements[levelIndex].SetActive(true);
            RenderSettings.skybox = skyboxMaterials[levelIndex];
            RenderSettings.fogColor = fogColors[levelIndex];
            RenderSettings.ambientLight = ambientColors[levelIndex];
            RenderSettings.fogDensity = fogAmount[levelIndex];
        }


        if (missionMode)
        {
            if (levelData != null)
            {
                currentLevelData = new LevelDetails(levelData);

                text_levelTask.text = "";
                text_currentTaskStatus.text = "";

                switch (currentLevelData.level_type)
                {
                    case LEVEL_TYPE.SURVIVAL:
                        {
                            text_levelTask.text = "Survive for " + currentLevelData.level_time.ToString() + " Seconds to pass this level ";
                            break;
                        }
                    case LEVEL_TYPE.KILLCOUNT:
                        {
                            text_levelTask.text = "Kill " + currentLevelData.kill_count.ToString() + " Zombies to pass this level.";
                            break;
                        }
                    case LEVEL_TYPE.KILLCOUNT_TIMER:
                        {
                            text_levelTask.text = "Kill " + currentLevelData.kill_count.ToString() + " Zombies in " + currentLevelData.level_time.ToString() + "seconds to pass this level.";
                            break;
                        }
                    case LEVEL_TYPE.HPSAVE_TIMER:
                        {
                            text_levelTask.text = "Maintain " + currentLevelData.hp_percentage.ToString() + "% Health till " + currentLevelData.level_time.ToString() + " seconds to pass this level.";
                            break;
                        }
                    case LEVEL_TYPE.HEADSHOT:
                        {
                            text_levelTask.text = "Kill " + currentLevelData.headshot_count.ToString() + " Enemies With Headshow " + " to pass this level.";
                            break;
                        }
                    case LEVEL_TYPE.FINDKEY:
                        {
                            for (int i = 0; i < currentLevelData.needToCollected.Count; i++)
                            {
                                GameObject go = Instantiate(collectionUiItem, collectionUIParent);
                                go.transform.GetChild(0).GetComponent<Image>().sprite = itemSprites[(int)currentLevelData.needToCollected[i].type];
                                go.transform.GetChild(1).GetComponent<TMP_Text>().text = currentLevelData.needToCollected[i].count.ToString();
                                go.GetComponent<CollectionUIItem>().type = currentLevelData.needToCollected[i].type;

                                for (int j = currentLevelData.needToCollected[i].count; j > 0; j--)
                                {
                                    Instantiate(currentLevelData.needToCollected[i].collectedPrefab, FindObjectParent);
                                }
                            }

                            text_levelTask.text = "Find The Items.";

                            for (int i = 0; i < currentLevelData.needToCollected.Count; i++)
                            {
                                if (currentLevelData.needToCollected[i].count > 0)
                                {
                                    //                                    text_levelTask.text += "Find The Items.";
                                }
                            }

                            collectionUIParent.gameObject.SetActive(true);
                            break;
                        }
                }

                text_levelTask.gameObject.SetActive(true);
                text_currentTaskStatus.gameObject.SetActive(true);
                isGameRunning = true;
            }
            else if (currentLevelData != null)
            {

                currentLevelData = new LevelDetails(LevelSystem.levelsData[currentLevelData.level_no]);
                text_levelTask.text = "";
                text_currentTaskStatus.text = "";

                switch (currentLevelData.level_type)
                {
                    case LEVEL_TYPE.SURVIVAL:
                        {
                            text_levelTask.text = "Survive for " + currentLevelData.level_time.ToString() + " Seconds to pass this level ";
                            break;
                        }
                    case LEVEL_TYPE.KILLCOUNT:
                        {
                            text_levelTask.text = "Kill " + currentLevelData.kill_count.ToString() + " Zombies to pass this level.";
                            break;
                        }
                    case LEVEL_TYPE.KILLCOUNT_TIMER:
                        {
                            text_levelTask.text = "Kill " + currentLevelData.kill_count.ToString() + " Zombies in " + currentLevelData.level_time.ToString() + "seconds to pass this level.";
                            break;
                        }
                    case LEVEL_TYPE.HPSAVE_TIMER:
                        {
                            text_levelTask.text = "Maintain " + currentLevelData.hp_percentage.ToString() + "% Health till " + currentLevelData.level_time.ToString() + " seconds to pass this level.";
                            break;
                        }
                    case LEVEL_TYPE.HEADSHOT:
                        {
                            text_levelTask.text = "Kill " + currentLevelData.headshot_count.ToString() + " Enemies With Headshow " + " to pass this level.";
                            break;
                        }
                    case LEVEL_TYPE.FINDKEY:
                        {

                            for (int i = 0; i < currentLevelData.needToCollected.Count; i++)
                            {
                                GameObject go = Instantiate(collectionUiItem, collectionUIParent);
                                go.transform.GetChild(0).GetComponent<Image>().sprite = itemSprites[(int)currentLevelData.needToCollected[i].type];
                                go.transform.GetChild(1).GetComponent<TMP_Text>().text = currentLevelData.needToCollected[i].count.ToString();
                                go.GetComponent<CollectionUIItem>().type = currentLevelData.needToCollected[i].type;

                                for (int j = currentLevelData.needToCollected[i].count; j > 0; j--)
                                {
                                    Instantiate(currentLevelData.needToCollected[i].collectedPrefab, FindObjectParent);
                                }
                            }
                            text_levelTask.text = "Find The Items.";
                            for (int i = 0; i < currentLevelData.needToCollected.Count; i++)
                            {
                                if (currentLevelData.needToCollected[i].count > 0)
                                {
                                    //                                    text_levelTask.text += "Find The Items.";
                                }
                            }

                            collectionUIParent.gameObject.SetActive(true);
                            break;
                        }
                }

                text_levelTask.gameObject.SetActive(true);
                text_currentTaskStatus.gameObject.SetActive(true);
                isGameRunning = true;
            }
        }
        else
        {
            currentLevelData = null;
            text_levelTask.gameObject.SetActive(false);
            text_currentTaskStatus.gameObject.SetActive(false);
        }

        AbilitiesManager.Instance?.ResetAll(CommonReferences.Instance.gameState);

        //StartUI.SetActive(false);
        if (PlayerPrefs.GetInt("tutorial", 0) == 0 && CommonReferences.Instance.gameState != GameState.MULTIPLAYER)
        {
            ShowTutorial();
        }
        else
        {

            if (CommonReferences.Instance.gameState == GameState.MULTIPLAYER)
            {
                if (CommonReferences.Instance.weaponSystem != null)
                {
                    weaponSystem = CommonReferences.Instance.weaponSystem;
                }
                else
                {
                    weaponSystem = CommonReferences.Instance.weaponSystem = CommonReferences.Instance.myPlayer.GetComponent<FirstPersonCharacter>().weaponSystem;
                }
            }
            else
            {
                weaponSystem = CommonReferences.Instance.staticWeaponSystem;
            }



            weaponSystem.ResetWeapons();

            if (CommonReferences.Instance.myPlayer == null)
            {
                CommonReferences.Instance.myPlayer = CommonReferences.Instance.staticSinglePlayer;
                CommonReferences.Instance.myPlayer.gameObject.SetActive(true);
                CommonReferences.Instance.myPlayer.GetComponent<FirstPersonCharacter>().PreparePlayer();
            }
            else
            {
                CommonReferences.Instance.myPlayer.gameObject.SetActive(true);
                CommonReferences.Instance.myPlayer.GetComponent<FirstPersonCharacter>().PreparePlayer();
            }

            if (PlayerHealthInfo.Instance)
            {
                PlayerHealthInfo.Instance.ResetHealthSlider();
            }

            for (int i = 0; i < toDisableObjectsOnGameStart.Length; i++)
            {
                toDisableObjectsOnGameStart[i].SetActive(false);
            }
            for (int i = 0; i < toEnableObjectsOnGameStart.Length; i++)
            {
                toEnableObjectsOnGameStart[i].SetActive(true);
            }

            weaponInfoSystem.GenerateWeaponUI(weaponSystem.weapons);

            ZombiePool.Instance.GenerateNewZombies();
            AudioManager.insta.StartThunderSound();
        }
    }


    public void StartGame(LevelDetails levelData = null)
    {
        CommonReferences.Instance.ResetVariables();

        StartCoroutine(startGame(levelData));

    }
    public void PlayEndLessModeSinglePlayer()
    {
        missionMode = false;
        currentLevelData = null;

        CommonReferences.Instance.gameState = GameState.SINGLEPLAYER;

        CommonReferences.is1v1 = false;
        // PhotonManager.Instance.JoinPublicRoom();
        //endlessMultiplayerUI.SetActive(true);
        StartGame(null);
    }


    public void PlayEndLessModeMultiplayer()
    {
        missionMode = false;
        currentLevelData = null;

        PhotonManager.Instance.JoinPublicRoom();
        endlessMultiplayerUI.SetActive(true);

        CommonReferences.is1v1 = false;
        CommonReferences.Instance.gameState = GameState.MULTIPLAYER;
        //StartGame(null);
    }

    public void Create1v1Room()
    {
        if (string.IsNullOrEmpty(roomName_Input.text)) return;

        missionMode = false;
        currentLevelData = null;

        PhotonManager.Instance.CreatePrivateRoom1v1(roomName_Input.text);
        private1v1MultiplayerUI.SetActive(true);
        CommonReferences.is1v1 = true;
        CommonReferences.Instance.gameState = GameState.MULTIPLAYER;
        //StartGame(null);
    }
    public void Join1v1Room()
    {
        if (string.IsNullOrEmpty(roomName_Input.text)) return;

        missionMode = false;
        currentLevelData = null;

        PhotonManager.Instance.JoinPrivateRoom1v1(roomName_Input.text);

        private1v1MultiplayerUI.SetActive(true);
        CommonReferences.is1v1 = true;
        CommonReferences.Instance.gameState = GameState.MULTIPLAYER;
        //StartGame(null);
    }

    #region MISSION MODE METHODS

    public int headshots = 0;
    public int zombiesKilled = 0;
    [SerializeField] Image health_fillbar;
    public bool isGameRunning = false;

    [SerializeField] float gameRunningTimer => CommonReferences.Instance.playerController.survivalTime;
    private void CheckForLevelComplete()
    {
        if (currentLevelData == null) return;
        if (!isGameRunning) return;

        switch (currentLevelData.level_type)
        {
            case LEVEL_TYPE.SURVIVAL:
                {
                    if (gameRunningTimer >= currentLevelData.level_time)
                    {
                        isGameRunning = false;
                        LevelPassed();
                    }
                    text_currentTaskStatus.text = "Timer : " + (int)gameRunningTimer + " seconds";
                    break;
                }
            case LEVEL_TYPE.KILLCOUNT:
                {
                    if (zombiesKilled >= currentLevelData.kill_count)
                    {
                        isGameRunning = false;
                        LevelPassed();
                    }
                    text_currentTaskStatus.text = "Zombie Kill Count : " + zombiesKilled;
                    break;
                }
            case LEVEL_TYPE.KILLCOUNT_TIMER:
                {
                    text_currentTaskStatus.text = "Zombie Kill Count : " + zombiesKilled + "\nTimer : " + (int)gameRunningTimer + " seconds";

                    if (zombiesKilled >= currentLevelData.kill_count)
                    {
                        isGameRunning = false;
                        LevelPassed();
                        break;
                    }
                    if (gameRunningTimer > currentLevelData.level_time)
                    {
                        isGameRunning = false;
                        LevelFailed();
                    }



                    break;
                }
            case LEVEL_TYPE.HPSAVE_TIMER:
                {
                    text_currentTaskStatus.text = "Current HP per. : " + (health_fillbar.fillAmount * 100) + "\nTimer : " + (int)gameRunningTimer + " seconds";

                    if (gameRunningTimer > currentLevelData.level_time)
                    {
                        isGameRunning = false;
                        if (health_fillbar.fillAmount >= currentLevelData.hp_percentage / 100)
                        {
                            LevelPassed();
                        }
                        else
                        {
                            LevelFailed();
                        }

                    }

                    break;
                }
            case LEVEL_TYPE.HEADSHOT:
                {
                    text_currentTaskStatus.text = "Headshots : " + (headshots.ToString());

                    if (headshots >= currentLevelData.headshot_count)
                    {
                        isGameRunning = false;
                        LevelPassed();
                        break;
                    }
                    if (gameRunningTimer > currentLevelData.level_time)
                    {
                        isGameRunning = false;
                        LevelFailed();
                    }
                    break;
                }
            case LEVEL_TYPE.FINDKEY:
                {

                    if (allFinded)
                    {
                        allFinded = false;
                        isGameRunning = false;
                        ScoreManager.Instance.coinsGot += 100;
                        LevelPassed();
                    }
                    break;
                }
        }
    }
    private void ItemCollected(ItemType _type, int reward)
    {
        if (currentLevelData != null)
        {
            ItemsCount iCount = currentLevelData.needToCollected.Find(x => x.type == _type);
            if (iCount != null)
            {
                iCount.count--;
            }

            for (int i = 0; i < collectionUIParent.childCount; i++)
            {
                int temp = i;
                if (collectionUIParent.GetChild(i).TryGetComponent<CollectionUIItem>(out CollectionUIItem uiItem))
                {
                    if (uiItem.type == iCount.type)
                    {


                        collectionUIParent.GetChild(temp).GetChild(1).GetComponent<TMP_Text>().text = iCount.count.ToString();
                        if (iCount.count == 0)
                        {
                            Destroy(collectionUIParent.GetChild(temp).gameObject);
                        }
                        break;
                    }
                }
            }

            ScoreManager.Instance.coinsGot += reward;

            bool passed = true;
            for (int i = 0; i < currentLevelData.needToCollected.Count; i++)
            {
                if (currentLevelData.needToCollected[i].count > 0)
                {
                    passed = false;
                    break;
                }
            }

            allFinded = passed;

        }
    }

    public bool allFinded = false;

    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] GameObject nextLevelBTN;
    [SerializeField] GameObject retryBTN;
    [SerializeField] GameObject get3XCoinsBTN;
    [SerializeField] TMP_Text level_status_text;
    [SerializeField] TMP_Text coinsGot_level_text;
    private void LevelPassed()
    {
        ZombiePool.Instance.ClearPool();
        ShowLevelCompleteUI();
        get3XCoinsBTN.SetActive(true);

        UpdateGunSystemData();
    }



    private void LevelFailed()
    {
        ZombiePool.Instance.ClearPool();
        get3XCoinsBTN.SetActive(false);
        ShowLevelFailUI();
        LocalData data = DatabaseManager.Instance.GetLocalData();
        if (data != null)
        {
            for (int i = 0; i < data.weaponDetails.Count; i++)
            {
                for (int j = 0; j < weaponSystem.weapons.Count; j++)
                {
                    if (weaponSystem.weapons[j].GetComponent<Weapon>().weapon_index == data.weaponDetails[i].weaponInfo.weaponIndex)
                    {

                        data.weaponDetails[i].weaponInfo.ammoCount = weaponSystem.weapons[j].GetComponent<Weapon>().totalAmmo + weaponSystem.weapons[j].GetComponent<Weapon>().currentAmmo;
                        if (data.weaponDetails[i].weaponInfo.weaponIndex == 0)
                        {
                            data.weaponDetails[i].weaponInfo.ammoCount = 176;
                        }
                        WeaponDetails w_Details = PlayerWeaponsInfo.Instance.all_weapon_details.Find(x => x.weaponInfo.weaponIndex == weaponSystem.weapons[j].GetComponent<Weapon>().weapon_index);
                        if (w_Details != null)
                        {
                            w_Details.weaponInfo.ammoCount = weaponSystem.weapons[j].GetComponent<Weapon>().totalAmmo + weaponSystem.weapons[j].GetComponent<Weapon>().currentAmmo;
                        }

                    }
                }
            }

            data.coins += ScoreManager.Instance.coinsGot;
            DatabaseManager.Instance.UpdateData(data);
        }
    }


    private void UpdateGunSystemData()
    {
        LocalData data = DatabaseManager.Instance.GetLocalData();
        if (data != null)
        {
            for (int i = 0; i < data.weaponDetails.Count; i++)
            {
                for (int j = 0; j < weaponSystem.weapons.Count; j++)
                {
                    if (weaponSystem.weapons[j].GetComponent<Weapon>().weapon_index == data.weaponDetails[i].weaponInfo.weaponIndex)
                    {

                        data.weaponDetails[i].weaponInfo.ammoCount = weaponSystem.weapons[j].GetComponent<Weapon>().totalAmmo + weaponSystem.weapons[j].GetComponent<Weapon>().currentAmmo;
                        if (data.weaponDetails[i].weaponInfo.weaponIndex == 0)
                        {
                            data.weaponDetails[i].weaponInfo.ammoCount = 176;
                        }
                        WeaponDetails w_Details = PlayerWeaponsInfo.Instance.all_weapon_details.Find(x => x.weaponInfo.weaponIndex == weaponSystem.weapons[j].GetComponent<Weapon>().weapon_index);
                        if (w_Details != null)
                        {
                            w_Details.weaponInfo.ammoCount = weaponSystem.weapons[j].GetComponent<Weapon>().totalAmmo + weaponSystem.weapons[j].GetComponent<Weapon>().currentAmmo;
                        }

                    }
                }
            }

            if (CommonReferences.Instance.playerController != null)
            {
                data.coins += ScoreManager.Instance.coinsGot;
            }
            else
            {
                if (CommonReferences.Instance.myPlayer != null)
                {
                    CommonReferences.Instance.playerController = CommonReferences.Instance.myPlayer.GetComponent<FirstPersonCharacter>();
                    data.coins += ScoreManager.Instance.coinsGot;
                }
            }
            DatabaseManager.Instance.UpdateData(data);
        }
    }
    public void NextLevel()
    {
        currentLevelData = new LevelDetails(LevelSystem.levelsData[currentLevelData.level_no + 1]);
        Time.timeScale = 1f;

        LeanTween.scale(levelCompletePanel.transform.GetChild(0).gameObject, Vector3.zero, 0.2f).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
        {
            levelCompletePanel.SetActive(false);
            StartGame();
        });

        if (weaponSystem.current_weapon != null)
        {
            weaponSystem.current_weapon.showCrosshair = true;
        }
    }
    private void ShowLevelCompleteUI()
    {
        CommonReferences.Instance.ChangeState(PlayerState.WORLD);
        carController.canDrive = false;
        carController.GetComponent<AudioListener>().enabled = false;
        CommonReferences.Instance.myPlayer.SetActive(false);
        menuCamera.SetActive(true);
        lastCoins = ScoreManager.Instance.coinsGot;

        level_status_text.text = "Level Complete";
        coinsGot_level_text.text = "Coins Got: " + lastCoins.ToString();

        LocalData data = DatabaseManager.Instance.GetLocalData();

        if (currentLevelData != null)
        {
            if (currentLevelData.level_no >= data.current_level)
            {
                data.current_level++;
            }
        }

        if (data.current_level > LevelSystem.levelsData.Count - 1)
        {
            data.current_level = LevelSystem.levelsData.Count - 1;
        }
        data.coins += lastCoins;
        DatabaseManager.Instance.UpdateData(data);

        retryBTN.SetActive(false);
        if (currentLevelData.level_no != LevelSystem.levelsData.Count - 1)
        {
            nextLevelBTN.SetActive(true);
        }
        else
        {
            nextLevelBTN.SetActive(false);
        }

        levelCompletePanel.SetActive(true);

        LeanTween.scale(levelCompletePanel.transform.GetChild(0).gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInQuad);
    }
    internal void ShowLevelFailUI()
    {
        CommonReferences.Instance.myPlayer.SetActive(false);
        menuCamera.SetActive(true);
        carController.GetComponent<AudioListener>().enabled = false;
        lastCoins = ScoreManager.Instance.coinsGot;

        LocalData data = DatabaseManager.Instance.GetLocalData();
        data.coins += lastCoins;
        DatabaseManager.Instance.UpdateData(data);

        level_status_text.text = "Level Failed";
        coinsGot_level_text.text = "Coins Got: " + lastCoins.ToString();

        retryBTN.SetActive(true);
        nextLevelBTN.SetActive(false);

        levelCompletePanel.SetActive(true);


        LeanTween.scale(levelCompletePanel.transform.GetChild(0).gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInQuad);
    }
    #endregion

    #region Tutorial
    public void ShowTutorial()
    {
        TutorialUI.SetActive(true);
        for (int i = 0; i < tutorialObjects.Length; i++)
        {
            tutorialObjects[i].SetActive(false);
        }
        tutorialObjects[currentTutorial].SetActive(true);
    }
    public void NextTutorial()
    {
        tutorialObjects[currentTutorial].SetActive(false);
        currentTutorial++;
        if (currentTutorial >= tutorialObjects.Length)
        {
            SkipTutorial();
            return;
        }
        tutorialObjects[currentTutorial].SetActive(true);
    }
    public void SkipTutorial()
    {
        PlayerPrefs.SetInt("tutorial", 1);
        TutorialUI.SetActive(false);
        StartGame();
        //MPNetworkManager.insta.OnConnectedToServer();
    }


    #endregion

    public void UpdatePlayerUIData(bool _show, LocalData data, bool _init = false)
    {
        if (_show)
        {
            if (_init)
            {
                nameInput.text = data.name;
                // SelectGender(data.characterNo);
            }

            scoreTxt.text = data.coins.ToString();
            SetCoinsText();


            // if (PhotonNetwork.LocalPlayer.CustomProperties["health"] != null) healthSlider.value = float.Parse(PhotonNetwork.LocalPlayer.CustomProperties["health"].ToString());
        }
        else
        {
            GameplayUI.SetActive(false);
        }
    }
    public void UpdatePlayerUIData(bool _show, bool _init = false)
    {
        if (_show)
        {
            //if (PhotonNetwork.LocalPlayer.CustomProperties["health"] != null) healthSlider.value = float.Parse(PhotonNetwork.LocalPlayer.CustomProperties["health"].ToString());
        }
        else
        {
            GameplayUI.SetActive(false);
        }
    }


    public TMP_Text txt_information;
    public void ShowBurnableNFTConfimation(int _id, string status)
    {
        txt_information.transform.parent.gameObject.SetActive(true);
        if (status.Equals("success"))
        {
            txt_information.text = "Coin Purchase of " + status + " successful";
        }
        else
        {
            txt_information.text = "Coin Purchase of " + status + " Failed";
        }

        StartCoroutine(disableTextInfo());
    }
    public void ShowCoinPurchaseStatus(TranscationInfo info)
    {
        txt_information.transform.parent.gameObject.SetActive(true);
        if (info.transactionStatus.Equals("success"))
        {
            txt_information.text = "Coin Purchase of " + info.coinAmount + " successful";
        }
        else
        {
            txt_information.text = "Coin Purchase of " + info.coinAmount + " Failed";
        }
        StartCoroutine(disableTextInfo());
    }



    public void UpdateUserName(string _name, string _ethad = null)
    {
        if (_ethad != null)
        {
            usernameText.text = "Hi, " + _name + "\n Your crypto address is : " + _ethad;
            username = _name;
        }
        else usernameText.text = _name;
    }

    public void UpdateStatus(string _msg)
    {
        statusText.text = _msg;
        StartCoroutine(ResetUpdateText());
    }

    IEnumerator ResetUpdateText()
    {
        yield return new WaitForSeconds(2);
        statusText.text = "";
    }


    public void EditUserProfile()
    {
        if (DatabaseManager.Instance != null)
        {
            nameInput.text = DatabaseManager.Instance.GetLocalData().name;
        }

        usernameUI.SetActive(true);
        StartUI.SetActive(false);
    }
    public void GetName()
    {
        if (nameInput.text.Length > 0 && !nameInput.text.Contains("Enter")) username = nameInput.text;
        else username = "Player_" + UnityEngine.Random.Range(11111, 99999);

        usernameUI.SetActive(false);

        LocalData data = DatabaseManager.Instance.GetLocalData();
        data.name = username;

        DatabaseManager.Instance.UpdateData(data);
        StartUI.SetActive(true);
    }


    #region Kill Combo  Management
    [Header("Combo")]
    public int headshotCombo;
    [SerializeField] float comboResetTime;
    [SerializeField] float currentTime;
    [SerializeField] GameObject combo_obj;
    [SerializeField] Image combo_timerFill;
    [SerializeField] TMP_Text combo_txt;
    [SerializeField] Vector3 start_combo_pos;

    public void showHeadShotComboMsg()
    {
        combo_obj.GetComponent<CanvasGroup>().alpha = 0;
        combo_obj.SetActive(true);
        headshotCombo++;
        currentTime = comboResetTime;
        combo_txt.text = headshotCombo.ToString();
        combo_obj.GetComponent<RectTransform>().anchoredPosition = start_combo_pos + new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0);
        combo_obj.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, Random.Range(-30f, 30f));
        LeanTween.scale(combo_obj, Vector3.one, 0.25f).setFrom(Vector3.one * 4).setEaseInQuad();
        LeanTween.alphaCanvas(combo_obj.GetComponent<CanvasGroup>(), 1, 0.2f).setFrom(0).setFrom(Vector3.one * 1000).setEaseInQuad();
        AudioManager.insta.playSound(0);

    }
    private void Update()
    {
        combo_timerFill.fillAmount = currentTime / comboResetTime;


        if (headshotCombo > 0 && currentTime > 0)
        {

            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {

                currentTime = comboResetTime;
                headshotCombo = 0;

                combo_obj.SetActive(false);
                combo_obj.GetComponent<CanvasGroup>().alpha = 0;

            }

        }
        else
        {
            currentTime = comboResetTime;
        }
        if (isGameRunning && CommonReferences.is1v1)
        {
            current1v1Timer -= Time.deltaTime;
            currentTimer1v1UI.text = "Time Remaining : " + (int)current1v1Timer + " Seconds";
            if (current1v1Timer <= 0)
            {
                Show1v1GameCompleteUI((int)PhotonNetwork.LocalPlayer.CustomProperties["score"], (int)CommonReferences.playerIn1v1.CustomProperties["score"]);
            }
        }

        if (missionMode && isGameRunning)
        {
            CheckForLevelComplete();
        }

    }


    #endregion

    public void ToggleLoadingPanel(bool _show)
    {
        LoadingPanel.SetActive(_show);
    }

    public void ToggleExtraButtons(bool _active)
    {
        extraBTNS.SetActive(_active);
    }


    public bool GetPausePanelActive()
    {
        return pausePanelUI.activeInHierarchy;
    }
    [SerializeField] GameObject pausePanelUI;
    [Header("GAme Complete UI")]
    public GameObject gameoverUI;
    [SerializeField] TMP_Text survivalTime_text;
    [SerializeField] TMP_Text starsgotText;
    [SerializeField] TMP_Text myScoreTxt;
    [SerializeField] TMP_Text otherScoreTxt;
    [SerializeField] TMP_Text winnerTxt;
    [SerializeField] bool won = false;
    [SerializeField] GameObject menuCamera;

    int lastCoins;
    public void ShowGameCompleteUI(float timer, int coinsChange)
    {
        CommonReferences.Instance.ChangeState(PlayerState.WORLD);

        Cursor.lockState = CursorLockMode.None;

        carController.canDrive = false;
        menuCamera.SetActive(true);
        carController.GetComponent<AudioListener>().enabled = false;

        survivalTime_text.text = "Survival Time : " + string.Format("{0:00}:{1:00}", (int)timer / 60, (int)timer % 60);
        starsgotText.text = "Coins Got: " + coinsChange.ToString();


        gameoverUI.SetActive(true);
        if (timer >= 120)
        {
            gameoverUI.transform.GetChild(1).transform.gameObject.SetActive(true);
        }
        else
        {
            gameoverUI.transform.GetChild(1).transform.gameObject.SetActive(false);
        }

        LeanTween.scale(gameoverUI.transform.GetChild(0).gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInQuad);
    }

    public void Show1v1GameCompleteUI(int myScore, int otherScore)
    {

        Cursor.lockState = CursorLockMode.None;

        GameOverPanel.is1v1 = true;
        isGameRunning = false;
        CommonReferences.Instance.ChangeState(PlayerState.WORLD);

        menuCamera.SetActive(true);

        myScoreTxt.text = "My Score : " + myScore.ToString();
        otherScoreTxt.text = "Opponent's Score : " + otherScore.ToString();
        if (myScore > otherScore)
        {
            winnerTxt.text = "Winner!";
        }
        else if (myScore < otherScore)
        {
            winnerTxt.text = "Loser!";
        }
        else
        {
            winnerTxt.text = "Its a Tie!";
        }

        starsgotText.text = "Coins Got: " + myScore.ToString();

        LocalData data = DatabaseManager.Instance.GetLocalData();
        data.coins += ScoreManager.Instance.coinsGot;
        data.zombiesKilled += ScoreManager.Instance.zombiesKilled;
        DatabaseManager.Instance.UpdateData(data);

        gameoverUI.SetActive(true);
        CommonReferences.Instance.myPlayer.SetActive(false);

        LeanTween.scale(gameoverUI.transform.GetChild(0).gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInQuad);

        UpdateGunSystemData();

    }
    public void Show1v1GameCompleteUForce(int myScore, bool myPlayerDied = false, bool otherplayerLeft = false)
    {
        CommonReferences.Instance.myPlayer.SetActive(false);
        GameOverPanel.is1v1 = true;
        isGameRunning = false;
        CommonReferences.Instance.ChangeState(PlayerState.WORLD);


        menuCamera.SetActive(true);

        myScoreTxt.text = "My Score : " + myScore.ToString();

        otherScoreTxt.text = "";
        if (myPlayerDied && !otherplayerLeft)
        {
            gameoverUI.transform.GetChild(1).gameObject.SetActive(false);
            winnerTxt.text = "Winner is Opponent";
        }
        else if (!myPlayerDied && !otherplayerLeft)
        {
            gameoverUI.transform.GetChild(1).gameObject.SetActive(true);
            winnerTxt.text = "Oppenent Died! You Are Winner";
        }
        else if (!myPlayerDied && otherplayerLeft)
        {
            gameoverUI.transform.GetChild(1).gameObject.SetActive(true);
            winnerTxt.text = "Other Player Left!";
        }

        LocalData data = DatabaseManager.Instance.GetLocalData();
        data.coins += ScoreManager.Instance.coinsGot;
        data.zombiesKilled += ScoreManager.Instance.zombiesKilled;
        DatabaseManager.Instance.UpdateData(data);


        gameoverUI.SetActive(true);

        LeanTween.scale(gameoverUI.transform.GetChild(0).gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInQuad);

        UpdateGunSystemData();

        CommonReferences.is1v1 = false;
        CommonReferences.playerIn1v1 = null;
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        if (gameoverUI.activeInHierarchy)
        {
            LeanTween.scale(gameoverUI.transform.GetChild(0).gameObject, Vector3.zero, 0.2f).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
            {
                gameoverUI.SetActive(false);
                StartGame();
            });
        }
        else if (pausePanelUI.activeInHierarchy)
        {
            pausePanelUI.SetActive(false);
            StartGame();
        }
        if (weaponSystem.current_weapon != null)
        {
            weaponSystem.current_weapon.showCrosshair = true;
        }

        carController.transform.SetPositionAndRotation(carStartPoz.position, carStartPoz.rotation);

    }
    public void Pause()
    {
        if (CommonReferences.Instance.gameState == GameState.SINGLEPLAYER)
        {
            if (weaponSystem.current_weapon != null)
            {
                weaponSystem.current_weapon.showCrosshair = false;
            }
            Cursor.lockState = CursorLockMode.None;
            pausePanelUI.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            if (weaponSystem.current_weapon != null)
            {
                weaponSystem.current_weapon.showCrosshair = false;
            }
            Cursor.lockState = CursorLockMode.None;
            pausePanelUI.SetActive(true);
        }
    }


    public void Resume()
    {
        if (weaponSystem.current_weapon != null)
        {
            weaponSystem.current_weapon.showCrosshair = true;
        }
        if (!ControlSwitcher.Instance.isMobileControls)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Time.timeScale = 1f;
        pausePanelUI.SetActive(false);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;

        environements[0].SetActive(false);
        environements[1].SetActive(false);
        environements[2].SetActive(true);

        carController.canDrive = false;
        carController.transform.SetPositionAndRotation(carMenuPoz.position, carMenuPoz.rotation);


        CommonReferences.Instance.ChangeState(PlayerState.WORLD);

        LocalData data = DatabaseManager.Instance.GetLocalData();
        if (data != null)
        {
            for (int i = 0; i < data.weaponDetails.Count; i++)
            {
                for (int j = 0; j < weaponSystem.weapons.Count; j++)
                {
                    if (weaponSystem.weapons[j].GetComponent<Weapon>().weapon_index == data.weaponDetails[i].weaponInfo.weaponIndex)
                    {

                        data.weaponDetails[i].weaponInfo.ammoCount = weaponSystem.weapons[j].GetComponent<Weapon>().totalAmmo + weaponSystem.weapons[j].GetComponent<Weapon>().currentAmmo;
                        if (data.weaponDetails[i].weaponInfo.weaponIndex == 0)
                        {
                            data.weaponDetails[i].weaponInfo.ammoCount = 176;
                        }
                        WeaponDetails w_Details = PlayerWeaponsInfo.Instance.all_weapon_details.Find(x => x.weaponInfo.weaponIndex == weaponSystem.weapons[j].GetComponent<Weapon>().weapon_index);
                        if (w_Details != null)
                        {
                            w_Details.weaponInfo.ammoCount = weaponSystem.weapons[j].GetComponent<Weapon>().totalAmmo + weaponSystem.weapons[j].GetComponent<Weapon>().currentAmmo;
                        }

                    }
                }
            }


            data.coins += ScoreManager.Instance.coinsGot;
            data.zombiesKilled += ScoreManager.Instance.zombiesKilled;
            DatabaseManager.Instance.UpdateData(data);

        }


        ZombiePool.Instance.ClearPool();

        Cursor.lockState = CursorLockMode.None;

        if (gameoverUI.activeInHierarchy)
        {
            LeanTween.scale(gameoverUI.transform.GetChild(0).gameObject, Vector3.zero, 0.2f).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
            {
                gameoverUI.SetActive(false);
                for (int i = 0; i < toEnableObjectsOnGameStart.Length; i++)
                {
                    toEnableObjectsOnGameStart[i].SetActive(false);
                }
                for (int i = 0; i < toDisableObjectsOnGameStart.Length; i++)
                {
                    toDisableObjectsOnGameStart[i].SetActive(true);
                }



            });
        }
        else if (pausePanelUI)
        {
            pausePanelUI.SetActive(false);
            for (int i = 0; i < toEnableObjectsOnGameStart.Length; i++)
            {
                toEnableObjectsOnGameStart[i].SetActive(false);
            }
            for (int i = 0; i < toDisableObjectsOnGameStart.Length; i++)
            {
                toDisableObjectsOnGameStart[i].SetActive(true);
            }

        }

        if (PhotonNetwork.InRoom)
        {
            PhotonManager.Instance.LeaveGame();
        }

        for (int i = 0; i < environements.Length; i++)
        {
            environements[i].SetActive(false);
        }

        int levelIndex = 2;

        environements[levelIndex].SetActive(true);
        RenderSettings.skybox = skyboxMaterials[levelIndex];
        RenderSettings.fogColor = fogColors[levelIndex];
        RenderSettings.fogDensity = fogAmount[levelIndex];

    }



    public void CloseGameCompleteUI()
    {


        ToggleExtraButtons(true);
        LeanTween.scale(gameoverUI.transform.GetChild(0).gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInQuad).setOnComplete(() =>
        {
            gameoverUI.SetActive(false);
        });
    }








    Coroutine coroutine;
    [SerializeField] GameObject danger_msg_ui;
    public void ShowDangerMsg()
    {


        danger_msg_ui.SetActive(true);
        AudioManager.insta.playSound(2);
        LeanTween.scale(danger_msg_ui, Vector3.one, 0.15f).setOnComplete(() =>
        {
            LeanTween.scale(danger_msg_ui, Vector3.zero, 0.15f).setDelay(1.5f).setOnComplete(() =>
            {
                danger_msg_ui.SetActive(false);
            });
        });

    }
    public void ShowInfoMsg(string info, float time = 3f)
    {
        txt_information.transform.parent.gameObject.SetActive(true);

        txt_information.text = info;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(disableTextInfo(time));
    }
    IEnumerator disableTextInfo(float t = 3f)
    {
        yield return new WaitForSeconds(t);
        txt_information.transform.parent.gameObject.SetActive(false);
    }



    [SerializeField] TMP_Text[] txt_coins;
    public void SetCoinsText()
    {
        LocalData data = DatabaseManager.Instance.GetLocalData();
        for (int i = 0; i < txt_coins.Length; i++)
        {
            txt_coins[i].text = "Coins : " + data.coins.ToString();
        }
    }

    internal bool CheckUIOpened()
    {
        return pausePanelUI.activeInHierarchy;
    }


    public void ClaimToken()
    {
        CoreChainManager.Instance.getDailyToken();
    }
}
