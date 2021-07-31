using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttributeUI : MonoBehaviour
{

    public AttributeUI_SO attribute;
    public Button addButton;
    public Image attributeImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI valueText;

    public void InitBaseAttribute()
    {
        attributeImage.sprite = attribute.attributeIcon;
        titleText.text = attribute.attributeName;
        valueText.text = PlayerStatManager.Instance.GetAttributeLevel(attribute.attributeType, out bool maxed).ToString();

        if(addButton != null) addButton.interactable = (PlayerStatManager.Instance.GetAttributePoint() > 0);
        addButton.gameObject.SetActive(!maxed);
    }

    public void InitEquipmentAttribute()
    {
        attributeImage.sprite = attribute.attributeIcon;
        titleText.text = attribute.attributeName;
        valueText.text = EquipmentManager.Instance.GetEquipmentStat(attribute.attributeType).ToString();

        addButton.gameObject.SetActive(false);
    }

    public void InitTotalAttribute()
    {
        attributeImage.sprite = attribute.attributeIcon;
        titleText.text = attribute.attributeName;
        valueText.text = PlayerStatManager.Instance.GetTotalAttributeLevel(attribute.attributeType).ToString();

        addButton.gameObject.SetActive(false);
    }

    //button
    public void AddAttributeLevel()
    {
        if(PlayerStatManager.Instance.GetAttributePoint() > 0)
        {
            PlayerStatManager.Instance.AddAttributeLevel(attribute.attributeType);
            MainBaseManager.Instance.stats.RefreshStat();
        }
    }
}
