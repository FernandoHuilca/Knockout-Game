using Unity.VisualScripting;
using UnityEngine;

public class AlienSpecialAttack : MonoBehaviour
{
    [Header("Special Attack Settings")]
    [SerializeField] private float specialCharge = 0f; 
    [SerializeField] private float maxCharge; 

    [Header("Script")]
    [SerializeField] private UIController UIController;

    [Header("Galactic Octopus Settings")]
    [SerializeField] private GameObject galacticOctopus;
    [SerializeField] private Vector3 spawnPosition; // Posici�n donde aparecer� el prefab
    [SerializeField] private Quaternion spawnRotation = Quaternion.identity; // Rotaci�n del prefab

    private void Start()
    {
        UIController = GetComponent<UIController>();
        updateUI();
    }

    public void increaseCharge(float amount)
    {
        specialCharge += amount;
        updateUI();
    }

    // M�todo para usar el ataque especial.
    public void useSpecialAttack()
    {
        specialCharge = 0f; // Reiniciar la barra.
        performSpecialAttack(); // Aqu� colocas la l�gica del ataque especial.
        updateUI();
    }

    private void performSpecialAttack()
    {
        galacticOctopus.GetComponent<GalacticOctopus>().setTag(gameObject.tag);
        GameObject galacticOctopusInstance = Instantiate(galacticOctopus, spawnPosition, spawnRotation);
    }

    private void updateUI()
    {
        UIController.updateSpecialBar(specialCharge, maxCharge);
    }

    public void setMaxCharge(float maxCharge)
    {
        this.maxCharge = maxCharge;
    }
}
