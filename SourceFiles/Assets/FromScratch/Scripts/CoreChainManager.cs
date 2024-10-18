using Cysharp.Threading.Tasks;
using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class CoreChainManager : MonoBehaviour
{
    #region Singleton
    public static CoreChainManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public const string abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"values\",\"type\":\"uint256[]\"}],\"name\":\"TransferBatch\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"TransferSingle\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"string\",\"name\":\"value\",\"type\":\"string\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"URI\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_add\",\"type\":\"address\"}],\"name\":\"GetAllUserToken\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"accounts\",\"type\":\"address[]\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"}],\"name\":\"balanceOfBatch\",\"outputs\":[{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_itemId\",\"type\":\"uint256\"}],\"name\":\"buyCoins\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_tokenId\",\"type\":\"uint256\"},{\"internalType\":\"string\",\"name\":\"_tokenUrl\",\"type\":\"string\"}],\"name\":\"buyNonBurnItem\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getCurrentTime\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"_result\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"amounts\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeBatchTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"uri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_recipient\",\"type\":\"address\"}],\"name\":\"withdraw\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"}]";
    // address of contract
    public static string contract = "0x64dDF9B674B6CC05CaC59375Fe52f66750E9Fd98";


    public static string abiToken = "[{\"inputs\":[{\"internalType\":\"string\",\"name\":\"name\",\"type\":\"string\"},{\"internalType\":\"string\",\"name\":\"symbol\",\"type\":\"string\"},{\"internalType\":\"uint256\",\"name\":\"initialSupply\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"initialYearlyReturnRate\",\"type\":\"uint256\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_amount\",\"type\":\"uint256\"}],\"name\":\"ExchangeToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"GetGameToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"stake\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"TokensMinted\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"user\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"TokensStaked\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"user\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"reward\",\"type\":\"uint256\"}],\"name\":\"TokensUnstaked\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"recipient\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"unstake\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"newRate\",\"type\":\"uint256\"}],\"name\":\"updateYearlyReturnRate\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdraw\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"GetCurrentTime\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"_result\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_account\",\"type\":\"address\"}],\"name\":\"getPotentialReward\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"GetSmartContractBalance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_account\",\"type\":\"address\"}],\"name\":\"getStakingDuration\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_account\",\"type\":\"address\"}],\"name\":\"getTotalStaked\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_account\",\"type\":\"address\"}],\"name\":\"GetuserBalance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"stakers\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"stakingTime\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"yearlyReturnRate\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";


    // address of contract
    public static string contractToken = "0xc30E684Fa057dbcBA0cC4DBfCEe75AD79eD40C0A";

    static string chain = "";
    static string network = "";
    static string chainId = "199";
    static string networkRPC = "https://rpc.bittorrentchain.io";



    public static float[] coinCost = { 0.00001f, 0.00002f, 0.00003f, 0.00004f };

    public static string userBalance = "0";

    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account;

    [SerializeField] TMP_Text _status;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject[] toEnableObjectsAfterLogin;
    [SerializeField] GameObject[] toDisableObjectsAfterLogin;





    ProjectConfigScriptableObject projectConfigSO = null;
    private void Start()
    {
        //LoginWallet();
        //TestIT();

        projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));

        if (UIManager.insta.isMainnet)
        {
            projectConfigSO.ChainId = "199";
            projectConfigSO.Rpc = "https://rpc.bittorrentchain.io";
            chainId = "199";
            networkRPC = "https://rpc.bittorrentchain.io";
            contract = "0x8195AcB98d1bdA24ff51EC69949FA3Eb70b01D5E";
            contractToken = "0x0CE8817ec14f424898f65c44A6025AeC378D7a09";
        }
        else
        {
            projectConfigSO.ChainId = "1313161555";
            projectConfigSO.Rpc = "https://testnet.aurora.dev";

            chainId = "1313161555";
            networkRPC = "https://testnet.aurora.dev";
            contract = "0xa0BCe9CDAb2C34a4e4cEC168555cC9E3c72Eb186";
            contractToken = "0x0e4c0321B1e9B2Cf58A5BFe76909c01e63626e7a";
        }

        PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectId);
        PlayerPrefs.SetString("ChainID", projectConfigSO.ChainId);
        PlayerPrefs.SetString("Chain", projectConfigSO.Chain);
        PlayerPrefs.SetString("Network", projectConfigSO.Network);
        PlayerPrefs.SetString("RPC", projectConfigSO.Rpc);

    }


    public async void LoginWallet()
    {
        _status.text = "Connecting...";
#if !UNITY_EDITOR && !UNITY_ANDROID
        Web3Connect();
        OnConnected();
#else
        // get current timestamp
        int timestamp = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        // set expiration time
        int expirationTime = timestamp + 60;
        // set message
        string message = expirationTime.ToString();
        // sign message
        string signature = await Web3Wallet.Sign(message);
        // verify account
        string account = await EVM.Verify(message, signature);
        account = account.ToLower();
        int now = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        // validate
        if (account.Length == 42 && expirationTime >= now)
        {
            // save account
            PlayerPrefs.SetString("Account", account); ;

            print("Account: " + account);
            _status.text = "connected : " + account;
            CheckUserBalance();
            getTokenBalance();


            if (DatabaseManager.Instance)
            {
                DatabaseManager.Instance.GetData(true);
            }
            // load next scene
        }

        for (int i = 0; i < toDisableObjectsAfterLogin.Length; i++)
        {
            toDisableObjectsAfterLogin[i].SetActive(false);
        }
        loadingPanel.SetActive(true);
        SingletonDataManager.userethAdd = account;
        CovalentManager.insta.GetNFTUserBalance();
        PhotonManager.Instance.ConnectToPhotonNow();

        // Debug.Log("LIST OF PUZZLE: " + await CheckPuzzleList());



#endif

    }

    public void EnablePlayPanels()
    {
        for (int i = 0; i < toEnableObjectsAfterLogin.Length; i++)
        {
            toEnableObjectsAfterLogin[i].SetActive(true);
        }
        loadingPanel.SetActive(false);
    }

    async private void OnConnected()
    {
        account = ConnectAccount();
        while (account == "")
        {
            await new WaitForSecondsRealtime(2f);
            account = ConnectAccount();
        };
        account = account.ToLower();
        // save account for next scene
        PlayerPrefs.SetString("Account", account);
        _status.text = "connected : " + account;
        // reset login message
        SetConnectAccount("");
        CheckUserBalance();
        getTokenBalance();
        if (DatabaseManager.Instance)
        {
            DatabaseManager.Instance.GetData(true);
        }
        // load next scene
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);



        for (int i = 0; i < toDisableObjectsAfterLogin.Length; i++)
        {
            toDisableObjectsAfterLogin[i].SetActive(false);
        }
        loadingPanel.SetActive(true);
        //CoinBuyOnSendContract(0);
        SingletonDataManager.userethAdd = account;
        CovalentManager.insta.GetNFTUserBalance();
        PhotonManager.Instance.ConnectToPhotonNow();

    }


    #region BuyCoins
    async public void CoinBuyOnSendContract(int _pack)
    {
        if (MessaeBox.insta) MessaeBox.insta.showMsg("Coin purchase process started\nThis can up to minute", true);

        object[] inputParams = { _pack };

        float _amount = coinCost[_pack];
        float decimals = 1000000000000000000; // 18 decimals
        float wei = _amount * decimals;
        print(Convert.ToDecimal(wei).ToString() + " | " + Newtonsoft.Json.JsonConvert.SerializeObject(inputParams));
        // smart contract method to call
        string method = "buyCoins";

        // array of arguments for contract
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        // value in wei
        string value = Convert.ToDecimal(wei).ToString();
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";

        Debug.Log("value : " + value + " | " + coinCost[_pack]);
        // connects to user's browser wallet (metamask) to update contract state
        try
        {


#if !UNITY_EDITOR && !UNITY_ANDROID 
            string response = await Web3GL.SendContract(method, abi, contract, args, value);
            Debug.Log(response);
#else
            // string response = await EVM.c(method, abi, contract, args, value, gasLimit, gasPrice);
            // Debug.Log(response);
            string data = await EVM.CreateContractData(abi, method, args);
            string response = await Web3Wallet.SendTransaction(chainId, contract, value, data);


            Debug.Log(response);
#endif

            if (!string.IsNullOrEmpty(response))
            {
                if (DatabaseManager.Instance)
                {
                    DatabaseManager.Instance.AddTransaction(response, "pending", _pack);
                }

                CheckTransactionStatus(response);
                if (MessaeBox.insta) MessaeBox.insta.showMsg("Your Transaction has been recieved\nCoins will reflect to your account once it is completed!", true);
            }




        }
        catch (Exception e)
        {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Transaction Has Been Failed", true);
            Debug.Log(e, this);
        }
    }
    #endregion

    #region NonBurnNFTBuy
    async public void NonBurnNFTBuyContract(int _no, string _uri)
    {


        //string uri = "ipfs://bafyreifebcra6gmbytecmxvmro3rjbxs6oqosw3eyuldcwf2qe53gbrpxy/metadata.json";

        //  Debug.Log("Non Burn NFT Buy  " + _no + "URI : " + _uri);

        object[] inputParams = { _no, _uri };

        string method = "buyNonBurnItem"; // buyBurnItem";// "buyCoins";

        // array of arguments for contract
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        // value in wei
        string value = "";// Convert.ToDecimal(wei).ToString();
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to update contract state
        try
        {

#if !UNITY_EDITOR && !UNITY_ANDROID
                string response = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
                Debug.Log(response);
#else
            //string response = await EVM.Call(chain, network, contract, abi, args, method, args);
            //Debug.Log(response);
            string data = await EVM.CreateContractData(abi, method, args);
            string response = await Web3Wallet.SendTransaction(chainId, contract, "0", data, gasLimit, gasPrice);
            Debug.Log(response);

#endif


            if (CovalentManager.insta)
            {
                CovalentManager.insta.GetNFTUserBalance();
            }
            if (StoreManager.insta)
            {
                StoreManager.insta.DeductCoins(DatabaseManager.Instance.allMetaDataServer[_no].cost);
                StoreManager.insta.DisaleLastSelectedButton();
            }
            if (MyNFTCollection.insta)
            {
                await CheckPuzzleList();
                MyNFTCollection.insta.SetNewData();
            }

            if (MessaeBox.insta) MessaeBox.insta.showMsg("Your Transaction has been recieved\nIt will reflect to your account once it is completed!", true);



        }
        catch (Exception e)
        {
            Debug.Log(e, this);
            if (MessaeBox.insta)
            {

                MessaeBox.insta.showMsg("Server Error", true);

            }

        }
    }
    async public void NonBurnNFTPuzzleBuyContract(string _uri)
    {


        //string uri = "ipfs://bafyreifebcra6gmbytecmxvmro3rjbxs6oqosw3eyuldcwf2qe53gbrpxy/metadata.json";

        object[] inputParams = { _uri };

        string method = "mintPuzzleNFTItem"; // buyBurnItem";// "buyCoins";

        // array of arguments for contract
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        // value in wei
        string value = "";// Convert.ToDecimal(wei).ToString();
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to update contract state
        try
        {

#if !UNITY_EDITOR && !UNITY_ANDROID
                string response = await Web3GL.SendContract(method, abi, contract, args, value, gasLimit, gasPrice);
                Debug.Log(response);
#else
            //string response = await EVM.Call(chain, network, contract, abi, args, method, args);
            //Debug.Log(response);
            string data = await EVM.CreateContractData(abi, method, args);
            string response = await Web3Wallet.SendTransaction(chainId, contract, "0", data, gasLimit, gasPrice);
            Debug.Log(response);

#endif
            if (CovalentManager.insta)
            {
                CovalentManager.insta.GetNFTUserBalance();
            }

            if (MessaeBox.insta) MessaeBox.insta.showMsg("Your Transaction has been recieved\nIt will reflect to your account once it is completed!", true);



        }
        catch (Exception e)
        {
            Debug.Log(e, this);
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Server Error", true);

        }
    }
    #endregion


    #region CheckTime
    public async Task<string> CheckTimeStatus()
    {
        // smart contract method to call
        string method = "getCurrentTime";
        // array of arguments for contract
        object[] inputParams = { };
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        try
        {
            string response = await EVM.Call(chain, network, contract, abi, method, args);
            Debug.Log(response);
            return response;

        }
        catch (Exception e)
        {
            Debug.Log(e, this);
            return "";
        }
    }
    public long GetCurrentTime()
    {
        long currentEpoch = DateTimeOffset.Now.ToUnixTimeSeconds();


        // DateTime currentTime= ConvertEpochToDatatime(currentEpoch);
        return currentEpoch;
    }
    public void SaveNewClaimTime()
    {
        long currentEpoch = DateTimeOffset.Now.ToUnixTimeSeconds();
        DatabaseManager.Instance.GetLocalData().lastClaimedTime = currentEpoch.ToString();
        DatabaseManager.Instance.UpdateData();


        // DateTime currentTime= ConvertEpochToDatatime(currentEpoch);




    }



    public List<string> nftList = new List<string>();

    public async Task<string> CheckPuzzleList()
    {
        // smart contract method to call
        nftList = new List<string>();
        nftList.Clear();
        string method = "GetAllUserToken";
        // array of arguments for contract
        object[] inputParams = { PlayerPrefs.GetString("Account") };
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        Debug.Log("CheckPuzzleList ===================");
        try
        {
            string response = await EVM.Call(chain, network, contract, abi, method, args, networkRPC);
            Debug.Log("CheckPuzzleList =================== Now");
            Debug.Log(response);
            string[] splitArray = response.Split(char.Parse(",")); //return one word for each string in the array
                                                                   //here, splitArray[0] = Give; splitArray[1] = me etc...

            for (int i = 0; i < splitArray.Length; i++)
            {
                if (string.IsNullOrEmpty(splitArray[i])) continue;
                nftList.Add(splitArray[i]);
            }


            return response;

        }
        catch (Exception e)
        {
            Debug.Log(e, this);
            return "";
        }
    }
    #endregion

    #region CheckNFTBalance

    public string balanceNFT;

    #endregion

    #region CheckUserBalance
    public async UniTaskVoid CheckUserBalance()
    {
        COMEHERE:
        try
        {

            string response = await EVM.BalanceOf(chain, network, PlayerPrefs.GetString("Account"), networkRPC);
            Debug.Log("CheckUserBalance " + response);
            if (!string.IsNullOrEmpty(response))
            {
                float wei = float.Parse(response);
                float decimals = 1000000000000000000; // 18 decimals
                float eth = wei / decimals;
                // print(Convert.ToDecimal(eth).ToString());
                Debug.Log(Convert.ToDecimal(eth).ToString());
                userBalance = Convert.ToDecimal(eth).ToString();
                if (StoreManager.insta)
                {
                    StoreManager.insta.SetBalanceText();
                }
                UIManager.insta.UpdateBalance();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e, this);
        }

        await UniTask.Delay(UnityEngine.Random.Range(5200, 12500));
        goto COMEHERE;
    }
    #endregion

    #region CheckTRansactionStatus
    //private string transID;
    public static string userTokenBalance = "0";

    public async UniTaskVoid CheckTransactionStatus(string _tranID)
    {
        bool NoCheckAgain = false;
        COMEHERE:
        await UniTask.Delay(UnityEngine.Random.Range(4000, 10000));
        try
        {
            string txConfirmed = await EVM.TxStatus(chain, network, _tranID, networkRPC);
            print(txConfirmed); // success, fail, pending
            if (txConfirmed.Equals("success") || txConfirmed.Equals("fail"))
            {
                NoCheckAgain = true;
                // NonBurnNFTBuyContract(0, "ipfs://bafyreigkpnryq6t53skpbmfylegrp7wl3xkegzxq7ogimvnkzdceisya4a/metadata.json");
                // CancelInvoke("CheckTransactionStatus");
                if (DatabaseManager.Instance)
                {
                    DatabaseManager.Instance.ChangeTransactionStatus(_tranID, txConfirmed);
                }

            }

        }
        catch (Exception e)
        {
            Debug.Log(e, this);
        }

        if (!NoCheckAgain) goto COMEHERE;
    }


    #endregion

    #region getMetaData
    async public void getMetaData()
    {

        try
        {
            string response = await ERC1155.URI(chain, network, contract, "400");
            Debug.Log(response);
        }
        catch (Exception e)
        {
            Debug.Log(e, this);
        }
    }
    #endregion

    #region NFTUploaded

    public void purchaseItem(int _id, bool _skin)
    {
        Debug.Log("purchaseItem");
        if (MessaeBox.insta) MessaeBox.insta.showMsg("NFT purchase process started\nThis can up to minute", false);
        if (!_skin) NonBurnNFTBuyContract(_id, "ignore");
        return;

        MetadataNFT meta = new MetadataNFT();


        meta.itemid = DatabaseManager.Instance.allMetaDataServer[_id].itemid;
        meta.name = DatabaseManager.Instance.allMetaDataServer[_id].name;
        meta.description = DatabaseManager.Instance.allMetaDataServer[_id].description;
        meta.image = DatabaseManager.Instance.allMetaDataServer[_id].imageurl;

        StartCoroutine(UploadNFTMetadata(Newtonsoft.Json.JsonConvert.SerializeObject(meta), _id, _skin));

    }
    public void uploadPuzzleNFT(string jsonData, string url)
    {
        Debug.Log("purchaseItem");

        MetadataNFT meta = new MetadataNFT();


        meta.jsonData = jsonData;
        meta.name = "Puzzle";
        meta.description = "Puzzle Desc.";
        meta.image = url;

        StartCoroutine(UploadNFTPuzzleMetadata(Newtonsoft.Json.JsonConvert.SerializeObject(meta)));
        //NonBurnNFTPuzzleBuyContract(url);
    }
    IEnumerator UploadNFTMetadata(string _metadata, int _id, bool _skin)
    {
        if (MessaeBox.insta) MessaeBox.insta.showMsg("NFT purchase process started\nThis can up to minute", false);
        //if (!_skin) NonBurnNFTBuyContract(_id, "ignore");
        yield break;

        Debug.Log("Creating and saving metadata to IPFS..." + _metadata);
        Debug.Log("Sending ID To SERVER " + _id);
        WWWForm form = new WWWForm();
        form.AddField("meta", _metadata);

        using (UnityWebRequest www = UnityWebRequest.Post("https://api.nft.storage/store", form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + ConstantManager.nftStorage_key);
            www.timeout = 40;
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                Debug.Log("UploadNFTMetadata upload error " + www.downloadHandler.text);


                if (MessaeBox.insta) MessaeBox.insta.showMsg("Server error\nPlease try again", true);

                www.Abort();
                www.Dispose();
            }
            else
            {
                Debug.Log("UploadNFTMetadata upload complete! " + www.downloadHandler.text);

                JSONObject j = new JSONObject(www.downloadHandler.text);
                if (j.HasField("value"))
                {
                    //Debug.Log("Predata " + j.GetField("value").GetField("ipnft").stringValue);
                    SingletonDataManager.nftmetaCDI = j.GetField("value").GetField("url").stringValue; //ipnft
                    //SingletonDataManager.tokenID = j.GetField("value").GetField("ipnft").stringValue; //ipnft
                    Debug.Log("Metadata saved successfully");

                    if (!_skin) NonBurnNFTBuyContract(_id, j.GetField("value").GetField("url").stringValue);
                }
            }
        }
    }
    IEnumerator UploadNFTPuzzleMetadata(string _metadata)
    {
        if (MessaeBox.insta) MessaeBox.insta.showMsg("NFT purchase process started\nThis can up to minute", false);
        Debug.Log("Creating and saving metadata to IPFS..." + _metadata);
        WWWForm form = new WWWForm();
        form.AddField("meta", _metadata);

        using (UnityWebRequest www = UnityWebRequest.Post("https://api.nft.storage/store", form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + ConstantManager.nftStorage_key);
            www.timeout = 40;
            yield return www.SendWebRequest();



            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                Debug.Log("UploadNFTMetadata upload error " + www.downloadHandler.text);


                if (MessaeBox.insta) MessaeBox.insta.showMsg("Server error\nPlease try again", true);

                www.Abort();
                www.Dispose();
            }
            else
            {
                Debug.Log("UploadNFTMetadata upload complete! " + www.downloadHandler.text);

                JSONObject j = new JSONObject(www.downloadHandler.text);
                if (j.HasField("value"))
                {
                    //Debug.Log("Predata " + j.GetField("value").GetField("ipnft").stringValue);
                    SingletonDataManager.nftmetaCDI = j.GetField("value").GetField("url").stringValue; //ipnft
                    //SingletonDataManager.tokenID = j.GetField("value").GetField("ipnft").stringValue; //ipnft
                    Debug.Log("Metadata saved successfully");
                    //PurchaseItem(cost, _id);
                    NonBurnNFTPuzzleBuyContract(j.GetField("value").GetField("url").stringValue);
                }
            }
        }
    }
    #endregion

    #region Token

    async public void ExchangeToken(int _pack)
    {

        if (MessaeBox.insta) MessaeBox.insta.showMsg("Exchange token process started", false);

        float decimals = 1000000000000000000; // 18 decimals
        float wei = (_pack) * decimals;

        object[] inputParams = { contractToken, Convert.ToDecimal(wei).ToString() };


        // smart contract method to call
        string method = "transfer";

        // array of arguments for contract
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        // value in wei
        string value = "0";
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to update contract state
        try
        {


#if !UNITY_EDITOR && !UNITY_ANDROID
            string response = await Web3GL.SendContract(method, abiToken, contractToken, args, value, gasLimit, gasPrice);
            Debug.Log(response);
#else
            // string response = await EVM.c(method, abi, contract, args, value, gasLimit, gasPrice);
            // Debug.Log(response);
            string data = await EVM.CreateContractData(abiToken, method, args);
            string response = await Web3Wallet.SendTransaction(chainId, contractToken, value, data, gasLimit, gasPrice);


            Debug.Log(response);
#endif



            //if (MessaeBox.insta) MessaeBox.insta.showMsg("Coin exchanged successfully", true);

            if (!string.IsNullOrEmpty(response))
            {
                if (DatabaseManager.Instance)
                {
                    DatabaseManager.Instance.AddTransaction(response, "pending", _pack - 1);
                }

                CheckTransactionStatus(response);
                if (MessaeBox.insta) MessaeBox.insta.showMsg("Your Transaction has been recieved\nCoins will reflect to your account once it is completed!", true);
            }



        }
        catch (Exception e)
        {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Transaction Has Been Failed", true);
            Debug.Log(e, this);
        }
    }
    public async static UniTaskVoid getTokenBalance()
    {
        COMEHERE:
        // smart contract method to call
        string method = "balanceOf";
        // array of arguments for contract
        object[] inputParams = { PlayerPrefs.GetString("Account") };
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        try
        {
            string response = await EVM.Call(chain, network, contractToken, abiToken, method, args, networkRPC);
            Debug.Log(response);
            try
            {
                float wei = float.Parse(response);
                float decimals = 1000000000000000000; // 18 decimals
                float eth = wei / decimals;
                // print(Convert.ToDecimal(eth).ToString());
                var tokenBalance = Convert.ToDecimal(eth).ToString();
                userTokenBalance = tokenBalance;
                Debug.Log("Token Bal : " + Convert.ToDecimal(eth).ToString() + " | " + response);

                UIManager.insta.UpdateBalance();
            }
            catch (Exception)
            {
            }


        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        await UniTask.Delay(UnityEngine.Random.Range(5000, 12000));
        goto COMEHERE;

    }
    async public void getDailyToken()
    {

        if (MessaeBox.insta) MessaeBox.insta.showMsg("Claiming Token! This may take some time please wait!", false);

        object[] inputParams = { };
        string method = "GetGameToken"; // buyBurnItem";// "buyCoins";

        // array of arguments for contract
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        // value in wei
        string value = "";// Convert.ToDecimal(wei).ToString();
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        string response = "";
        // connects to user's browser wallet (metamask) to update contract state
        try
        {

#if !UNITY_EDITOR && !UNITY_ANDROID
                response = await Web3GL.SendContract(method, abiToken, contractToken, args, value, gasLimit, gasPrice);
                Debug.Log(response);
#else
            string data = await EVM.CreateContractData(abiToken, method, args);
            response = await Web3Wallet.SendTransaction(chainId, contractToken, "0", data, gasLimit, gasPrice);
            Debug.Log(response);
#endif

        }
        catch (Exception e)
        {
            Debug.Log("error" + e);
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Server Error", true);
            return;
        }

        if (!string.IsNullOrEmpty(response))
        {
            MessaeBox.insta.showMsg("Token will be credited soon", true);
            if (UIManager.insta) UIManager.insta.gameoverUI.transform.GetChild(1).transform.gameObject.SetActive(false);

        }
        else
        {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Server Error", true);
            Debug.Log("In check blank");
        }

    }


    //New methods starts from here

    public async UniTask<string> getStakingValue()
    {
        // smart contract method to call
        string method = "getPotentialReward";
        // array of arguments for contract
        object[] inputParams = { PlayerPrefs.GetString("Account") };
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        try
        {
            string response = await EVM.Call(chain, network, contractToken, abiToken, method, args, networkRPC);
            Debug.Log(response);
            try
            {
                float wei = float.Parse(response);
                float decimals = 1000000000000000000; // 18 decimals
                float eth = wei / decimals;
                // print(Convert.ToDecimal(eth).ToString());
                var tokenBalance = Convert.ToDecimal(eth).ToString();

                Debug.Log("getPotentialReward : " + Convert.ToDecimal(eth).ToString() + " | " + response);
                return Convert.ToDecimal(eth).ToString();

            }
            catch (Exception)
            {
                return "No Token Staked";
            }


        }
        catch (Exception e)
        {
            Debug.Log(e);
            return "No Token Staked";
        }

    }

    public async UniTask<string> getTotalStaked()
    {
        // smart contract method to call
        string method = "getTotalStaked";
        // array of arguments for contract
        object[] inputParams = { PlayerPrefs.GetString("Account") };
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        try
        {
            string response = await EVM.Call(chain, network, contractToken, abiToken, method, args, networkRPC);
            Debug.Log(response);
            try
            {
                float wei = float.Parse(response);
                float decimals = 1000000000000000000; // 18 decimals
                float eth = wei / decimals;
                // print(Convert.ToDecimal(eth).ToString());
                var tokenBalance = Convert.ToDecimal(eth).ToString();

                Debug.Log("getTotalStaked : " + Convert.ToDecimal(eth).ToString() + " | " + response);
                return Convert.ToDecimal(eth).ToString();

            }
            catch (Exception)
            {
                return "No Token Staked";
            }


        }
        catch (Exception e)
        {
            Debug.Log(e);
            return "No Token Staked";
        }

    }

    public async UniTask<string> getYearlyReturnRate()
    {
        // smart contract method to call
        string method = "yearlyReturnRate";
        // array of arguments for contract
        object[] inputParams = { };
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        try
        {
            string response = await EVM.Call(chain, network, contractToken, abiToken, method, args, networkRPC);
            Debug.Log(response);
            try
            {

                Debug.Log("yearlyReturnRate : " + response);
                return response;

            }
            catch (Exception)
            {
                return "Try After Sometime";
            }


        }
        catch (Exception e)
        {
            Debug.Log(e);
            return "Try After Sometime";
        }

    }

    async public UniTaskVoid StakeToken(float _value)
    {
        if (MessaeBox.insta) MessaeBox.insta.showMsg("Staking " + _value + " Tokens! This may take some time please wait!", true);

        float decimals = 1000000000000000000; // 18 decimals
        float wei = _value * decimals; //enter value here

        object[] inputParams = { Convert.ToDecimal(wei).ToString() };


        // smart contract method to call
        string method = "stake";

        // array of arguments for contract
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        // value in wei
        string value = "0";

        // connects to user's browser wallet (metamask) to update contract state
        try
        {


#if !UNITY_EDITOR && !UNITY_ANDROID
            string response = await Web3GL.SendContract(method, abiToken, contractToken, args, value);
            Debug.Log(response);
#else
            // Debug.Log(response);
            string data = await EVM.CreateContractData(abiToken, method, args);
            string response = await Web3Wallet.SendTransaction(chainId, contractToken, value, data);


            Debug.Log(response);
#endif
            //if (MessaeBox.insta) MessaeBox.insta.showMsg("Coin exchanged successfully", true);

            if (!string.IsNullOrEmpty(response))
            {
                if (MessaeBox.insta) MessaeBox.insta.showMsg("Token Staked Successfully", true);
            }

        }
        catch (Exception e)
        {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Transaction Has Been Failed", true);
            Debug.Log(e, this);
        }
    }

    async public UniTaskVoid UnstakeToken()
    {
        if (MessaeBox.insta) MessaeBox.insta.showMsg("Untaking Tokens! This may take some time please wait!", true);
        object[] inputParams = { };

        // smart contract method to call
        string method = "unstake";

        // array of arguments for contract
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        // value in wei
        string value = "0";

        // connects to user's browser wallet (metamask) to update contract state
        try
        {


#if !UNITY_EDITOR && !UNITY_ANDROID
            string response = await Web3GL.SendContract(method, abiToken, contractToken, args, value);
            Debug.Log(response);
#else
            // Debug.Log(response);
            string data = await EVM.CreateContractData(abiToken, method, args);
            string response = await Web3Wallet.SendTransaction(chainId, contractToken, value, data);


            Debug.Log(response);
#endif
            //if (MessaeBox.insta) MessaeBox.insta.showMsg("Coin exchanged successfully", true);

            if (!string.IsNullOrEmpty(response))
            {
                if (MessaeBox.insta) MessaeBox.insta.showMsg("Token UnStaked Successfully", true);
            }

        }
        catch (Exception e)
        {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Transaction Has Been Failed", true);
            Debug.Log(e, this);
        }
    }

    #endregion


}
