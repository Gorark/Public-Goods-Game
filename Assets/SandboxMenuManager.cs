using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SandboxMenuManager : MonoBehaviour
{
    public const string NoForcedPersonality = "Random";

    public static int sandboxMaxRounds = 5;
    public static int sandboxHumanStartMoney = 100;
    public static int sandboxAIStartMoney = 100;
    public static float sandboxPotMultiplier = 1.6f;
    public static float sandboxAlpha = 0.6f;
    public static string sandboxForcedPersonality = NoForcedPersonality;
    public static bool sandboxShowAIPersonalities = false;
    public static bool sandboxPunishmentEnabled = true;
    public static bool sandboxIncludeFreeRider = false;
    public static string sandboxLesson = string.Empty;

    int pendingRounds;
    int pendingHumanStartMoney;
    int pendingAIStartMoney;
    float pendingPotMultiplier;
    float pendingAlpha;
    string pendingForcedPersonality;
    bool pendingPunishmentEnabled;
    bool pendingIncludeFreeRider;
    string pendingLesson;
    bool showingModeDescription;

    public GameObject personalityPopup;
    public GameObject modeDescriptionContent;
    public TMP_Text modeDescriptionText;
    public TMP_Text smallModeDescriptionText;
    public GameObject personalityPromptText;
    public GameObject unknownChoiceText;
    public GameObject knownChoiceText;

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    void ChooseSandboxGame(int rounds, int humanStartMoney, int aiStartMoney, float potMultiplier, float alpha, string forcedPersonality, string description, bool punishmentEnabled = true, string lesson = "", bool useSmallDescription = false, bool includeFreeRider = false)
    {
        pendingRounds = rounds;
        pendingHumanStartMoney = humanStartMoney;
        pendingAIStartMoney = aiStartMoney;
        pendingPotMultiplier = potMultiplier;
        pendingAlpha = alpha;
        pendingForcedPersonality = forcedPersonality;
        pendingPunishmentEnabled = punishmentEnabled;
        pendingIncludeFreeRider = includeFreeRider;
        pendingLesson = lesson;

        if (personalityPopup != null && modeDescriptionContent != null && modeDescriptionText != null && smallModeDescriptionText != null)
        {
            showingModeDescription = true;
            modeDescriptionText.gameObject.SetActive(!useSmallDescription);
            smallModeDescriptionText.gameObject.SetActive(useSmallDescription);

            if (useSmallDescription)
            {
                smallModeDescriptionText.text = description;
            }
            else
            {
                modeDescriptionText.text = description;
            }
            modeDescriptionContent.SetActive(true);
            SetPersonalityChoiceVisible(false);
            personalityPopup.SetActive(true);
        }
        else
        {
            StartSandboxGame(false);
        }
    }

    void SelectUnknownPersonalities()
    {
        StartSandboxGame(false);
    }

    public void ContinueDescriptionOrSelectUnknown()
    {
        if (showingModeDescription)
        {
            showingModeDescription = false;
            modeDescriptionContent.SetActive(false);
            SetPersonalityChoiceVisible(true);
            return;
        }

        SelectUnknownPersonalities();
    }

    void SetPersonalityChoiceVisible(bool isVisible)
    {
        if (personalityPromptText != null)
        {
            personalityPromptText.SetActive(isVisible);
        }

        if (unknownChoiceText != null)
        {
            unknownChoiceText.SetActive(isVisible);
        }

        if (knownChoiceText != null)
        {
            knownChoiceText.SetActive(isVisible);
        }
    }

    public void ReturnDescriptionOrSelectKnown()
    {
        if (showingModeDescription)
        {
            showingModeDescription = false;
            personalityPopup.SetActive(false);
            return;
        }

        SelectKnownPersonalities();
    }

    void SelectKnownPersonalities()
    {
        StartSandboxGame(true);
    }

    void StartSandboxGame(bool showAIPersonalities)
    {
        sandboxMaxRounds = pendingRounds;
        sandboxHumanStartMoney = pendingHumanStartMoney;
        sandboxAIStartMoney = pendingAIStartMoney;
        sandboxPotMultiplier = pendingPotMultiplier;
        sandboxAlpha = pendingAlpha;
        sandboxForcedPersonality = pendingForcedPersonality;
        sandboxShowAIPersonalities = showAIPersonalities;
        sandboxPunishmentEnabled = pendingPunishmentEnabled;
        sandboxIncludeFreeRider = pendingIncludeFreeRider;
        sandboxLesson = pendingLesson;

        SceneManager.LoadScene("PlayGame");
    }

    public void OpenBulletSandbox()
    {
        ChooseSandboxGame(3, 100, 100, 1.6f, 0.6f, NoForcedPersonality, "BULLET SANDBOX\nThe game lasts 3 rounds instead of 5.");
    }

    public void OpenBaseSandbox()
    {
        ChooseSandboxGame(5, 100, 100, 1.6f, 0.6f, NoForcedPersonality, "BASE SANDBOX\nStandard rules: 5 rounds, 100 starting coins and a 1.6 pot multiplier.", true, "", true);
    }

    public void OpenLongSandbox()
    {
        ChooseSandboxGame(10, 100, 100, 1.6f, 0.6f, NoForcedPersonality, "LONG SANDBOX\nThe game lasts 10 rounds instead of 5.");
    }

    public void OpenRichAI()
    {
        ChooseSandboxGame(5, 100, 300, 1.6f, 0.6f, NoForcedPersonality, "RICH AI\nEach AI starts with 300 coins while you start with 100.", true, "Starting resources strongly affect the balance of competition, even when everyone follows the same rules.");
    }

    public void OpenNoPunishment()
    {
        ChooseSandboxGame(5, 100, 100, 1.6f, 0.6f, NoForcedPersonality, "NO PUNISHMENT\nNo contribution threshold is calculated, nobody can be punished, and one AI invests 0% for the entire game.", false, "Without punishment, the Free Rider can invest nothing and still benefit from everyone else's contributions. Cooperation becomes fragile when keeping everything carries no risk.", true, true);
    }

    public void OpenRichGame()
    {
        ChooseSandboxGame(5, 100, 100, 5f, 0.6f, NoForcedPersonality, "HIGH MULTIPLIER\nThe pot multiplier is 5 instead of 1.6.", true, "With such a high multiplier, investing becomes highly rewarding and cooperation feels much easier.", true);
    }

    public void OpenOnlyBanquier()
    {
        ChooseSandboxGame(5, 100, 100, 1.6f, 0.6f, "Banker", "ONLY BANKER\nAll three AI players use the Banker personality.");
    }

    public void OpenOnlyGambler()
    {
        ChooseSandboxGame(5, 100, 100, 1.6f, 0.6f, "Gambler", "ONLY GAMBLER\nAll three AI players use the Gambler personality.");
    }

    public void OpenOnlyFastFoodManager()
    {
        ChooseSandboxGame(5, 100, 100, 1.6f, 0.6f, "FastFoodManager", "ONLY FAST FOOD MANAGER\nAll three AI players follow the previous average contribution.", true, "", true);
    }
}
