using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemStat : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI valueText;

    public void InitValue(string _name, float _value)
    {
        nameText.text = _name;
        valueText.text = _value.ToString();
    }
}
