using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedScrollingText : MonoBehaviour, IDestructable
{

    public ScrollingText Text;
    public Color textColor;
    Vector3 offset = Vector3.zero;
    public void OnDestruct(GameObject destroyer)
    {
        if(TryGetComponent(out EnemyStat enemyStat))
        {
            var text = "+ " + enemyStat.enemyStat.expGivenOnDeath.ToString() + " EXP";
            var random = Random.Range(1, 10);
            if (random >= 6)
            {
                offset.x = 1;
            }
            else
            {
                offset.x = -1;
            }
            var position = destroyer.transform.position + offset;
            var scrollingText = Instantiate(Text, position, Quaternion.identity);
            scrollingText.SetText(text);
            scrollingText.SetColor(textColor);
        }
    }
}
