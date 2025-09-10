using UnityEngine;

public abstract class Gun2D : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;

    [Header("Sounds")]
    [SerializeField] private AudioClip shootSound;

    // Manejo de dirección
    private Vector3 direction;
    [SerializeField] private bool gunFacingRight;

    private SpecialAttack parentSpecialAttack;

    void Start()
    {
        parentSpecialAttack = transform.parent?.GetComponent<SpecialAttack>();  // Obtener referencia al SpecialAttack del padre para pasarlo a las balas
    }

    void Update()
    {
        determineDirection(bulletSpawnPoint.position);
        manageGunFlip(bulletSpawnPoint.position);
    }

    protected virtual void rotateGun()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunPoint.rotation = Quaternion.Euler(0, 0, angle); // Rotar el arma hacia el objetivo
    }

    protected virtual void manageGunFlip(Vector3 pointToShoot)
    {
        if (pointToShoot.x < gunPoint.position.x && !gunFacingRight)
        {
            flipGun();
        }
        else if (pointToShoot.x > gunPoint.position.x && gunFacingRight)
        {
            flipGun();
        }
    }

    protected virtual void flipGun()
    {
        gunFacingRight = !gunFacingRight;
        gunPoint.localScale = new Vector3(gunPoint.localScale.x, gunPoint.localScale.y * -1, gunPoint.localScale.z);
    }

    protected virtual void determineDirection(Vector3 pointToAim)
    {
        direction = pointToAim - gunPoint.position;
    }

    public abstract void shoot(float damage, float shieldDamage, string shooterTag);

    protected virtual void createBullet(float damage, float shieldDamage, string shooterTag)
    {
        Vector3 spawnPosition = bulletSpawnPoint.position; // CAPTURAR posici�n actual del bulletSpawnPoint

        var bulletGameObject = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);  // crear la bala

        var bulletScript = bulletGameObject.GetComponent<Bullet>();
        
        bulletScript.Initialize(damage, shieldDamage, shooterTag, parentSpecialAttack);

        bulletGameObject.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * bulletSpeed; // Aplicar velocidad

        SoundsController.Instance.RunSound(shootSound);
    }

}
