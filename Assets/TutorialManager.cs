using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject highlight;
    public GameObject tutorialBox;
    public GameObject gameRoundUI;

    public TMP_Text tutorialText;

    public TMP_Text ai1AmountText;
    public TMP_Text ai2AmountText;
    public TMP_Text ai3AmountText;
    public TMP_Text playerAmountText;

    public Slider investSlider;
    public TMP_Text resultText;

    public GameObject investButton;
    public GameObject investSliderObject;
    public GameObject investPreviewText;

    public GameObject humanText;
    public GameObject ai1Text;
    public GameObject ai2Text;
    public GameObject ai3Text;
    public GameObject roundText;

    

    private int stepIndex = 0;

    private string[] tutorialSteps =
    {
        "We are in the city of Carouge. Here, four entrepreneurs have decided to collaborate in order to improve their neighbourhood: Battelle.",
    "And the first round starts...",
    "All players start the game with a stack of 100 tokens, but after each round, the stack does not reset to the initial amount.",
    "The idea behind the tokens is to simulate real life: we all have a certain amount of money that we can choose to invest in a project.",
    "                                           ROUND 1!",
    "The characters in the game, just like in real life, make decisions based on their profiles and personalities.",
    "There are specific economic profiles involved here.",
    " ",
    "Character 1: Banker. This character is a risk-taker, meaning they believe that taking risks is an important part of an economic event. They always invest a number between 50 and 70.",
    " ",
    "Character 2: Fast Food Manager. This character is risk-averse, meaning they are cautious and almost never take big risks. They will always invest the average amount invested in the previous round, if that average is between 20 and 70.",
    " ",
    "Character 3: Gambler. This character loves taking risks and also wants to surprise the other players each round by choosing a totally random number between 0 and 100.",
    "Now it is your turn!", 
    " ",
    "About the results:",
    "Once everyone has invested the amount they want, the investment pot is multiplied by a value called M, which is a multiplier.",
    "The goal of the multiplier is to make the investment process more interesting for the player. Depending on its value, the multiplier can make everyone gain more tokens or lose tokens collectively.",
    "For example, a multiplier lower than 1 would make every player lose money, no matter how much they contributed to the investment pot.",
    "Is that why a multiplier equal to 1 would not change the gains once the investment pot is divided? YES. This means that the multiplier has to be higher than 1.00... but which value should it have?",
    "Let's look at a multiplier of 1.2. Dividing it by 4, the number of players, gives 0.3. This means a loss of 0.7, because 1.00 - 0.3 = 0.7. That is not very balanced.",
    "The multiplier should be higher than 1.2 then. What about 1.6? Dividing it by 4 gives 0.4, and the loss is 0.6 because 1.00 - 0.4 = 0.6. That is actually an almost equal gain-to-loss ratio!",
    "Actually, that is the exact multiplier we chose. What a coincidence...",
    "Another advantage of using a multiplier is that it discourages players from not contributing to the investment pot while still benefiting from the gains. This is called being a free-rider.",
    "An example of free-riding is a public park: we all enjoy a well-maintained public park, even if we did not personally contribute to the funds used to improve it.",
    "You could think that this is a method of punishment, but it is not. It only prevents one extreme case from happening: someone contributing nothing to the investment pot.",
    "Oh? What is the game's method of punishment? Well, now that you asked...",
    "The game is designed so that all characters place their funds in the pot before the player. This means the player cannot analyse the amounts played in the current round, only the amounts played in the previous round.",
    "Because of this principle, a player's actions in the current round will determine the level of punishment or help they receive in the future round.",
    "The formula we created only punishes players who invest less than 80% of the average amount invested in one round. Their gains will be cut in half. Pretty tough!",
    "The purpose of the game and its rules is to encourage equality between the characters, based on two principles: 1) dividing the investment pot into four equal amounts, and 2) punishing players who contribute much less in order to keep more tokens for themselves.",
    "The game aims to make you play correctly and be fair with the other players.",
    "It is as if the rules put in place for a game directly impact the players' behaviours and choices... Interesting."
    };

    public void SkipTutorial()
    {
        SceneManager.LoadScene("MainMenuSandbox");
    }

    void Start()
    {
        ShowStep();
    }

    public void NextStep()
    {
        stepIndex++;

        if (stepIndex >= tutorialSteps.Length)
        {
        highlight.SetActive(false);
        tutorialBox.SetActive(false);
        return;
        }

        ShowStep();
    }

    void ShowStep()
{
    tutorialText.text = tutorialSteps[stepIndex];
    highlight.SetActive(false);
    gameRoundUI.SetActive(false);

    ai1AmountText.gameObject.SetActive(false);
    ai2AmountText.gameObject.SetActive(false);
    ai3AmountText.gameObject.SetActive(false);
    playerAmountText.gameObject.SetActive(false);

    if (stepIndex == 2)
    {
        highlight.SetActive(true);
        highlight.transform.position = new Vector3(1150, 290, -70);
    }
    else if (stepIndex == 7)
    {
        ai1AmountText.gameObject.SetActive(true);
        ai1AmountText.text = "Given: 70\n Left: 30";
    }
    else if (stepIndex == 8)
    {
        ai1AmountText.gameObject.SetActive(true);
        ai1AmountText.text = "Given: 70\n Left: 30";
        highlight.SetActive(true);
        highlight.transform.position = new Vector3(580, 500, 0);
    }
    else if (stepIndex == 9)
    { 
        ai1AmountText.gameObject.SetActive(true);
        ai1AmountText.text = "Given: 70\n Left: 30";
        ai2AmountText.gameObject.SetActive(true);
        ai2AmountText.text = "Given: 10\n Left: 90";
    }
    else if (stepIndex == 10)
    {
        ai1AmountText.gameObject.SetActive(true);
        ai1AmountText.text = "Given: 70\n Left: 30";
        ai2AmountText.gameObject.SetActive(true);
        ai2AmountText.text = "Given: 10\n Left: 90";
        highlight.SetActive(true);
        highlight.transform.position = new Vector3(1150, 520, 0);
    }
      else if (stepIndex == 11)
    { 
        ai1AmountText.gameObject.SetActive(true);
        ai1AmountText.text = "Given: 70\n Left: 30";
        ai2AmountText.gameObject.SetActive(true);
        ai2AmountText.text = "Given: 10\n Left: 90";
        ai3AmountText.gameObject.SetActive(true);
        ai3AmountText.text = "Given: 64\n Left: 36";
    }
    else if (stepIndex == 12)
    {
        ai1AmountText.gameObject.SetActive(true);
        ai1AmountText.text = "Given: 70\n Left: 30";
        ai2AmountText.gameObject.SetActive(true);
        ai2AmountText.text = "Given: 10\n Left: 90";
        ai3AmountText.gameObject.SetActive(true);
        ai3AmountText.text = "Given: 64\n Left: 36";
        highlight.SetActive(true);
        highlight.transform.position = new Vector3(1400, 450, 0);
        highlight.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    else if (stepIndex == 14)
    {
        gameRoundUI.SetActive(true);
        investButton.SetActive(true);
        investSliderObject.SetActive(true);
        investPreviewText.SetActive(true);

        humanText.SetActive(true);
        ai1Text.SetActive(true);
        ai2Text.SetActive(true);
        ai3Text.SetActive(true);
        roundText.SetActive(true);
    }
    else if (stepIndex == 15)
    {
        humanText.SetActive(true);
        ai1Text.SetActive(true);
        ai2Text.SetActive(true);
        ai3Text.SetActive(true);
        roundText.SetActive(true);
    }
    else if (stepIndex == 16)
    {

    }
    else
    {
        highlight.SetActive(false);
        gameRoundUI.SetActive(false);
        investButton.SetActive(false);
        investSliderObject.SetActive(false);
        investPreviewText.SetActive(false);

        humanText.SetActive(false);
        ai1Text.SetActive(false);
        ai2Text.SetActive(false);
        ai3Text.SetActive(false);
        roundText.SetActive(false);
    }
}

public void PlayTutorialRound()
{
    int ai1Invest = 70;
    int ai2Invest = 10;
    int ai3Invest = 64;

    int playerInvest = Mathf.RoundToInt(investSlider.value);

    int totalInvested = playerInvest + ai1Invest + ai2Invest + ai3Invest;
    float multipliedPot = totalInvested * 1.6f;
    float share = multipliedPot / 4f;

    int playerFinal = Mathf.RoundToInt((100 - playerInvest) + share);
    int ai1Final = Mathf.RoundToInt((100 - ai1Invest) + share);
    int ai2Final = Mathf.RoundToInt((100 - ai2Invest) + share);
    int ai3Final = Mathf.RoundToInt((100 - ai3Invest) + share);

    // Hide investment UI
    investButton.SetActive(false);
    investSliderObject.SetActive(false);
    investPreviewText.SetActive(false);

    // Keep round UI visible
    gameRoundUI.SetActive(true);

    // Show labels above heads
    humanText.SetActive(true);
    ai1Text.SetActive(true);
    ai2Text.SetActive(true);
    ai3Text.SetActive(true);
    roundText.SetActive(true);

    // Show result texts above heads
    playerAmountText.gameObject.SetActive(true);
    ai1AmountText.gameObject.SetActive(true);
    ai2AmountText.gameObject.SetActive(true);
    ai3AmountText.gameObject.SetActive(true);

    playerAmountText.text =
        "You received: " + share.ToString("0.0") +
        "\nAmount Now: " + playerFinal;

    ai1AmountText.text =
        "AI1 received: " + share.ToString("0.0") +
        "\nAmount Now: " + ai1Final;

    ai2AmountText.text =
        "AI2 received: " + share.ToString("0.0") +
        "\nAmount Now: " + ai2Final;

    ai3AmountText.text =
        "AI3 received: " + share.ToString("0.0") +
        "\nAmount Now: " + ai3Final;

    // Optional big result text
    resultText.gameObject.SetActive(true);
    resultText.text = "Round complete!";
}
}