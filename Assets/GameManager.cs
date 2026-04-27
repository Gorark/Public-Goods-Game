using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_InputField investInput;

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

    void Start()
    {
        UpdateUI();
        ai1Personality = GetRandomPersonality();
        ai2Personality = GetRandomPersonality();
        ai3Personality = GetRandomPersonality();

        Debug.Log("AI 1 = " + ai1Personality);
        Debug.Log("AI 2 = " + ai2Personality);
        Debug.Log("AI 3 = " + ai3Personality);
    }
    int GetAIInvestment(string type, int currentMoney, float lastAverage)
{
    float ratio = 0f;

    if (type == "Banquier")
    {
        ratio = Random.Range(0.5f, 0.7f);
    }
    else if (type == "Gambler")
    {
        ratio = Random.Range(0f, 1f);
    }
    else if (type == "Patron")
    {
        ratio = Random.Range(0.25f, 0.45f);
    }
    else if (type == "FastFoodManager")
    {
        ratio = Mathf.Clamp(lastAverage, 0.2f, 0.7f);
    }
    else if (type == "FreeRider")
    {
        ratio = Random.Range(0f, 0.1f);
    }

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

        humanMoney = Mathf.RoundToInt(humanMoney - humanInvest + share);
        ai1Money = Mathf.RoundToInt(ai1Money - ai1Invest + share);
        ai2Money = Mathf.RoundToInt(ai2Money - ai2Invest + share);
        ai3Money = Mathf.RoundToInt(ai3Money - ai3Invest + share);

        resultText.text =
        "Last Round Investments:"+
            "\nYou invested: " + humanInvest +
            "\nAI 1 invested: " + ai1Invest +
            "\nAI 2 invested: " + ai2Invest +
            "\nAI 3 invested: " + ai3Invest +
            "\nTotal pot: " + totalInvested +
            "\nMultiplied pot: " + multipliedPot +
            "\nEach receives: " + share;

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