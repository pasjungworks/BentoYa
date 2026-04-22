using UnityEngine;

public class Trigger_Dialogue : MonoBehaviour
{
    public Dialogue dialogue;
    private void Start()
    {
        TriggerDialogue();
    }
    public void TriggerDialogue()
    {
        Object.FindFirstObjectByType<Manager_dialogue>().StartDialogue(dialogue);
    }

   
}
