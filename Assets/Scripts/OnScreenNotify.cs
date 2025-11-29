using TMPro;
using UnityEngine;

public class OnScreenNotify : MonoBehaviour
{
    private TMP_Text Text;
    private Animator animator;

    void Start()
    {
        Text = GetComponent<TMP_Text>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.BackQuote)) Notify("Hello, world!", 0);
    }

    public void Notify(string text, int Level)
    {
        Color UsingColor = Color.navyBlue;
        switch (Level)
        {
            case 0: UsingColor = Color.softBlue; break;
            case 1: UsingColor = Color.softGreen; break;
            case 2: UsingColor = Color.softYellow; break;
            case 3: UsingColor = Color.softRed; break;
            case 4: UsingColor = Color.crimson; break;
        }
        Text.color = UsingColor;
        Text.text = text;
        animator.SetTrigger("Notify");
    }
}
