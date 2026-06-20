using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    string ai1Personality;
    string ai2Personality;
    string ai3Personality;

    float lastAverage = 0.5f;

    public int humanMoney = 100;

    int ai1Money = 100;
    int ai2Money = 100;
    int ai3Money = 100;

    float alpha = 0.6f;
    float potMultiplier = 1.6f;
    bool punishmentEnabled = true;

    float lastAI1Ratio = -1f;
    float lastAI2Ratio = -1f;
    float lastAI3Ratio = -1f;
    float lastAI1Punishment;
    float lastAI2Punishment;
    float lastAI3Punishment;
    float totalHumanContributionRatio;
    float totalGroupContributionRatio;
    int humanPunishmentCount;

    public int currentRound = 0;
    public int maxRounds = 5;
    public float adaptationStrength = 0.15f;
    public float minAdaptationCoefficient = 0.95f;
    public float maxAdaptationCoefficient = 1.12f;

    public Slider investSlider;
    public TMP_Text investPreviewText;
    public TMP_Text humanText;
    public TMP_Text ai1Text;
    public TMP_Text ai2Text;
    public TMP_Text ai3Text;
    public TMP_Text resultText;
    public TMP_Text roundText;
    public GameObject endGamePopup;
    public TMP_Text winnerText;
    public Button returnToMainMenuButton;

    bool gameEnded = false;

    void Start()
    {
        ApplySandboxSettings();

        ai1Personality = GetRandomPersonality();
        ai2Personality = GetRandomPersonality();
        ai3Personality = GetRandomPersonality();

        if (SandboxMenuManager.sandboxIncludeFreeRider)
        {
            ai1Personality = "FreeRider";
        }

        Debug.Log("AI 1 = " + ai1Personality);
        Debug.Log("AI 2 = " + ai2Personality);
        Debug.Log("AI 3 = " + ai3Personality);

        if (investSlider != null)
        {
            investSlider.minValue = 0f;
            investSlider.maxValue = 100f;
            investSlider.wholeNumbers = true;
            investSlider.onValueChanged.AddListener(UpdateInvestPreview);
            UpdateInvestPreview(investSlider.value);
        }
        else
        {
            Debug.LogWarning("GameManager: investSlider is not assigned.");
            UpdateInvestPreview(0f);
        }

        SetupEndGamePopup();
        UpdateUI();
    }

    int GetAIInvestment(string type, int currentMoney, float lastAverage)
    {
        if (currentMoney <= 0)
        {
            return 0;
        }

        float ratio = 0f;

        if (type == "Banker")
        {ratio = Random.Range(0.5f, 0.7f);}
        else if (type == "Gambler")
        {ratio = Random.Range(0f, 1f);}
        else if (type == "FastFoodManager")
        {ratio = Mathf.Clamp(lastAverage, 0.2f, 0.7f);}
        else if (type == "FreeRider")
        {ratio = 0f;}

        int investment = Mathf.RoundToInt(currentMoney * ratio);
        return Mathf.Clamp(investment, 0, currentMoney);
    }

    void UpdateInvestPreview(float sliderValue)
    {
        if (investPreviewText == null)
        {
            return;
        }

        int percent = Mathf.Clamp(Mathf.RoundToInt(sliderValue), 0, 100);
        int moneyAmount = Mathf.RoundToInt(Mathf.Max(0, humanMoney) * (percent / 100f));

        investPreviewText.text = percent + "% = " + moneyAmount + " coins";
    }

    string GetRandomPersonality()
    {
        if (SandboxMenuManager.sandboxForcedPersonality != SandboxMenuManager.NoForcedPersonality)
        {
            return SandboxMenuManager.sandboxForcedPersonality;
        }

        string[] personalities = {
            "Banker",
            "Gambler",
            "FastFoodManager"
        };

        int randomIndex = Random.Range(0, personalities.Length);
        return personalities[randomIndex];
    }

    float GetPunishment(float contributionRatio, float threshold, float gain)
    {
        if (contributionRatio < threshold)
        {
            return gain * 0.5f;
        }

        return 0f;
    }

    string FormatPercent(float ratio)
    {
        return Mathf.RoundToInt(ratio * 100f) + "%";
    }

    string FormatCoins(float coins)
    {
        return Mathf.RoundToInt(coins) + " coins";
    }

    public void PlayRound()
    {
        if (gameEnded)
        {
            return;
        }

        if (investSlider == null)
        {
            Debug.LogWarning("GameManager: cannot play a round because investSlider is not assigned.");
            return;
        }

        int investPercent = Mathf.Clamp(Mathf.RoundToInt(investSlider.value), 0, 100);
        int humanInvest = Mathf.RoundToInt(Mathf.Max(0, humanMoney) * (investPercent / 100f));
        humanInvest = Mathf.Clamp(humanInvest, 0, humanMoney);

        int ai1Invest = GetAIInvestment(ai1Personality, ai1Money, lastAverage);
        int ai2Invest = GetAIInvestment(ai2Personality, ai2Money, lastAverage);
        int ai3Invest = GetAIInvestment(ai3Personality, ai3Money, lastAverage);

        int totalInvested = humanInvest + ai1Invest + ai2Invest + ai3Invest;

        float multipliedPot = totalInvested * potMultiplier;
        float share = multipliedPot / 4f;

        float humanRatio = humanInvest / Mathf.Max(1f, humanMoney);
        float ai1Ratio = ai1Invest / Mathf.Max(1f, ai1Money);
        float ai2Ratio = ai2Invest / Mathf.Max(1f, ai2Money);
        float ai3Ratio = ai3Invest / Mathf.Max(1f, ai3Money);

        float averageRatio = (humanRatio + ai1Ratio + ai2Ratio + ai3Ratio) / 4f;
        float punishmentThreshold = 0f;

        if (punishmentEnabled)
        {
            punishmentThreshold = averageRatio * 0.8f;
        }

        float humanGainBeforeAdaptation = share + alpha * humanInvest;
        int bestOpponentMoney = Mathf.Max(ai1Money, ai2Money, ai3Money);
        float adaptationGap = (bestOpponentMoney - humanMoney) / Mathf.Max(1f, bestOpponentMoney);
        float adaptationCoefficient = 1f + adaptationStrength * adaptationGap;
        adaptationCoefficient = Mathf.Clamp(adaptationCoefficient, minAdaptationCoefficient, maxAdaptationCoefficient);

        float humanGain = humanGainBeforeAdaptation * adaptationCoefficient;
        float ai1Gain = share + alpha * ai1Invest;
        float ai2Gain = share + alpha * ai2Invest;
        float ai3Gain = share + alpha * ai3Invest;

        float humanPunishment = 0f;
        float ai1Punishment = 0f;
        float ai2Punishment = 0f;
        float ai3Punishment = 0f;

        if (punishmentEnabled)
        {
            humanPunishment = GetPunishment(humanRatio, punishmentThreshold, humanGain);
            ai1Punishment = GetPunishment(ai1Ratio, punishmentThreshold, ai1Gain);
            ai2Punishment = GetPunishment(ai2Ratio, punishmentThreshold, ai2Gain);
            ai3Punishment = GetPunishment(ai3Ratio, punishmentThreshold, ai3Gain);
        }

        totalHumanContributionRatio += humanRatio;
        totalGroupContributionRatio += averageRatio;

        if (humanPunishment > 0f)
        {
            humanPunishmentCount++;
        }

        humanMoney = Mathf.RoundToInt((humanMoney - humanInvest) + humanGain - humanPunishment);
        ai1Money = Mathf.RoundToInt((ai1Money - ai1Invest) + ai1Gain - ai1Punishment);
        ai2Money = Mathf.RoundToInt((ai2Money - ai2Invest) + ai2Gain - ai2Punishment);
        ai3Money = Mathf.RoundToInt((ai3Money - ai3Invest) + ai3Gain - ai3Punishment);

        lastAverage = averageRatio;
        lastAI1Ratio = ai1Ratio;
        lastAI2Ratio = ai2Ratio;
        lastAI3Ratio = ai3Ratio;
        lastAI1Punishment = ai1Punishment;
        lastAI2Punishment = ai2Punishment;
        lastAI3Punishment = ai3Punishment;
        currentRound++;

        if (resultText != null)
        {
            string punishmentResult = "Punishment: disabled";

            if (punishmentEnabled)
            {
                punishmentResult =
                    "Punishment threshold: " + FormatPercent(punishmentThreshold) +
                    "\nYour punishment: " + FormatCoins(humanPunishment);
            }

            resultText.text =
                "Group average: " + FormatPercent(averageRatio) +
                "\nYour contribution: " + FormatPercent(humanRatio) +
                "\nYour gain: " + FormatCoins(humanGain) +
                "\n" + punishmentResult;
        }

        UpdateUI();
        UpdateInvestPreview(investSlider.value);

        if (currentRound >= maxRounds)
        {
            EndGame();
        }
    }

    void UpdateUI()
    {
        string[] names = { "You", GetAIName("AI 1", ai1Personality), GetAIName("AI 2", ai2Personality), GetAIName("AI 3", ai3Personality) };
        int[] moneyValues = { humanMoney, ai1Money, ai2Money, ai3Money };
        float[] contributionRatios = { -1f, lastAI1Ratio, lastAI2Ratio, lastAI3Ratio };
        float[] punishments = { 0f, lastAI1Punishment, lastAI2Punishment, lastAI3Punishment };

        for (int i = 0; i < moneyValues.Length - 1; i++)
        {
            for (int j = i + 1; j < moneyValues.Length; j++)
            {
                if (moneyValues[j] > moneyValues[i])
                {
                    Swap(names, i, j);
                    Swap(moneyValues, i, j);
                    Swap(contributionRatios, i, j);
                    Swap(punishments, i, j);
                }
            }
        }

        TMP_Text[] rankingTexts = { humanText, ai1Text, ai2Text, ai3Text };

        for (int i = 0; i < rankingTexts.Length; i++)
        {
            if (rankingTexts[i] != null)
            {
                rankingTexts[i].text = GetRankingLine(i + 1, names[i], moneyValues[i], contributionRatios[i], punishments[i]);
            }
        }

        if (roundText != null)
        {
            roundText.text = "Round: " + currentRound + " / " + maxRounds;
        }
    }

    string GetAIName(string aiName, string personality)
    {
        if (SandboxMenuManager.sandboxShowAIPersonalities)
        {
            return GetDisplayPersonalityName(personality);
        }

        return aiName;
    }

    string GetDisplayPersonalityName(string personality)
    {
        if (personality == "FreeRider")
        {
            return "Free Rider";
        }

        return personality;
    }

    string GetRankingLine(int rank, string playerName, int money, float contributionRatio, float punishment)
    {
        string text = rank + ". " + playerName + ": " + money + " coins";

        if (contributionRatio >= 0f)
        {
            text += "\nInvested: " + FormatPercent(contributionRatio);

            if (punishment > 0f)
            {
                text += " | Punished: " + FormatCoins(punishment);
            }
        }

        return text;
    }

    void Swap<T>(T[] values, int firstIndex, int secondIndex)
    {
        T temporaryValue = values[firstIndex];
        values[firstIndex] = values[secondIndex];
        values[secondIndex] = temporaryValue;
    }

    void ApplySandboxSettings()
    {
        maxRounds = SandboxMenuManager.sandboxMaxRounds;
        humanMoney = SandboxMenuManager.sandboxHumanStartMoney;
        ai1Money = SandboxMenuManager.sandboxAIStartMoney;
        ai2Money = SandboxMenuManager.sandboxAIStartMoney;
        ai3Money = SandboxMenuManager.sandboxAIStartMoney;
        potMultiplier = SandboxMenuManager.sandboxPotMultiplier;
        alpha = SandboxMenuManager.sandboxAlpha;
        punishmentEnabled = SandboxMenuManager.sandboxPunishmentEnabled;
    }

    void SetupEndGamePopup()
    {
        if (endGamePopup != null)
        {
            endGamePopup.SetActive(false);
        }

        if (returnToMainMenuButton != null)
        {
            returnToMainMenuButton.onClick.RemoveListener(ReturnToMainMenu);
            returnToMainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    void EndGame()
    {
        gameEnded = true;

        if (investSlider != null)
        {
            investSlider.interactable = false;
        }

        ShowEndGamePopup(GetWinnerText());
    }

    string GetWinnerText()
    {
        string[] playerNames = {
            "You",
            GetAIName("AI 1", ai1Personality),
            GetAIName("AI 2", ai2Personality),
            GetAIName("AI 3", ai3Personality)
        };
        int[] playerMoney = { humanMoney, ai1Money, ai2Money, ai3Money };

        int highestMoney = playerMoney[0];

        for (int i = 1; i < playerMoney.Length; i++)
        {
            if (playerMoney[i] > highestMoney)
            {
                highestMoney = playerMoney[i];
            }
        }

        string winners = string.Empty;

        for (int i = 0; i < playerMoney.Length; i++)
        {
            if (playerMoney[i] == highestMoney)
            {
                if (string.IsNullOrEmpty(winners))
                {
                    winners += playerNames[i];
                }
                else
                {
                    winners += ", " + playerNames[i];
                }
            }
        }

        string title;

        if (humanMoney == highestMoney)
        {
            title = winners == "You" ? "You won!" : "Tie game!";
        }
        else
        {
            title = "Game over";
        }

        return title + "\nWinner: " + winners + "\nMoney: " + highestMoney + " coins";
    }

    void ShowEndGamePopup(string message)
    {
        if (winnerText != null)
        {
            float averageHumanContribution = totalHumanContributionRatio / Mathf.Max(1, currentRound);
            float averageGroupContribution = totalGroupContributionRatio / Mathf.Max(1, currentRound);
            int totalGroupMoney = humanMoney + ai1Money + ai2Money + ai3Money;
            int fullCooperationBenchmark = GetFullCooperationBenchmark();
            float benchmarkReached = totalGroupMoney / Mathf.Max(1f, fullCooperationBenchmark);
            float distanceFromCollectiveOptimum = Mathf.Max(0f, 1f - benchmarkReached);
            int richestPlayerMoney = Mathf.Max(humanMoney, ai1Money, ai2Money, ai3Money);
            int poorestPlayerMoney = Mathf.Min(humanMoney, ai1Money, ai2Money, ai3Money);
            int wealthGap = richestPlayerMoney - poorestPlayerMoney;
            string cooperationLevel = "Medium";

            if (averageGroupContribution < 0.33f)
            {
                cooperationLevel = "Low";
            }
            else if (averageGroupContribution >= 0.66f)
            {
                cooperationLevel = "High";
            }

            winnerText.text =
                message +
                "\n\nLEARNING SUMMARY" +
                "\nYour average contribution: " + FormatPercent(averageHumanContribution) +
                "\nTimes punished: " + humanPunishmentCount +
                "\nGroup cooperation: " + cooperationLevel + " (" + FormatPercent(averageGroupContribution) + ")" +
                "\n\nCOLLECTIVE OUTCOME" +
                "\nGroup wealth: " + totalGroupMoney + " / " + fullCooperationBenchmark + " benchmark coins" +
                "\nDistance from collective optimum: " + FormatPercent(distanceFromCollectiveOptimum) +
                "\nHow to reach it: everyone invests more. Yes, that is basically it." +
                "\nWealth gap: " + wealthGap + " coins" +
                "\nIndividual success and collective success are not always the same.";

            if (!string.IsNullOrEmpty(SandboxMenuManager.sandboxLesson))
            {
                winnerText.text += "\n\nLESSON TO TAKE AWAY\n" + SandboxMenuManager.sandboxLesson;
            }
        }

        if (endGamePopup != null)
        {
            endGamePopup.SetActive(true);
        }
    }

    int GetFullCooperationBenchmark()
    {
        int benchmarkHumanMoney = SandboxMenuManager.sandboxHumanStartMoney;
        int benchmarkAI1Money = SandboxMenuManager.sandboxAIStartMoney;
        int benchmarkAI2Money = SandboxMenuManager.sandboxAIStartMoney;
        int benchmarkAI3Money = SandboxMenuManager.sandboxAIStartMoney;

        for (int round = 0; round < maxRounds; round++)
        {
            int totalInvestment = benchmarkHumanMoney + benchmarkAI1Money + benchmarkAI2Money + benchmarkAI3Money;
            float sharedReturn = totalInvestment * potMultiplier / 4f;
            int bestOpponentMoney = Mathf.Max(benchmarkAI1Money, benchmarkAI2Money, benchmarkAI3Money);
            float adaptationGap = (bestOpponentMoney - benchmarkHumanMoney) / Mathf.Max(1f, bestOpponentMoney);
            float adaptationCoefficient = 1f + adaptationStrength * adaptationGap;
            adaptationCoefficient = Mathf.Clamp(adaptationCoefficient, minAdaptationCoefficient, maxAdaptationCoefficient);

            benchmarkHumanMoney = Mathf.RoundToInt((sharedReturn + alpha * benchmarkHumanMoney) * adaptationCoefficient);
            benchmarkAI1Money = Mathf.RoundToInt(sharedReturn + alpha * benchmarkAI1Money);
            benchmarkAI2Money = Mathf.RoundToInt(sharedReturn + alpha * benchmarkAI2Money);
            benchmarkAI3Money = Mathf.RoundToInt(sharedReturn + alpha * benchmarkAI3Money);
        }

        return benchmarkHumanMoney + benchmarkAI1Money + benchmarkAI2Money + benchmarkAI3Money;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
