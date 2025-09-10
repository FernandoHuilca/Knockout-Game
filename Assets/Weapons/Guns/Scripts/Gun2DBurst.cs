using UnityEngine;

public class Gun2DBurst : Gun2D
{
    [SerializeField] private float timeBetweenBullets; // Tiempo entre cada bala en segundos
    [SerializeField] private int maxNumberOfBulletsPerShot;

    public override void shoot(float damage, float shieldDamage, string shooterTag)
    {
        shootBurst(damage, shieldDamage, shooterTag);
    }

    private void shootBurst(float damage, float shieldDamage, string shooterTag)
    {
        StartCoroutine(shootBurstCoroutine(damage, shieldDamage, shooterTag));
    }

    public System.Collections.IEnumerator shootBurstCoroutine(float damage, float shieldDamage, string shooterTag)
    {
        for (int horizontalDistanceMultiplier = 0; horizontalDistanceMultiplier < maxNumberOfBulletsPerShot; horizontalDistanceMultiplier++)
        {
            createBullet(damage, shieldDamage, shooterTag);

            // Esperar antes de generar la siguiente bala
            if (horizontalDistanceMultiplier < maxNumberOfBulletsPerShot  - 1) // Solo esperar si no es la última bala
            {
                yield return new WaitForSeconds(timeBetweenBullets);
            }
        }
    }

    public int getMaxNumberOfBulletsPerShot()
    {
        return maxNumberOfBulletsPerShot;
    }

    public float getTimeBetweenBullets()
    {
        return timeBetweenBullets;
    }


    public void setMaxNumberOfBulletsPerShot(int maxNumberOfBulletsPerShot)
    {
        this.maxNumberOfBulletsPerShot = maxNumberOfBulletsPerShot;
    }

    public void setTimeBetweenBullets(float timeBetweenBullets)
    {
        this.timeBetweenBullets = timeBetweenBullets;
    }
}
