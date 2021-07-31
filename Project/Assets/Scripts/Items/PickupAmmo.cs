using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : MonoBehaviour
{
    public int size;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.Instance.AddAmmo(size);

            Destroy(gameObject);
        }
    }
}
