using UnityEngine;
using TMPro;

public class InvestButton : MonoBehaviour
{
    public int money = 100;
    public TMP_Text moneyText;

    public void Invest()
{
    if (money < 10) return;

    money -= 10;
    moneyText.text = "Money: " + money;
}

    void Start()
    {
        moneyText.text = "Money: " + money;
    }
}