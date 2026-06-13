using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialBox;
    public TMP_Text tutorialText;

    private int stepIndex = 0;

    private string[] tutorialSteps =
    {
        "Welcome to the tutorial!",
        "Use the slider to choose how much money to invest.",
        "The AI players invest according to their personalities.",
        "The total pot is multiplied and shared.",
        "Low contributors may receive a punishment.",
        "Tutorial complete!"
    };

    void Start()
    {
        ShowStep();
    }

    public void NextStep()
    {
        stepIndex++;

        if (stepIndex >= tutorialSteps.Length)
        {
            tutorialBox.SetActive(false);
            return;
        }

        ShowStep();
    }

    void ShowStep()
    {
        tutorialText.text = tutorialSteps[stepIndex];
    }
}