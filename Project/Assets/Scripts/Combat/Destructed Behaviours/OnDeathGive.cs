using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathGive : MonoBehaviour, IDestructable
{
    public void OnDestruct(GameObject destroyer)
    {
        int exp = 0;
        if (TryGetComponent<EnemyStat>(out EnemyStat enemyStat))
        {
            exp = enemyStat.enemyStat.expGivenOnDeath;
        }
        if(destroyer.TryGetComponent<PlayerStat>(out PlayerStat playerStat))
        {
            playerStat.GiveExp(exp);
        }
    }
}
