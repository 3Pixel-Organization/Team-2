﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class UIGameplayManager : MonoBehaviour
{
    public static UIGameplayManager Instance;

    [Header("Player Control")]
    public Joystick joystick;
    public TouchFieldDrag touchField;
    public SkillButton skillMeleeButton;
    public SkillButton skillGunButton;
    public SkillButton skillBodyButton;

    [Header("Player Stat")]
    public Image playerImage;
    public Slider expBar;
    public TextMeshProUGUI expBarText;
    public Slider healthBar;
    public TextMeshProUGUI healthBarText;
    public Slider shieldBar;
    public TextMeshProUGUI shieldBarText;
    public Slider defenseBar;
    public TextMeshProUGUI defenseBarText;

    [Space]
    public TextMeshProUGUI levelText;
    public Button attackButton;
    public TextMeshProUGUI ammoAmountText;

    public Slider reloadSlider;
    public bool reloading;
    float reloadTime;
    float gameTime;


    [Header("Wave")]
    public TextMeshProUGUI waitingWaveStartText;
    public TextMeshProUGUI waveState;

    [Space]
    public GameObject pausePanel;
    public UITempInventory tempInventory;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[UIGameplayManager] There is more then one player Instance");
            return;
        }
        Instance = this;
    }

    void Start()
    {
        InitSkillControl();
        UpdatePlayerStatUI();

        pausePanel.SetActive(false);


        reloading = false;
    }
    private void Update()
    {
        if (reloading)
        {
            ReloadSlider(reloadTime, Time.time);
        }
    }

    void InitSkillControl()
    {
        skillMeleeButton.InitSkillButton(SkillManager.Instance.meleeSkill);
        skillGunButton.InitSkillButton(SkillManager.Instance.gunSkill);
        skillBodyButton.InitSkillButton(SkillManager.Instance.bodySkill);
    }

    //button
    public void Shoot()
    {
        PlayerManager.Instance.Shooting();
    }

    public void Reload()
    {
        PlayerManager.Instance.ReloadWithTimer();
    }

    public void UpdatePlayerStatUI()
    {
        PlayerStat playerStat = PlayerManager.Instance.playerStat;

        playerImage.sprite = playerStat.playerStat.playerIcon;
        levelText.text = playerStat.currentLevel.ToString();

        expBarText.text = playerStat.currentExp + " / " + playerStat.MaxExp();
        expBar.maxValue = playerStat.MaxExp();
        expBar.value = playerStat.currentExp;

        int maxHealth = (int)PlayerStatManager.Instance.GetTotalAttributeLevel(TypeOfAttributes.HEALTH);
        healthBarText.text = playerStat.currentHealth + " / " + maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = playerStat.currentHealth;

        int maxShield = (int)PlayerStatManager.Instance.GetTotalAttributeLevel(TypeOfAttributes.SHIELD);
        shieldBarText.text = playerStat.currentShield + " / " +  maxShield;
        shieldBar.maxValue = maxShield;
        shieldBar.value = playerStat.currentShield;

        int maxDefense = (int)PlayerStatManager.Instance.GetTotalAttributeLevel(TypeOfAttributes.DEFENCE);
        defenseBarText.text = playerStat.currentDefense + " / " + maxDefense;
        defenseBar.maxValue = maxDefense;
        defenseBar.value = playerStat.currentDefense;
    }

    public void UpdateAmmoText(PlayerStat playerStat)
    {
        int maxAmmo = EquipmentManager.Instance.GetEquipment(EquipmentType.WEAPON).magazineSize;
        ammoAmountText.text = playerStat.currentAmmo + " / " + maxAmmo;
    }
    public void ReloadSlider(float timer, float currentTime)
    {
        if (reloading == true)
        {
            float time = currentTime - gameTime;
            if (time < reloadTime)
            {
                reloadSlider.value = time;
            }
            else
            {
                reloading = false;
                reloadSlider.gameObject.SetActive(false);
            }
        }
        else
        {
            reloading = true;
            gameTime = currentTime;
            reloadSlider.gameObject.SetActive(true);
            reloadSlider.maxValue = timer;
            reloadSlider.value = timer;
            reloadTime = timer;
        }
    }

    //button
    public void OpenPanelPause()
    {
        pausePanel.SetActive(true);
        tempInventory.InitInventory();
        Time.timeScale = 0;
    }

    //button
    public void ClosePanelPause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
