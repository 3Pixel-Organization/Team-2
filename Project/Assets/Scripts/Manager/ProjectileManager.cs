using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public Transform pfBullet;

    public Transform gunEndPosition;

    public Transform shootPosition;
    public Vector3 shootDir;
    public PlayerManager playerManager;

    public FieldOFView fieldOFView;
    public GameObject vfxMuzzle;

   float coolDownTimer;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        fieldOFView = GetComponent<FieldOFView>();
    }

    private void Update()
    {
        if (coolDownTimer >= 0)
            coolDownTimer -= Time.deltaTime;
        
    }
    public void ShootWeapon(bool isRaycast)
    {
        if (fieldOFView.getClosestEnemy() && coolDownTimer<=0)
        {
           if (Vector3.Distance(transform.position, fieldOFView.getClosestEnemy().position) < playerManager.playerStat.attackRange)
            {
                if (isRaycast)
                    ShootRaycastBullet();
                else if (!isRaycast)
                    ShootProjectileBullet();
                coolDownTimer= playerManager.playerStat.attackInterval;
            }
        }
    }

    public void ShootProjectileBullet()
    {
        Transform bullet = Instantiate(pfBullet, gunEndPosition.position, Quaternion.identity);
        MuzzleFlashAnimation();
        shootDir = gunEndPosition.forward;
        bullet.localRotation = Quaternion.LookRotation(gunEndPosition.forward);
        bullet.GetComponent<Projectile>().Setup(shootDir);
        PlayerManager.Instance.ShootingAnimation();
    }

    public void ShootRaycastBullet()
    {
        shootDir = gunEndPosition.forward;
        RaycastHit raycastHit;
        if (Physics.Raycast(gunEndPosition.transform.position, shootDir, out raycastHit, 100f))
        {
            if (raycastHit.transform.gameObject.GetComponent<IAttackable>() != null)
            {
                playerManager.OnProjectileCollided(raycastHit.transform.gameObject);
                CreateWeaponTracer(gunEndPosition.transform.position, fieldOFView.getClosestEnemy().position);
                MuzzleFlashAnimation();
            }
        }
    }

    private void CreateWeaponTracer(Vector3 startPos, Vector3 targetPos)
    {
        // Vector3 dir=( targetPos-startPos).normalized;
    }

    public void MuzzleFlashAnimation()
    {
        if (vfxMuzzle != null)
        {
            var muzzle = Instantiate(vfxMuzzle, gunEndPosition.position, Quaternion.identity);
            muzzle.transform.forward = gameObject.transform.forward;
            var psMuzzle = muzzle.GetComponent<ParticleSystem>();
            if (psMuzzle != null)
            {
                psMuzzle.Play();
                Destroy(muzzle, psMuzzle.main.duration);
            }
            else
            {
                var psChild = muzzle.GetComponent<ParticleSystem>();
                Destroy(muzzle, psChild.main.duration);
            }
        }
    }
}