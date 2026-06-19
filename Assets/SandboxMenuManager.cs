using UnityEngine;
using UnityEngine.SceneManagement;

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

    int pendingRounds;
    int pendingHumanStartMoney;
    int pendingAIStartMoney;
    float pendingPotMultiplier;
    float pendingAlpha;
    string pendingForcedPersonality;

    public GameObject personalityPopup;

    public static void ResetSandboxSettings()
    {
        sandboxMaxRounds = 5;
        sandboxHumanStartMoney = 100;
        sandboxAIStartMoney = 100;
        sandboxPotMultiplier = 1.6f;
        sandboxAlpha = 0.6f;
        sandboxForcedPersonality = NoForcedPersonality;
        sandboxShowAIPersonalities = false;
    }

    public void OpenGame()
    {
        ResetSandboxSettings();
        SceneManager.LoadScene("PlayGame");
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    void ChooseSandboxGame(int rounds, int humanStartMoney, int aiStartMoney, float potMultiplier, float alpha, string forcedPersonality)
    {
        pendingRounds = rounds;
        pendingHumanStartMoney = humanStartMoney;
        pendingAIStartMoney = aiStartMoney;
        pendingPotMultiplier = potMultiplier;
        pendingAlpha = alpha;
        pendingForcedPersonality = forcedPersonality;

        if (personalityPopup != null)
        {
            personalityPopup.SetActive(true);
        }
        else
        {
            StartSandboxGame(false);
        }
    }

    public void SelectUnknownPersonalities()
    {
        StartSandboxGame(false);
    }

    public void SelectKnownPersonalities()
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

        SceneManager.LoadScene("PlayGame");
    }

    public void OpenBulletSandbox()
    {
        ChooseSandboxGame(3, 100, 100, 1.6f, 0.6f, NoForcedPersonality);
    }

    public void OpenBaseSandbox()
    {
        ChooseSandboxGame(5, 100, 100, 1.6f, 0.6f, NoForcedPersonality);
    }

    public void OpenLongSandbox()
    {
        ChooseSandboxGame(10, 100, 100, 1.6f, 0.6f, NoForcedPersonality);
    }

    public void OpenRichAI()
    {
        ChooseSandboxGame(5, 100, 200, 1.6f, 0.6f, NoForcedPersonality);
    }

    public void OpenRichHuman()
    {
        ChooseSandboxGame(5, 200, 100, 1.6f, 0.6f, NoForcedPersonality);
    }

    public void OpenRichGame()
    {
        ChooseSandboxGame(5, 100, 100, 2f, 0.8f, NoForcedPersonality);
    }

    public void OpenOnlyBanquier()
    {
        ChooseSandboxGame(5, 100, 100, 1.6f, 0.6f, "Banquier");
    }

    public void OpenOnlyGambler()
    {
        ChooseSandboxGame(5, 100, 100, 1.6f, 0.6f, "Gambler");
    }

    public void OpenOnlyFastFoodManager()
    {
        ChooseSandboxGame(5, 100, 100, 1.6f, 0.6f, "FastFoodManager");
    }
}
