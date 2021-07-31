using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item_SO : ScriptableObject
{
    public string id;
    public EquipmentType equipmentType;

    public GameObject itemObject;

    public string itemName;
    public Sprite itemSprite;

    public int itemAmount = 0;
    public bool showInStore = true;
    public bool isStackable = false;
    public bool isConsumable = false;

    public int stackSize = 0;
    public int maxStackSize = 20;

    [Header("Store Cost")]
    public int sellCost;
    public int buyCost;

    [Header("Equipment Stat")]
    public int health;
    public int shield;
    public int defensePower;
    public int attackPower;
    public float speed;
    [Range(0, 1)] public float critChance;
    [Range(0, 1)] public float critMultiplier;

    [Header("Weapon Stat")]
    public float attackInterval;
    public int magazineSize;
    public float attackRange;

    public void UseItem()
    {
        Debug.Log("[Item_SO] Using the item: " + itemName);
        EquipmentManager.Instance.EquipItem(this);

        if (stackSize >= 1 && isStackable)
        {
            stackSize--;
        }
    }

    public Dictionary<string, float> GetStats()
    {
        Dictionary<string, float> temps = new Dictionary<string, float>();

        if  (health > 0) temps.Add("Health", health);
        if  (shield > 0) temps.Add("Shield", shield);
        if  (defensePower > 0) temps.Add("Defense Power", defensePower);
        if  (attackPower > 0) temps.Add("Attack Power", attackPower);
        if  (critChance > 0) temps.Add("Crit Chance", critChance);
        if  (critMultiplier > 0) temps.Add("Crit Multiplier", critMultiplier);
        if  (speed < 0) temps.Add("Speed", speed);

        if (attackInterval > 0) temps.Add("Attack Interval", attackInterval);
        if (magazineSize > 0) temps.Add("Magazine Size", magazineSize);
        if (attackRange > 0) temps.Add("Attack Range", attackRange);

        return temps;
    }

    public float GetItemAttribute(TypeOfAttributes typeOfAttributes)
    {
        switch (typeOfAttributes)
        {
            case TypeOfAttributes.HEALTH:
                return health;
            case TypeOfAttributes.SHIELD:
                return shield;
            case TypeOfAttributes.ATTACK:
                return attackPower;
            case TypeOfAttributes.DEFENCE:
                return defensePower;
            case TypeOfAttributes.CRITCHANCE:
                return critChance;
            case TypeOfAttributes.CRITMULTIPLIER:
                return critMultiplier;
            case TypeOfAttributes.SPEED:
                return speed;
            default:
                return 0;
        }
    }
}

/*use items
 *     public float givearmor = 0;
public override void Use() //Armor Use Effect
{
    Debug.Log("You used Armor Item."); //will be removed after tests and bugfixes

    Armor playerArmor = GameObject.Find("ArmorBar").GetComponent<Armor>();
    if (playerArmor.currentArmor >= 100)
    {
        Debug.Log("You have full armor. You cant use Armor");
    }
    else
    {
        playerArmor.GiveArmor(givearmor);
        _Inventory.Instance.Remove(this);
    }

}    public float heal=0;

public override void Use()
{
    //will be removed after tests and bugfixes
    Debug.Log("You used Health Item.");


    Health playerHealth = GameObject.Find("HealthBar").GetComponent<Health>();
    if(playerHealth.currentHealth >= 100)
    {
        Debug.Log("You have full health. You cant use Medkit");
    }
    else
    {
        playerHealth.Heal(heal);
        _Inventory.Instance.Remove(this);
    }


}
public override void Use() //Ammo Use Effect
{
    //Health playerHealth = GameObject.Find("HealthBar").GetComponent<Health>();
    //playerHealth.Heal(heal);
    //_Inventory.Instance.Remove(this);

    Debug.Log("You used Ammo Item.");

}
}

*/

public enum EquipmentType
{
    HELMET,
    CHEST,
    PANTS,
    BOOTS,
    HANDS,
    WEAPON,
    SIDEARM
}

public enum WeaponType
{
    PISTOL,
    RIFLE,
    MELEE
}