using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;
    private TMP_Text text;
    private Animator animator;
    private bool isShowing = false;
    private int showedLines = 0;
    private bool _cooldown = false;
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
            if(textList.Count > showedLines && !_cooldown) ShowNextLine();
            else
            {
                dialogueCanvas.SetActive(false);
                text.text = "";
                isShowing = false;
                showedLines = 0;
                textList.Clear();
            }
        }
    }

   void ShowNextLine()
    {
        StartCoroutine(Cooldown());
        try
        {
            text.text = "";
            animator.SetTrigger("ShowAnim");
            showedLines++;
            text.text = textList[showedLines];
            Debug.Log( showedLines + "/" + textList.Count);   
        }
        catch (Exception ex)
        {
            if (ex is ArgumentOutOfRangeException)
            {
                dialogueCanvas.SetActive(false);
                text.text = "";
                isShowing = false;
                showedLines = 0;
                textList.Clear();
            }
        }
            
    }
    IEnumerator Cooldown()
    {
        _cooldown = true;
        yield return new WaitForSeconds(5f);
        _cooldown = false;
    }
}