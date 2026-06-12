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

    public static void ResetSandboxSettings()
    {
        sandboxMaxRounds = 5;
        sandboxHumanStartMoney = 100;
        sandboxAIStartMoney = 100;
        sandboxPotMultiplier = 1.6f;
        sandboxAlpha = 0.6f;
        sandboxForcedPersonality = NoForcedPersonality;
    }

    void StartSandboxGame(int rounds, int humanStartMoney, int aiStartMoney, float potMultiplier, float alpha, string forcedPersonality)
    {
        sandboxMaxRounds = rounds;
        sandboxHumanStartMoney = humanStartMoney;
        sandboxAIStartMoney = aiStartMoney;
        sandboxPotMultiplier = potMultiplier;
        sandboxAlpha = alpha;
        sandboxForcedPersonality = forcedPersonality;

        SceneManager.LoadScene("PlayGame");
    }

    public void OpenBulletSandbox()
    {
        StartSandboxGame(3, 100, 100, 1.6f, 0.6f, NoForcedPersonality);
    }

    public void OpenBaseSandbox()
    {
        StartSandboxGame(5, 100, 100, 1.6f, 0.6f, NoForcedPersonality);
    }

    public void OpenLongSandbox()
    {
        StartSandboxGame(10, 100, 100, 1.6f, 0.6f, NoForcedPersonality);
    }

    public void OpenRichAI()
    {
        StartSandboxGame(5, 100, 200, 1.6f, 0.6f, NoForcedPersonality);
    }

    public void OpenRichHuman()
    {
        StartSandboxGame(5, 200, 100, 1.6f, 0.6f, NoForcedPersonality);
    }

    public void OpenRichGame()
    {
        StartSandboxGame(5, 100, 100, 2f, 0.8f, NoForcedPersonality);
    }

    public void OpenOnlyBanquier()
    {
        StartSandboxGame(5, 100, 100, 1.6f, 0.6f, "Banquier");
    }

    public void OpenOnlyGambler()
    {
        StartSandboxGame(5, 100, 100, 1.6f, 0.6f, "Gambler");
    }

    public void OpenOnlyFastFoodManager()
    {
        StartSandboxGame(5, 100, 100, 1.6f, 0.6f, "FastFoodManager");
    }
}
