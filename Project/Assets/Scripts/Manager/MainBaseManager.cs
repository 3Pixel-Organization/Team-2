using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBaseManager : MonoBehaviour
{
    public static MainBaseManager Instance;
    public UIBaseInventory inventory;
    public UIBaseSkill skill;
    public UIStore store;
    public UIBaseStat stats;
    public GameObject mission;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[InventoryManager] There is more then one inventory Instance");
            return;
        }
        Instance = this;
    }

    void Start(){
        inventory.gameObject.SetActive(true);
        inventory.InitInventory();
        store.gameObject.SetActive(false);
        skill.gameObject.SetActive(false);
        stats.gameObject.SetActive(false);
    }

    //open store
    public void StoreToggle(bool value){
        if(value){
            store.gameObject.SetActive(true);
            store.InitStore();
        }else
        {
            store.gameObject.SetActive(false);
        }
    }

    //open inventory
    public void InventoryToggle(bool value){
        if(value){
            inventory.gameObject.SetActive(true);
            inventory.InitInventory();
        }else
        {
            inventory.gameObject.SetActive(false);
        }
    }

    //open skills
    public void SkillToggle(bool value){
        if(value){
            skill.gameObject.SetActive(true);
            skill.InitSkillPanel();
        }else
        {
            skill.gameObject.SetActive(false);
        }
    }

    //open stats
    public void StatToggle(bool value){
        if(value){
            stats.gameObject.SetActive(true);
            stats.InitStat();
        }else
        {
            stats.gameObject.SetActive(false);
        }
    }

    //open mission
    public void MissionToggle(bool value){
        if(value){
            mission.gameObject.SetActive(true);
        }else
        {
            mission.gameObject.SetActive(false);
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
