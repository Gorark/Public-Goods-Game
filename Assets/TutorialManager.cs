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
    "And the first round starts... but first, here is the idea of the Public Goods Game: each player secretly chooses how many tokens to keep and how many tokens to put into a common pot.",
    "All players start with 100 tokens. After each round, your new amount becomes your budget for the next round, so decisions can snowball over time.",
    "The idea behind the tokens is to simulate real life: we all have a limited amount of money, and we can choose to keep it or invest part of it in a project that benefits everyone. No pressure... except the whole group is watching.",
    "                                           ROUND 1!",
    "The characters in the game, just like in real life, make decisions based on economic profiles: their profile changes how much risk they take and how much they usually contribute.",
    "This creates the main dilemma of the Public Goods Game: personal interest can conflict with group cooperation.",
    " ",
    "Character 1: Banker. This character is a risk-taker, meaning they believe that taking risks is an important part of an economic event. They always invest a number between 50 and 70.",
    " ",
    "Character 2: Fast Food Manager. This profile is cautious. It follows the previous average contribution when possible, while staying between 20 and 70 tokens. Very reasonable. Almost suspiciously reasonable.",
    " ",
    "Character 3: Gambler. This character loves taking risks, wants to surprise the other players each round, and can choose a totally random amount between 0 and 100 tokens. Nobody knows what will happen, including them.",
    "Now it is your turn!", 
    " ",
    "About the results:",
    "Once everyone has chosen an amount, all contributions are added into the common pot.",
    "The pot is then multiplied by M = 1.6. This creates a public benefit: together, the group creates more value than the raw amount that was invested.",
    "However, the multiplied pot is divided equally between all four players, even if someone contributed very little. Yes, this is where the suspicious smiles begin.",
    "That is why the game creates a dilemma. Contributing helps the group, but keeping tokens can be tempting for an individual player.",
    "If the multiplier were too low, contributing would feel useless, because the common pot would not create enough value.",
    "If the multiplier were too high, contributing everything would become too obvious, and the dilemma would disappear.",
    "With four players and M = 1.6, the shared return is useful, but not automatic. You still have to think about how much to invest.",
    "A player who benefits from the pot while contributing little is called a free-rider. Nice for them, annoying for literally everyone else.",
    "For example, imagine a public park: everyone can enjoy it, even people who did not help pay for its maintenance.",
    "If free-riding is always the best strategy, cooperation collapses. The game therefore includes a punishment rule, because society occasionally chooses drama.",
    "Oh? What is the game's method of punishment? Well, now that you asked...",
    "After a round, the game compares each player's contribution rate with the group average contribution rate.",
    "If a player contributed less than 80% of the group average, that player loses half of the gains from that round.",
    "This makes the question harder: should you keep more tokens now, or contribute enough to avoid being punished later? Losing half of your gains is pretty tough.",
    "The goal is not to force generosity. The goal is to show how incentives, equal redistribution, punishment, and shared benefits can change players' behaviours and choices.",
    "Try to finish the game with more tokens than the other players while understanding why cooperation can be fragile. Good luck, future economist."
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
