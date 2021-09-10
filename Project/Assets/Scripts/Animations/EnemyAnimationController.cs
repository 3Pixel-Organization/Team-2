using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    public EnemyController enemyController;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
    }

    public void EnemyAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void EnemyDiesAnimation()
    {
        animator.SetTrigger("Dying");
    }

    public void EnemyMovement(float velocity)
    {
        animator.SetFloat("Speed", Mathf.Abs(velocity));
        //animator.SetFloat("Speed", enemyController.agent.velocity.magnitude / enemyController.agent.speed);
    }
}
