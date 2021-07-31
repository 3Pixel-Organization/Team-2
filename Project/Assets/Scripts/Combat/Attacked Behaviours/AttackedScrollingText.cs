using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedScrollingText : MonoBehaviour, IAttackable
{
    public ScrollingText Text;
    public Color textColor;

    public void OnAttack(GameObject attacker, int damage)
    {
        var text = damage.ToString() + " DMG";
        var scrollingText = Instantiate(Text, transform.position, Quaternion.identity);
        scrollingText.SetText(text);
        scrollingText.SetColor(textColor);
    }
}
