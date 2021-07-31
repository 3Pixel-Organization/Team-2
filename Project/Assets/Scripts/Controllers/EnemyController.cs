using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IPooledObject
{
    public GameObject player;

    public Rigidbody rb;
    public NavMeshAgent agent;
    public EnemyAnimationController animationController;

    public EnemyStat enemyStats;
    public StatusBar healthBar;

    public AttackDefenition attack;

    private float _timeOfLastAttack;

    private bool _isAlive = true;
    private float _distanceFromPlayer;
    public  float gizmoRadius = 5f;

    public Image targetSprite;
    


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = PlayerManager.Instance.gameObject;
        animationController = GetComponent<EnemyAnimationController>();

        enemyStats = GetComponent<EnemyStat>();
        enemyStats.InitCharacterStat();
        UpdateHealthSlider();

        _timeOfLastAttack = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled)
        {
            _distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (_distanceFromPlayer <= enemyStats.aggroDistance)
            {
                agent.SetDestination(player.transform.position);
                animationController.EnemyMovement();
            }
            else if (_distanceFromPlayer > enemyStats.aggroDistance * 1.5)
            {
                agent.SetDestination(transform.position);
                animationController.EnemyMovement();
            }

            float timeSinceLastAttack = Time.time - _timeOfLastAttack;
            bool attackOnCoolDown = timeSinceLastAttack < enemyStats.attackInterval;
            bool attackInRange = _distanceFromPlayer < enemyStats.attackRange;
            agent.isStopped = attackOnCoolDown;
            if (!attackOnCoolDown && attackInRange)
            {
                agent.velocity = Vector3.zero;

                animationController.EnemyAttackAnimation();
                transform.LookAt(player.transform);
                _timeOfLastAttack = Time.time;
            }
        }
    }

    #region Attack and Defence

    //executed by the animation event "Hit"
    public void Hit()
    {
        attack.ExecuteAttack(gameObject, player);
    }

    //called on DestroyObject script
    public void Dying()
    {
        if (_isAlive)
        {
            _isAlive = false;
            SpawnController.Instance.actualEnemiesAlive--;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            animationController.EnemyDiesAnimation();
            agent.enabled = false;
            ReportDeathToPlayer();
        }
    }

    void ReportDeathToPlayer()
    {
        //player.GetComponent<PlayerQuest>().IncrementKillQuestGoal(enemyStats.stats.NpcName);
    }


    #endregion

    #region Decreasers
    public void TakeDamage(int amount)
    {
        enemyStats.TakeDamage(amount);
        UpdateHealthSlider();
    }
    #endregion


    public void UpdateHealthSlider()
    {
        if (healthBar != null)
        {
            healthBar.UpdateSlider((float)enemyStats.currentHealth / (float)enemyStats.GetMaxHealth());
        }
        else
        {
            healthBar = GetComponentInChildren<StatusBar>();
            healthBar.UpdateSlider((float)enemyStats.currentHealth / (float)enemyStats.GetMaxHealth());
        }
        if(enemyStats.currentHealth == enemyStats.GetMaxHealth())
        {
            if (healthBar.gameObject.activeSelf == true)
            {
                healthBar.gameObject.SetActive(false);
            }
        }
        else if(enemyStats.currentHealth == 0)
        {
            if (healthBar.gameObject.activeSelf == true)
            {
                healthBar.gameObject.SetActive(false);
            }
        }
        else
        {
            if(healthBar.gameObject.activeSelf == false)
            {
                healthBar.gameObject.SetActive(true);
            }
        }
        //add an if for armor / shield
        //healthBar.TakingDamage(amount, playerStats.stats);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
    
    //the method called when the enemy is spawned via the object pooling
    public void OnObjectSpawn() 
    {
        gameObject.SetActive(true);
        _isAlive = true;
        agent.enabled = true;
    }


}
