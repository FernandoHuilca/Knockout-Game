using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class eeManager : MonoBehaviour
{
    [SerializeField] private float verticalAmplitude;
    [SerializeField] private float verticalFrequency;
    private float baseScaleY; // Escala base en Y

    [Header("Sounds")]
    [SerializeField] private AudioClip eeScreamSound;
    [SerializeField] private AudioClip eeMusicSound;
    [SerializeField] private AudioClip eeFinalSound;
    private AudioSource soundsControllerAudio;

    [Header("Final Transformation")]
    [SerializeField] private Vector3 targetScale;
    [SerializeField] private float transitionDuration;
    private SpriteRenderer childSpriteRenderer;
    private bool isTransitioning = false;

    [Header("Actives GameObjects")]
    public List<GameObject> parentObjectsToKeep = new List<GameObject>();

    private GameObject lifeStatus;
    private Transform childTransform;
    private BoxCollider2D boxCollider2D;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (GameManager.getIsEESpaceDiscovered())
        {
            boxCollider2D.isTrigger = false;
            return;
        }
        
        GameObject soundsController = GameObject.Find("SoundsController"); // Buscar una vez al inicio en lugar de cada vez que se activa el trigger
        if (soundsController != null)
        {
            soundsControllerAudio = soundsController.GetComponent<AudioSource>();
        }

        lifeStatus = GameObject.Find("LifeStatus");

        childTransform = transform.Find("eeCreepyFace");
        if(childTransform != null)
        {
            baseScaleY = childTransform.localScale.y;
            childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
        }

    }

    void Update()
    {
        if (GameManager.getIsEESpaceDiscovered())
        {
            return;
        }

        if (DialogueManager.Instance.isDialogueFinished && !isTransitioning)
        {
            isTransitioning = true;
            soundsControllerAudio.clip = eeFinalSound;
            soundsControllerAudio.Play();
            StartCoroutine(transitionCoroutine());

        }
        else if (!isTransitioning)
        {
            // hacer que el scale en Y del creepyFaceBacground oscile en el tiempo
            if (childTransform == null)
            {
                return;
            }
            float verticalOffset = Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude; // Movimiento oscilatorio en el eje vertical

            // Aplicar a la escala Y (manteniendo X y Z originales)
            childTransform.localScale = new Vector3(
                childTransform.localScale.x,
                baseScaleY + verticalOffset,
                childTransform.localScale.z
            );
        }
    }

    private IEnumerator transitionCoroutine()
    {
        if (childTransform == null || childSpriteRenderer == null) yield break;

        Vector3 startScale = childTransform.localScale;
        float elapsedTime = 0.0f;

        // Guardar el estado original del sprite
        bool originalFlipX = childSpriteRenderer.flipX;
        bool originalEnabled = childSpriteRenderer.enabled;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / transitionDuration;

            // Interpolar posición y escala (suavizado con SmoothStep)
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            childTransform.localScale = Vector3.Lerp(startScale, targetScale, smoothT);

            // Efectos aleatorios de sprite (sin interrumpir la interpolación)
            if (Random.value < 0.3f) // 30% de probabilidad cada frame
            {
                childSpriteRenderer.enabled = !childSpriteRenderer.enabled;
            }

            if (Random.value < 0.2f) // 20% de probabilidad cada frame
            {
                childSpriteRenderer.flipX = !childSpriteRenderer.flipX;
            }

            yield return null;

        }

        // Asegurar valores finales
        childSpriteRenderer.enabled = true;

        GameManager.setIsEESpaceDiscovered(true);

        GameManager.gameManagerInstance.goToMainMenu();

        // Prevenir que Update siga ejecutándose
        enabled = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Untagged" || (other.gameObject.layer != LayerMask.NameToLayer("BaseFighter")))
        {
            Destroy(other.gameObject);
            return;
        }
        Vector2 centerPosition = boxCollider2D.bounds.center;
        if (other.transform.position.x > centerPosition.x)
        {
            return;
        }

        // Acceder a la Main Camera para cambiar su posición
        GameObject mainCamera = Camera.main.gameObject;
        Vector3 cameraPosition = mainCamera.transform.position;
        cameraPosition.x = -22;
        mainCamera.transform.position = cameraPosition;

        // Activar sonidos
        soundsControllerAudio.clip = eeMusicSound;
        soundsControllerAudio.Play();
        SoundsController.Instance.RunSound(eeScreamSound);

        // bloquear pausa
        GameManager.gameManagerInstance.setPauseKey(KeyCode.None);

        // bloquear ataque especial
        other.gameObject.GetComponent<UserConfiguration>().setSpecialAttackKey(KeyCode.None);
        other.gameObject.GetComponent<UserConfiguration>().setAttack1Key(KeyCode.None);
        other.gameObject.GetComponent<UserConfiguration>().setAttack2Key(KeyCode.None);

        // bloquear GameObjects excepto los hijos de los padres indicados
        DisableAllExceptChildren(other);

        // Deshabilitar UI de vida
        if (lifeStatus != null)
        {
            lifeStatus.SetActive(false);
        }

        // Hacer que deje de ser trigger 
        boxCollider2D.isTrigger = false;
    }

    public void DisableAllExceptChildren(Collider2D other)
    {
        // Crear una lista de todos los GameObjects que NO deben ser deshabilitados
        HashSet<GameObject> objectsToKeep = new HashSet<GameObject>();

        // Agregar los GameObjects padre y todos sus hijos recursivamente
        foreach (GameObject parent in parentObjectsToKeep)
        {
            if (parent != null)
            {
                objectsToKeep.Add(parent);
                AddAllChildren(parent, objectsToKeep);
            }
        }
        objectsToKeep.Add(other.gameObject);

        LeanTween leanTweenObject = FindObjectOfType<LeanTween>();
        if (leanTweenObject != null)
        {
            GameObject gameObjectLeanTween = leanTweenObject.gameObject;
            objectsToKeep.Add(gameObjectLeanTween);
        }


        // Obtener todos los GameObjects en la escena
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Deshabilitar todos los GameObjects que no estén en la lista de "mantener"
        foreach (GameObject obj in allObjects)
        {
            if (!objectsToKeep.Contains(obj))
            {
                obj.SetActive(false);
            }
        }

        Debug.Log($"Deshabilitados {allObjects.Length - objectsToKeep.Count} GameObjects. Mantenidos activos: {objectsToKeep.Count}");
    }

    // Método recursivo para agregar todos los hijos
    private void AddAllChildren(GameObject parent, HashSet<GameObject> objectsToKeep)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            objectsToKeep.Add(child);

            // Llamada recursiva para agregar los hijos de este hijo
            AddAllChildren(child, objectsToKeep);
        }
    }
}
