using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class BasicTakingDamage : MonoBehaviour, IAttackable
{
    private CharacterStats stats;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    public void OnAttack(GameObject attacker, int damage)
    {
        if(TryGetComponent(out CharacterStats characterStats))
        {
            characterStats.TakeDamage(damage);
        }

        if (stats.currentHealth <= 0)
        {
            var destructibles = GetComponents<IDestructable>();
            foreach (IDestructable d in destructibles)
            {
                d.OnDestruct(attacker);
            }
        }
    }
}
