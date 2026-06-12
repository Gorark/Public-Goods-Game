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

    public int currentRound = 0;
    public int maxRounds = 5;
    public float adaptationStrength = 0.15f;
    public float minAdaptationCoefficient = 0.95f;
    public float maxAdaptationCoefficient = 1.12f;

    public TMP_InputField investInput;
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
        ai1Personality = GetRandomPersonality();
        ai2Personality = GetRandomPersonality();
        ai3Personality = GetRandomPersonality();

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

        if (type == "Banquier")
        {ratio = Random.Range(0.5f, 0.7f);}
        else if (type == "Gambler")
        {ratio = Random.Range(0f, 1f);}
        else if (type == "Patron")
        {ratio = Random.Range(0.25f, 0.45f);}
        else if (type == "FastFoodManager")
        {ratio = Mathf.Clamp(lastAverage, 0.2f, 0.7f);}
        else if (type == "FreeRider")
        {ratio = Random.Range(0.2f, 0.3f);}

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
        string[] personalities = {
            "Banquier",
            "Gambler",
            "Patron",
            "FastFoodManager",
            "FreeRider"
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

        float multipliedPot = totalInvested * 1.6f;
        float share = multipliedPot / 4f;

        float humanRatio = humanInvest / Mathf.Max(1f, humanMoney);
        float ai1Ratio = ai1Invest / Mathf.Max(1f, ai1Money);
        float ai2Ratio = ai2Invest / Mathf.Max(1f, ai2Money);
        float ai3Ratio = ai3Invest / Mathf.Max(1f, ai3Money);

        float averageRatio = (humanRatio + ai1Ratio + ai2Ratio + ai3Ratio) / 4f;
        float punishmentThreshold = averageRatio * 0.8f;

        float humanGainBeforeAdaptation = share + alpha * humanInvest;
        int bestOpponentMoney = Mathf.Max(ai1Money, ai2Money, ai3Money);
        float adaptationGap = (bestOpponentMoney - humanMoney) / Mathf.Max(1f, bestOpponentMoney);
        float adaptationCoefficient = 1f + adaptationStrength * adaptationGap;
        adaptationCoefficient = Mathf.Clamp(adaptationCoefficient, minAdaptationCoefficient, maxAdaptationCoefficient);

        float humanGain = humanGainBeforeAdaptation * adaptationCoefficient;
        float ai1Gain = share + alpha * ai1Invest;
        float ai2Gain = share + alpha * ai2Invest;
        float ai3Gain = share + alpha * ai3Invest;

        float humanPunishment = GetPunishment(humanRatio, punishmentThreshold, humanGain);
        float ai1Punishment = GetPunishment(ai1Ratio, punishmentThreshold, ai1Gain);
        float ai2Punishment = GetPunishment(ai2Ratio, punishmentThreshold, ai2Gain);
        float ai3Punishment = GetPunishment(ai3Ratio, punishmentThreshold, ai3Gain);

        humanMoney = Mathf.RoundToInt((humanMoney - humanInvest) + humanGain - humanPunishment);
        ai1Money = Mathf.RoundToInt((ai1Money - ai1Invest) + ai1Gain - ai1Punishment);
        ai2Money = Mathf.RoundToInt((ai2Money - ai2Invest) + ai2Gain - ai2Punishment);
        ai3Money = Mathf.RoundToInt((ai3Money - ai3Invest) + ai3Gain - ai3Punishment);

        lastAverage = averageRatio;
        currentRound++;

        if (resultText != null)
        {
            resultText.text =
                "Last Round Investments:" +
                "\nYou invested: " + humanInvest + " coins (" + FormatPercent(humanRatio) + ")" +
                "\nAI 1 invested: " + ai1Invest + " coins (" + FormatPercent(ai1Ratio) + ")" +
                "\nAI 2 invested: " + ai2Invest + " coins (" + FormatPercent(ai2Ratio) + ")" +
                "\nAI 3 invested: " + ai3Invest + " coins (" + FormatPercent(ai3Ratio) + ")" +
                "\nTotal pot: " + totalInvested + " coins" +
                "\nMultiplied pot: " + FormatCoins(multipliedPot) +
                "\nShare per player: " + FormatCoins(share) +
                "\nYour base gain: " + FormatCoins(humanGainBeforeAdaptation) +
                "\nBest opponent money: " + bestOpponentMoney + " coins" +
                "\nAdaptation gap: " + FormatPercent(adaptationGap) +
                "\nAdaptation coefficient: " + adaptationCoefficient.ToString("0.00") +
                "\nYou gain after adaptation: " + FormatCoins(humanGain) +
                "\nAI 1 gain: " + FormatCoins(ai1Gain) +
                "\nAI 2 gain: " + FormatCoins(ai2Gain) +
                "\nAI 3 gain: " + FormatCoins(ai3Gain) +
                "\nAverage contribution: " + FormatPercent(averageRatio) +
                "\nPunishment threshold: " + FormatPercent(punishmentThreshold) +
                "\nYour punishment: " + FormatCoins(humanPunishment) +
                "\nAI 1 punishment: " + FormatCoins(ai1Punishment) +
                "\nAI 2 punishment: " + FormatCoins(ai2Punishment) +
                "\nAI 3 punishment: " + FormatCoins(ai3Punishment);
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
        if (humanText != null)
        {
            humanText.text = "You: " + humanMoney + " coins";
        }

        if (ai1Text != null)
        {
            ai1Text.text = "AI 1: " + ai1Money + " coins";
        }

        if (ai2Text != null)
        {
            ai2Text.text = "AI 2: " + ai2Money + " coins";
        }

        if (ai3Text != null)
        {
            ai3Text.text = "AI 3: " + ai3Money + " coins";
        }

        if (roundText != null)
        {
            roundText.text = "Round: " + currentRound + " / " + maxRounds;
        }
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
        string[] playerNames = { "You", "AI 1", "AI 2", "AI 3" };
        int[] playerMoney = { humanMoney, ai1Money, ai2Money, ai3Money };

        int highestMoney = playerMoney[0];

        for (int i = 1; i < playerMoney.Length; i++)
        {
            if (playerMoney[i] > highestMoney)
            {
                highestMoney = playerMoney[i];
            }
        }

        string winners = "";

        for (int i = 0; i < playerMoney.Length; i++)
        {
            if (playerMoney[i] == highestMoney)
            {
                if (winners == "")
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
            winnerText.text = message;
        }

        if (endGamePopup != null)
        {
            endGamePopup.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
