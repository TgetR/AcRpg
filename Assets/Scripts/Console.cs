using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Console : MonoBehaviour
{
    [SerializeField] private GameObject consoleUI;
    [SerializeField] private QuestChain questChain;
    [SerializeField] private QuestChecker questChecker;
    [SerializeField] private GameObject gameController;
    private TMP_InputField consoleInput;
    private TMP_Text outText;
    private int maxLines = 21;

    void Start()
    {
        consoleUI.SetActive(false);
        consoleInput = consoleUI.GetComponentInChildren<TMP_InputField>();
        outText = consoleUI.GetComponentInChildren<TMP_Text>();

        gameController = GameObject.FindGameObjectWithTag("GameController");
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

            //Display help
            case "help":
                return "Available commands: help, quest [command], clear, echo [message], give [itemID/key] [amount], GetItems";
            // Clear console
            case "clear":
                outText.text = "";
                return "";
            // Display any message
            case "echo":
                if (input.Length > 1)
                {
                    return string.Join(" ", input, 1, input.Length - 1);
                }
                else
                {
                    return "Usage: echo [message]";
                }
            //Quest commands category
            case "quest":
                if(input.Length > 1)
                {
                    switch (input[1].ToLower())
                    {
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

            case "getid":
                return GetItemsNamesAndIDs();

            //Give commands category
            case "give":
                if (input.Length > 2)
                {
                    switch (input[1].ToLower())
                    {
                        //Give keys
                        case "key":

                            int amount;
                            if (int.TryParse(input[2], out amount))
                            {
                                GameObject.FindGameObjectWithTag("GameController").GetComponent<KeySystemController>().KeysBalance += amount;
                                return $"Added {amount} keys.";
                            }

                            else 
                                return "Usage: give key [amount]";

                        //Give items
                        case "item":

                            int id;
                            if (int.TryParse(input[2], out id))
                            {
                                return FindItemByID(id);
                            }

                            else 
                                return "Usage: give item [itemID]";

                        default:
                            return $"Unknown item type: {input[1]}";
                    }
                }
                else
                {
                    return "Usage: give [itemID/key] [amount]";
                }
        }
        return $"Unknown command: {input[0]}";
    }

    string GetItemsNamesAndIDs()
    {
        Inventory inv = gameController.GetComponent<Inventory>();
        inv.GetItemsNames_ForConsoleUse();
        List<string> itemNames = inv.GetItemsNames_ForConsoleUse();
        string result = "Inventory items:\n";
        for (int i = 0; i < itemNames.Count; i++)
        {
            result += $"{i}: {itemNames[i]}\n";
        }
        return result;
    }

    string FindItemByID(int id)
    {
        Inventory inv = gameController.GetComponent<Inventory>();
        inv.GetItemsNames_ForConsoleUse();
        List<string> itemNames = inv.GetItemsNames_ForConsoleUse();
        if( itemNames.Count >= id)
        {
            return "Item by requested ID found: " + itemNames[id];
        }
        return "Item not found.";
    }
}
