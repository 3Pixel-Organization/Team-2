using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeal : MonoBehaviour
{
    public int heal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.Instance.AddHealth(heal, out bool success);

            if(success) Destroy(gameObject);
        }
    }
}
