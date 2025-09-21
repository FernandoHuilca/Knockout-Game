using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        if(DialogueManager.Instance == null)
        {
            GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allGameObjects)
            {
                if (obj.scene.name != null && obj.name == "Dialogue")
                {
                    obj.SetActive(true);
                }
            }
        }
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "User1" || collision.tag == "User2")
        {
            TriggerDialogue();
        }
    }
}