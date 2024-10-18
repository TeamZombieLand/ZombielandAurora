using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{    

    [SerializeField] List<LevelDetails> levels=new List<LevelDetails>();
    public static List<LevelDetails> levelsData=new List<LevelDetails>();
    
    [SerializeField] Transform levels_button_content;
    [SerializeField] GameObject levelPrefabButton;

    [SerializeField] GameObject titleImage;
    
    private void OnEnable()
    {
        SetLevelsButtonInteractable();
    }
    private void Start()
    {      

        levelsData = levels;
        for (int i = 0; i < levels.Count; i++)
        {
            int temp = i;
            GameObject go = Instantiate(levelPrefabButton, levels_button_content);
            go.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = (temp + 1).ToString();
            go.GetComponent<Button>().onClick.AddListener(() => {
                PlayLevel(temp);
            });
           // go.t
        }
        this.gameObject.SetActive(false);
    }


    private void PlayLevel(int index)
    {
       
        UIManager.insta.missionMode = true;
        UIManager.insta.StartGame(levels[index]);
        titleImage.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void SetLevelsButtonInteractable()
    {
        if (DatabaseManager.Instance != null)
        {
            int currenct_level=DatabaseManager.Instance.GetLocalData().current_level;
            for (int i = 0; i < levels_button_content.childCount; i++)
            {
                levels_button_content.GetChild(i).GetComponent<Button>().interactable = false;
                if (currenct_level >= i)
                {
                    levels_button_content.GetChild(i).GetComponent<Button>().interactable = true;
                }
            }
        }
       
    }    
}

[System.Serializable]
public class LevelDetails
{
    public int level_no;
    public LEVEL_TYPE level_type;
    public int kill_count;
    public int headshot_count;    
    public float level_time;   
    public float hp_percentage;
    public List<ItemsCount> needToCollected= new List<ItemsCount>();

    public LevelDetails(LevelDetails _temp)
    {
        level_no=_temp.level_no;
        level_type = _temp.level_type;
        kill_count = _temp.kill_count;
        headshot_count = _temp.headshot_count;
        level_time = _temp.level_time;
        hp_percentage = _temp.hp_percentage;
        needToCollected = new List<ItemsCount>();
        for (int i = 0; i < _temp.needToCollected.Count; i++)
        {
            needToCollected.Add(new ItemsCount(_temp.needToCollected[i]));
        }
        
    }

}

[System.Serializable]
public class ItemsCount
{
    public ItemType type;
    public int count;
    public GameObject collectedPrefab;
    public ItemsCount(ItemsCount _temp)
    {
        type=_temp.type;
        count=_temp.count;
        collectedPrefab = _temp.collectedPrefab;
    }
}

public enum LEVEL_TYPE
{
    SURVIVAL,KILLCOUNT,KILLCOUNT_TIMER,HPSAVE_TIMER,HEADSHOT,FINDKEY
}
