using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    [SerializeField] private GameObject consoleUI;
    [SerializeField] private QuestChain questChain;
    [SerializeField] private QuestChecker questChecker;
    private TMP_InputField consoleInput;
    private TMP_Text outText;
    private int maxLines = 21;

    void Start()
    {
        consoleUI.SetActive(false);
        consoleInput = consoleUI.GetComponentInChildren<TMP_InputField>();
        outText = consoleUI.GetComponentInChildren<TMP_Text>();
    }
    void Update()
    {
        //Open check
        if(Input.GetKeyDown(KeyCode.RightShift) && consoleUI.gameObject.activeSelf == false)
        {
            consoleUI.gameObject.SetActive(true);
            Debug.Log("Console activated.");
        }
        else if(Input.GetKeyDown(KeyCode.RightShift) && consoleUI.gameObject.activeSelf == true)
        {
            consoleUI.gameObject.SetActive(false);
            Debug.Log("Console deactivated.");
        }

        //Send check
        if(Input.GetKeyDown(KeyCode.Return) && consoleUI.gameObject.activeSelf == true)
        {
            string command = consoleInput.text;
            string output = ConsoleExecute(command);
            outText.text += output + "\n";
            consoleInput.text = "";
        }

        //Remove first line if max lines exceeded
        if(outText.text.Split('\n').Length > maxLines) RemoveFirstLine();
    }

    void RemoveFirstLine()
    {
        string[] lines = outText.text.Split('\n');
        if (lines.Length > maxLines)
        {
            outText.text = string.Join("\n", lines, 1, lines.Length - 1);
        }
    }
    
    string ConsoleExecute(string message)
    {
        string[] input = message.Split(' ');
        switch (input[0].ToLower())
        {
            case "help":
                return "Available commands: help, quest [command], clear, echo [message]";
            case "clear":
                outText.text = "";
                return "";
            case "echo":
                if (input.Length > 1)
                {
                    return string.Join(" ", input, 1, input.Length - 1);
                }
                else
                {
                    return "Usage: echo [message]";
                }
        }

        if(input[0].ToLower() == "quest")
        {
            if(input.Length > 1)
            {
                switch (input[1].ToLower())
                {
                    case "take":
                        switch (input[2].ToLower())
                        {
                            case "0":
                                questChain.Type0ChainCheck();
                                return "Type 0 quest taken.";
                            case "1":
                                // Logic for taking a type 1 quest
                                return "Type 1 quest taken.";
                            case "2":
                                // Logic for taking a type 2 quest
                                return "Type 2 quest taken.";
                            default:
                                return $"Unknown quest type: {input[2]}";
                        }
                    case "complete":
                        questChecker.CompleteQuest();
                        return "Quest completed.";
                    case "finish":
                        questChecker.DeleteQuest();
                        return "Quest finished.";
                    default:
                        return $"Unknown quest command: {input[1]}";
                }
            }
            else
            {
                return "Usage: quest [take/complete]";
            }
        }
        else return "Unknown command";
    }
}
