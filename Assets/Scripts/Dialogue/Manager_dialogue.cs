using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Manager_dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;

    private Queue<string> sentences;

    public Event_After Event_After;

    public RawImage portraitImage;
    public List<ExpressionData> expressions;

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisplayNextSentence();
        }
    }*/
    private void Awake()
    {
        sentences = new Queue<string>();
        dialogueBox.SetActive(false);

    }

    public void StartDialogue(Dialogue dialogue)
    {

        dialogueBox.SetActive(true);
        //Debug.Log(dialogueBox);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string expressionName = "Blue";
        string sentence = sentences.Dequeue();

        if (sentence.StartsWith("["))
        {
            int closingBracket = sentence.IndexOf("]");
            if (closingBracket != -1)
            {
                expressionName = sentence.Substring(1, closingBracket - 1);

                sentence = sentence.Substring(closingBracket + 1).Trim();
            }
        }
        UpdatePortrait(expressionName);

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    void UpdatePortrait(string name)
    {
        ExpressionData data = expressions.Find(e => e.name.ToLower() == name.ToLower());
        if (data != null)
        {
            portraitImage.texture = data.sprite;

            portraitImage.transform.localScale = Vector3.one * 1.1f;
        }
    }
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence)
        {
            dialogueText.text += letter;

            portraitImage.transform.localScale = Vector3.Lerp(portraitImage.transform.localScale, Vector3.one, Time.deltaTime * 70f);

            yield return new WaitForSeconds(0.05f);
        }
        portraitImage.transform.localScale = Vector3.one;
    }
    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        if (Event_After != null)
        {
            Event_After.Event_Dialogue();
        }
    }
}
