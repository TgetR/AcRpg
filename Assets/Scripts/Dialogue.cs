using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;
    private TMP_Text text;
    private Animator animator;
    private bool isShowing = false;
    private int showedLines = 0;
    private List<string> textList = new List<string>();
    void Start()
    {
        dialogueCanvas.SetActive(false);
        text = dialogueCanvas.GetComponentInChildren<TMP_Text>();
        animator = dialogueCanvas.GetComponentInChildren<Animator>();
        text.text = "";
    }
    public void StartDialogue(List<string> dialogue)
    {
        isShowing = true;
        textList = dialogue;
        dialogueCanvas.SetActive(true);
        text.text = dialogue[0]; // Display the first line of the dialogue
        animator.SetTrigger("ShowAnim");
    }
    void Update()
    {
        if (isShowing && Input.GetKeyDown(KeyCode.Space))
        {
           if(textList.Count > showedLines) ShowNextLine();
            else
            {
                dialogueCanvas.SetActive(false);
                text.text = "";
                isShowing = false;
            }
        }
    }

    void ShowNextLine()
    {
        if(showedLines < textList.Count)
        {   
            text.text = "";
            animator.SetTrigger("ShowAnim");
            showedLines++;
            text.text = textList[showedLines];
        }
        else 
        {
            dialogueCanvas.SetActive(false);
            text.text = "";
            isShowing = false;
        }
    }
}