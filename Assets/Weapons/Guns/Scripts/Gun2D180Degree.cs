using UnityEngine;

public class Gun2D180Degree : Gun2DBurst
{
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        determineDirection(mousePos);

        rotateGun();

        manageGunFlip(mousePos); 
    }
}
