using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    public TMP_InputField investInput;
    public Slider investSlider;
    public TMP_Text investPreviewText;
    public TMP_Text humanText;
    public TMP_Text ai1Text;
    public TMP_Text ai2Text;
    public TMP_Text ai3Text;
    public TMP_Text resultText;

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
            investSlider.onValueChanged.AddListener(UpdateInvestPreview);
            UpdateInvestPreview(investSlider.value);
        }
        else
        {
            Debug.LogWarning("GameManager: investSlider is not assigned.");
            UpdateInvestPreview(0f);
        }

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

        float humanGain = share + alpha * humanInvest;
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
                "\nYou gain: " + FormatCoins(humanGain) +
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
    }
}
