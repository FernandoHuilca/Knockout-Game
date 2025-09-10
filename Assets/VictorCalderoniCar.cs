using UnityEngine;

public class VictorCalderoniCar : ColliderObstacleAttack
{
    [SerializeField] private AudioClip carDriftingSound;
    [SerializeField] private AudioClip carDrivingSound;
    private string ownerTag; // Cambié el nombre para evitar conflictos


    public AudioClip getCarDriftingSound()
    {
        return carDriftingSound;
    }

    public AudioClip getCarDrivindSound()
    {
        return carDrivingSound;
    }

    public void setTag(string tag)
    {
        this.ownerTag = tag;
        Debug.Log("Owner tag set to: " + ownerTag); // Debug para confirmar
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision with: " + collision.gameObject.tag + " | Owner: " + ownerTag);

        if (collision.gameObject.tag == ownerTag)
        {
            Debug.Log("SAME TAG - No damage");
            return;
        }

        Debug.Log("DIFFERENT TAG - Applying damage");
        //base.OnCollisionEnter2D(collision);
    }
}
