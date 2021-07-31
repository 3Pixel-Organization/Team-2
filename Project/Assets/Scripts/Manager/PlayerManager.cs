using System;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public PlayerStat playerStat;
    public PlayerAnimator playerAnimator;
    public AttackDefenition baseAttack;
    public ProjectileManager projectileManager;

    //shooting variables
    private bool readyToShoot = true;

    public bool isWeaponRaycast;

    public MissionInventory missionInventory;

    UIGameplayManager uiGameplay => UIGameplayManager.Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[PlayerManager] There is more then one player Instance");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        projectileManager = GetComponent<ProjectileManager>();

        //updates all the UI
        uiGameplay?.UpdateAmmoText(playerStat);
        playerStat.InitCharacterStat();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    playerStat.TakeDamage(10);
        //    uiGameplay?.UpdatePlayerStatUI();
        //}

        //if (Input.GetKeyDown(KeyCode.L)) //Debug purposes, can be removed at any time
        //{
        //    playerStat.GiveExp(40);
        //    uiGameplay?.UpdatePlayerStatUI();
        //}

        ////I dont have Smaprthone so I am shooting with mouse click
        //if (Input.GetMouseButtonDown(0))
        //    projectileManager.ShootWeapon(isWeaponRaycast);
    }

    public void ShootingAnimation()
    {
        playerAnimator.Shooting();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ItemPickup itemPickup))
        {
            //missionInventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    public void OnProjectileCollided(GameObject target)
    {
        var attack = baseAttack.CalculateDamage(playerStat);

        var attackable = target.GetComponentsInChildren<IAttackable>();

        foreach (IAttackable e in attackable)
        {
            e.OnAttack(gameObject, attack);
        }
    }

    public void Shooting()
    {
        if (readyToShoot == false)
        {
            return;
        }

        readyToShoot = false;
        if (playerStat.currentAmmo > 0)
        {
            playerStat.currentAmmo--;

            projectileManager.ShootWeapon(isWeaponRaycast);
            AudioManager.Instance.Play("Shoot");

            uiGameplay?.UpdateAmmoText(playerStat);
            Invoke(nameof(ResetShot), playerStat.attackInterval);
        }
        else
        {
            Debug.Log("[Player Manager] You dont have enought ammo!");
            ResetShot();
            return;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    public void AddHealth(int amount, out bool success)
    {
        playerStat.GiveHealth(amount, out success);
        uiGameplay?.UpdatePlayerStatUI();
    }

    public void AddShield(int amount, out bool success)
    {
        playerStat.GiveShield(amount, out success);
        uiGameplay?.UpdatePlayerStatUI();
    }

    public void AddAmmo(int amount)
    {
        AudioManager.Instance.Play("Reload");
        playerStat.AddAmmo(amount);
        uiGameplay?.UpdateAmmoText(playerStat);
        ResetShot();
    }
}