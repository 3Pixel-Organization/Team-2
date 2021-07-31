using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBaseStat : MonoBehaviour
{
    public Image playerImage;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI pointText;
    public Image xpBar;

    public List<AttributeUI> baseAttributes = new List<AttributeUI>();
    public List<AttributeUI> weaponAttributes = new List<AttributeUI>();
    public List<AttributeUI> totalAttributes = new List<AttributeUI>();

    PlayerData playerData => SaveManager.Instance.playerData;

    public void InitStat()
    {
        playerImage.sprite = PlayerStatManager.Instance.playerStat.playerIcon;
        playerNameText.text = playerData.playerName;
        levelText.text = playerData.playerLevel.ToString();
        xpText.text = playerData.playerXP.ToString();
        xpBar.fillAmount = (float)playerData.playerXP / (float)PlayerStatManager.Instance.MaxCurrentLevelExp();

        RefreshStat();
    }

    public void RefreshStat()
    {
        pointText.text = PlayerStatManager.Instance.GetAttributePoint().ToString();
        InitBaseStat();
        InitEquipmentStat();
        InitTotalStat();
    }

    void InitBaseStat()
    {
        foreach (var item in baseAttributes)
        {
            item.InitBaseAttribute();
        }
    }

    void InitEquipmentStat()
    {
        foreach (var item in weaponAttributes)
        {
            item.InitEquipmentAttribute();
        }
    }

    void InitTotalStat()
    {
        foreach (var item in totalAttributes)
        {
            item.InitTotalAttribute();
        }
    }
}
