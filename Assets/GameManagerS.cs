using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManagers : MonoBehaviour
{
    public TMP_InputField investInput;
    public Slider investSlider;
    public TMP_Text investPreviewText;
    public TMP_Text humanText;
    public TMP_Text ai1Text;
    public TMP_Text ai2Text;
    public TMP_Text ai3Text;
    public TMP_Text resultText;
    string ai1Personality;
    string ai2Personality;
    string ai3Personality;
    float lastAverage = 0.5f;
    int humanMoney = 100;
    int ai1Money = 100;
    int ai2Money = 100;
    int ai3Money = 100;
    float alpha = 0.6f;


    void Start()
    {
        

        Debug.Log("AI 1 = " + ai1Personality);
        Debug.Log("AI 2 = " + ai2Personality);
        Debug.Log("AI 3 = " + ai3Personality);
    }
    int GetAIInvestment(string type, int currentMoney, float lastAverage)
{
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

    return Mathf.RoundToInt(currentMoney * ratio);
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
    public void PlayRound()
    {
        int humanInvest = int.Parse(investInput.text);

        if (humanInvest < 0) humanInvest = 0;
        if (humanInvest > humanMoney) humanInvest = humanMoney;

        
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

        resultText.text =
        "Last Round Investments:"+
            "\nYou invested: " + humanInvest +
            "\nAI 1 invested: " + ai1Invest +
            "\nAI 2 invested: " + ai2Invest +
            "\nAI 3 invested: " + ai3Invest +
            "\nTotal pot: " + totalInvested +
            "\nYou receive: " + humanMoney +
            "\nIA 1 receive: " + ai1Money +
            "\nIA 2 receive: " + ai2Money +
            "\nIA 3 receive: " + ai3Money +
            "\nAverage ratio: " + averageRatio+"%"+
            "\nYour ratio:" + humanRatio +
            "\nPunishment threshold: " + punishmentThreshold+"%"+
            "\nYou punishment: " + humanPunishment+
            "\nAI 1 punishment: " + ai1Punishment+
            "\nAI 2 punishment: " + ai2Punishment+
            "\nAI 3 punishment: " + ai3Punishment;

        UpdateUI();
    }

    void UpdateUI()
    {
        humanText.text = "You: " + humanMoney;
        ai1Text.text = "AI 1: " + ai1Money;
        ai2Text.text = "AI 2: " + ai2Money;
        ai3Text.text = "AI 3: " + ai3Money;
    }
}