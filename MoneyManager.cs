using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public TMP_Text MoneyLabel;
    public int Money; 

    public void Start()
    {
        MoneyLabel.text = Money.ToString();

        if (PlayerPrefs.HasKey("Money"))
        {
            Money = PlayerPrefs.GetInt("Money");
        }

        MoneyLabel.text = Money.ToString();
    }

    public void ChangeMoney(int amount)
    {
        Money += amount;
        MoneyLabel.text = Money.ToString();

        PlayerPrefs.SetInt("Money", Money);
    }
}
