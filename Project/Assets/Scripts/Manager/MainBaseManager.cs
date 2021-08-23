using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBaseManager : MonoBehaviour
{
    public static MainBaseManager Instance;
    public GameObject inventoryUI;
    public GameObject skillUI;
    public GameObject storeUI;
    public GameObject statsUI;
    public GameObject baseMenuUI;
    public GameObject playUI;
    public GameObject levelUI;
    public GameObject missionUI;



    [Header("Inscript getters")]
    public UIBaseInventory inventory;
    public UIBaseSkill skill;
    public UIStore store;
    public UIBaseStat stats;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[MainBaseManager] There is more then one Main Base Instance");
            return;
        }
        Instance = this;

        inventory = inventoryUI.GetComponent<UIBaseInventory>();
        store = storeUI.GetComponent<UIStore>();
        skill = skillUI.GetComponent<UIBaseSkill>();
        stats = statsUI.GetComponent<UIBaseStat>();
    }

    void Start()
    {
        Toggle(baseMenuUI);
        //inventory.InitInventory();
    }

    //sets all other menu UI elemets to false and executes specific UI initializing methods
    public void Toggle(GameObject obj)
    {
        if (obj == baseMenuUI)
        {
            inventoryUI.gameObject.SetActive(false);
            storeUI.gameObject.SetActive(false);
            skillUI.gameObject.SetActive(false);
            statsUI.gameObject.SetActive(false);
            playUI.gameObject.SetActive(false);
            levelUI.gameObject.SetActive(false);
            baseMenuUI.gameObject.SetActive(true);
            return;
        }

        if (obj.gameObject.activeSelf)
        {
            inventoryUI.gameObject.SetActive(false);
            storeUI.gameObject.SetActive(false);
            skillUI.gameObject.SetActive(false);
            statsUI.gameObject.SetActive(false);
            playUI.gameObject.SetActive(false);
            levelUI.gameObject.SetActive(false);
            baseMenuUI.gameObject.SetActive(true);
        }
        else
        {
            inventoryUI.gameObject.SetActive(false);
            storeUI.gameObject.SetActive(false);
            skillUI.gameObject.SetActive(false);
            statsUI.gameObject.SetActive(false);
            baseMenuUI.gameObject.SetActive(false);
            playUI.gameObject.SetActive(false);
            levelUI.gameObject.SetActive(false);

            obj.gameObject.SetActive(true);

            if (obj == inventoryUI)
            {
                //inventory.InitInventory();
            }
            else if (obj == skillUI)
            {
                //skill.InitSkillPanel();
            }
            else if (obj == statsUI)
            {
                //stats.InitStat();
            }
            else if (obj == storeUI)
            {
                //store.InitStore();
            }
            else if (obj == playUI)

            {
                //displays the "choose level UI"
            }
            else if (obj == levelUI)

            {
                //display level info and the start button
                //needs to do the re-writing of the level info based on level SO (needs to be created).

            }
        }
    }

    //open mission when pressing the start button - requires a level SO
    public void MissionToggle(bool value){
        if(value){
            baseMenuUI.gameObject.SetActive(false);
            missionUI.gameObject.SetActive(true);
        }else
        {
            missionUI.gameObject.SetActive(false);
            baseMenuUI.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Go to game
    /// </summary>
    public void ChangeScenePlay()
    {
        GameManager.Instance.GoToGameScene();
    }
}
