using System.Collections;
using TMPro;
using UnityEngine;

public class IntroTextManager : MonoBehaviour
{
    public TextMeshProUGUI introText; // Assign this in the Inspector
    public float typingSpeed = 0.05f; // Delay between each character

    private string tutorialText = "Welcome to Finding Meno! \n Objective: Find Meno. I heard he is in the cave to the right \n Use WASD to move, Space to charge and shoot, and Q/E to spin. \n GOOD LUCK!!";
    private bool isTyping = false;

    void Start()
    {
        StartCoroutine(TypeText(tutorialText));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        introText.text = ""; // Clear the text initially

        foreach (char c in text)
        {
            introText.text += c; // Add one character at a time
            yield return new WaitForSeconds(typingSpeed); // Wait before adding the next character
        }

        isTyping = false;

        // Wait for 3 seconds after typing is complete
        yield return new WaitForSeconds(3f);

        // Clear the text
        introText.text = "";
    }
}