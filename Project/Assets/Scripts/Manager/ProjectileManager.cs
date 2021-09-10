using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public Transform pfBullet;

    public LineRenderer bulletTrailRaycast;

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

        if (coolDownTimer<=0)
        {
            //if (Vector3.Distance(transform.position, fieldOFView.getClosestEnemy().position) < playerManager.playerStat.attackRange)
            if (isRaycast)
                ShootRaycastBullet();
            else if (!isRaycast)
                ShootProjectileBullet();
            coolDownTimer = playerManager.playerStat.attackInterval;
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
        if (Physics.Raycast(gunEndPosition.transform.position, shootDir, out raycastHit, playerManager.playerStat.attackRange))
        {
            if (raycastHit.transform.gameObject.GetComponent<IAttackable>() != null)
            {
                playerManager.OnProjectileCollided(raycastHit.transform.gameObject);
                CreateWeaponTracer(gunEndPosition.transform.position, fieldOFView.getClosestEnemy().position);
            }
            else
            {
                CreateWeaponTracer(gunEndPosition.transform.position, raycastHit.point);
            }

            MuzzleFlashAnimation();
            PlayerManager.Instance.ShootingAnimation();
        }
    }

    private void CreateWeaponTracer(Vector3 startPos, Vector3 targetPos)
    {
        GameObject bulletTrailEffect = Instantiate(bulletTrailRaycast.gameObject, startPos, Quaternion.identity);
        LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();

        lineR.SetPosition(0, startPos);
        lineR.SetPosition(1, targetPos);

        Destroy(bulletTrailEffect, 0.1f);
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