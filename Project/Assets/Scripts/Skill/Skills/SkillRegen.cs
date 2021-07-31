using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRegen : Skill
{
    public int regenAmount;

    public override void ActivateSkill()
    {
        base.ActivateSkill();

        Debug.Log("active skill regen skill");

        PlayerManager.Instance.AddHealth(regenAmount, out _);

        DeactivateSkill();
    }
}
