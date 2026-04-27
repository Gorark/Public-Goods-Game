using UnityEngine;
using TMPro;

public class InvestButton : MonoBehaviour
{
    public int Playermoney = 100;
    public TMP_Text moneyText;

    public void Invest()
{
    if (Playermoney < 10) return;

    Playermoney -= 10;
    moneyText.text = "Money: " + Playermoney;
}

    void Start()
    {
        moneyText.text = "Money: " + Playermoney;
    }
}