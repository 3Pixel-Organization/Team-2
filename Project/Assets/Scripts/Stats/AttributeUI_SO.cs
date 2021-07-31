using UnityEngine;

[CreateAssetMenu(fileName = " New UI Attribute", menuName ="Character Stats")]
public class AttributeUI_SO : ScriptableObject
{
    [Header("Name of Attribute:")]
    public string attributeName;
    public Sprite attributeIcon;
    [TextArea] public string desc;

    [Header("Values of the Attribute:")]
    public int maxValue;
    public float addValue;
    public TypeOfAttributes attributeType = TypeOfAttributes.HEALTH;

}
