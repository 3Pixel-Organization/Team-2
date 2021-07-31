using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShield : MonoBehaviour
{
    public int shield;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.Instance.AddShield(shield, out bool success);
            UIGameplayManager.Instance.UpdatePlayerStatUI();

            if(success) Destroy(gameObject);
        }
    }
}
