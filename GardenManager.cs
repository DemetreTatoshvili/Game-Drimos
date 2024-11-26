using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GardenManager : MonoBehaviour
{
    public List<Button> TreeButtons;
    public List<Button> AddButtons;
    public Button BuyButton;
    public UIManager UIManagerScript;
    public MoneyManager MoneyManagerScript;
    public RawImage TreePanelInamge;
    public List<Texture> TreeImages;
    public TMP_Text TreesQuantityLabel;
    public TMP_Text BuyPanelQuestion;

    public TMP_Text TreeNameLabel;
    public TMP_Text LocationLabel;
    public TMP_Text AgeLabel;
    public QuestionsDataStore QuestionsDataStoreScript;

    public int AddButtonIndex;
    public int price;

    void Start()
    {
        foreach(Button but in AddButtons)
        {
            but.GetComponent<Button>().onClick.AddListener(() => UIManagerScript.ChangeUIState(5));
            but.GetComponent<Button>().onClick.AddListener(() => DoubleCall(but));
        }

        if (PlayerPrefs.HasKey("AddButtonIndex"))
        {
            AddButtonIndex = PlayerPrefs.GetInt("AddButtonIndex");

            foreach (Button but in TreeButtons)
            {
                if(TreeButtons.IndexOf(but) < AddButtonIndex)
                {
                    but.transform.parent.gameObject.SetActive(true);

                    but.onClick.AddListener(() => UIManagerScript.ChangeUIState(4));
                    but.onClick.AddListener(() => CustomizeTreePanel(AddButtons[TreeButtons.IndexOf(but)]));
                }
            }
        }

        if (AddButtonIndex != 6)
        {
            AddButtons[AddButtonIndex].transform.parent.gameObject.SetActive(true);
        }

        TreesQuantityLabel.text = $"ხეები {AddButtonIndex}/6";
    }

    public void CallBuyButton(Button but)
    {
        if (MoneyManagerScript.Money - price >= 0)
        {
            MoneyManagerScript.ChangeMoney(-1 * price);

            but.transform.parent.gameObject.SetActive(false);
            TreeButtons[AddButtons.IndexOf(but)].transform.parent.gameObject.SetActive(true);

            //TreeButtons[AddButtons.IndexOf(but)].onClick.AddListener(() => but.GetComponent<Image>().enabled = false);

            TreeButtons[AddButtons.IndexOf(but)].onClick.AddListener(() => UIManagerScript.ChangeUIState(4));

            TreeButtons[AddButtons.IndexOf(but)].onClick.AddListener(() => CustomizeTreePanel(but));

            if (AddButtons.IndexOf(but) != 5)
            {
                AddButtons[AddButtons.IndexOf(but) + 1].transform.parent.gameObject.SetActive(true);
            }

            AddButtonIndex = AddButtons.IndexOf(but) + 1;

            TreesQuantityLabel.text = $"ხეები {AddButtonIndex}/6";

            PlayerPrefs.SetInt("AddButtonIndex", AddButtonIndex);
        }
    }

    public void DoubleCall(Button but)
    {
        price = 10 * (10 * (AddButtonIndex + 1) - 5);
        BuyPanelQuestion.text = $"გსურთ რომ გადაიხადოთ {price} ქულა?";

        // Remove previous listeners to avoid triggering multiple times
        BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
        
        // Add the new listener
        BuyButton.GetComponent<Button>().onClick.AddListener(() => CallBuyButton(but));
    }

    public void CustomizeTreePanel(Button but)
    {
        int index = AddButtons.IndexOf(but);
        TreePanelInamge.texture = TreeButtons[index].transform.GetChild(0).GetComponent<RawImage>().texture;

        string loc = QuestionsDataStoreScript.LocationsData[index];
        LocationLabel.text = $"მდებარეობა: {loc}";

        TreeNameLabel.text = QuestionsDataStoreScript.TreeNamesData[index];

        string age = QuestionsDataStoreScript.TreeAgesData[index];
        AgeLabel.text = $"ასაკი: {age}";
    }
}
