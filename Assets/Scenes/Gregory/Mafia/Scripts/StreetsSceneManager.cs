using UnityEngine;
using System.Collections;

public class StreetsSceneManager : MonoBehaviour
{
    private float startedTime;
    private bool movementStarted;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject mafiaCar;
    [SerializeField] private GameObject policeCar;
    [SerializeField] private GameObject henchman;
    [SerializeField] private GameObject policeOfficer;

    [Header("Final Position Settings")]
    [SerializeField] private Vector3 mafiaCarFinalPos = new Vector3(-8.64f, -3.42f, -1.0f);
    [SerializeField] private Vector3 policeCarFinalPos = new Vector3(8.75f, -3.13f, -1.0f);
    [SerializeField] private Vector3 henchmanFinalPos = new Vector3(-7.48f, -3.03f, 0.0f);
    [SerializeField] private Vector3 policeOfficerFinalPos = new Vector3(7.575f, -2.93f, 0.0f);

    [Header("Movement Settings")]
    [SerializeField] private float durationToFinalPosCar;
    [SerializeField] private float durationToFinalPosPeople;
    [SerializeField] private float durationDuringFinalPos;
    [SerializeField] private float durationBetweenSpawns;

    private Vector3 mafiaCarInitialPos;
    private Vector3 policeCarInitialPos;
    private Vector3 henchmanInitialPos;
    private Vector3 policeOfficerInitialPos;

    [Header("Random Shooting Settings")]
    [SerializeField] private int minBullets;
    [SerializeField] private int maxBullets;
    [SerializeField] private float minTimeBetweenBullets ;  
    [SerializeField] private float maxTimeBetweenBullets;
    [SerializeField] private float damage;
    [SerializeField] private float damageToShield;

    [SerializeField] private AudioClip policeSirenSound;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startedTime = Time.time;
       
        mafiaCarInitialPos = mafiaCar.transform.position;
        policeCarInitialPos = policeCar.transform.position;
        henchmanInitialPos = henchman.transform.position;
        policeOfficerInitialPos = policeOfficer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time < (startedTime+durationBetweenSpawns))
        {
            return;
        }

        if (movementStarted)
        {
            return;
        }
        movementStarted = true;
        SoundsController.Instance.RunSound(policeSirenSound);
        StartCoroutine(MovementCycle());

    }

    private IEnumerator MovementCycle()
    {
        Vector3[] carPositions = { mafiaCarFinalPos, policeCarFinalPos };
        GameObject[] gameObjectsCar = { mafiaCar, policeCar };
        yield return StartCoroutine(moveObjects(carPositions, gameObjectsCar));
        //yield return new WaitForSeconds(durationDuringFinalPos);

        Vector3[] peoplePositions = { henchmanFinalPos, policeOfficerFinalPos };
        GameObject[] gameObjectsPeople = { henchman, policeOfficer };
        yield return StartCoroutine(moveObjects(peoplePositions, gameObjectsPeople));

        foreach (GameObject gameObjectPeople in gameObjectsPeople)
        {
            int randomBulletCount = Random.Range(minBullets, maxBullets + 1);
            float randomDelay = Random.Range(minTimeBetweenBullets, maxTimeBetweenBullets);

            GameObject gun2D = gameObjectPeople.transform.Find("Gun").gameObject;
            
            gun2D.GetComponent<Gun2DBurst>().setTimeBetweenBullets(randomDelay);
            gun2D.GetComponent<Gun2DBurst>().setMaxNumberOfBulletsPerShot(randomBulletCount);
            
            gun2D.GetComponent<Gun2DBurst>().shoot(damage, damageToShield, "Untagged");
        }
        yield return new WaitForSeconds(durationDuringFinalPos);

        peoplePositions = new Vector3[] { henchmanInitialPos, policeOfficerInitialPos };
        yield return StartCoroutine(moveObjects(peoplePositions, gameObjectsPeople));
        //yield return new WaitForSeconds(durationDuringFinalPos);

        carPositions = new Vector3[] { mafiaCarInitialPos, policeCarInitialPos };
        yield return StartCoroutine(moveObjects(carPositions, gameObjectsCar));
        yield return new WaitForSeconds(durationDuringFinalPos);

        startedTime = Time.time;
        movementStarted = false;
    }

    private IEnumerator moveObjects(Vector3[] positions, GameObject[] gameObjects)
    {
        Coroutine[] moveCoroutines = new Coroutine[positions.Length];

        for(int i = 0; i< moveCoroutines.Length; i++)
        {
            moveCoroutines[i] = StartCoroutine(moveObject(gameObjects[i], positions[i]));
        }

        foreach(var coroutine in moveCoroutines)
        {
            if(coroutine != null)
            {
                yield return coroutine;
            }
        }
    }

    private IEnumerator moveObject(GameObject gameObject, Vector3 targetPosition)
    {
        Vector3 startPosition = gameObject.transform.position;
        float elapsedTime = 0.0f;

        while(elapsedTime < durationToFinalPosCar) {

            elapsedTime += Time.deltaTime;

            float t = elapsedTime / durationToFinalPosCar;

            gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;

        }

        gameObject.transform.position = targetPosition;

    }
}
