using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UIStates")]
    public GameObject GardenStatePanel;
    public GameObject TasksStatePanel;
    public GameObject QuizStatePanel;
    public GameObject AboutStatePanel;
    public GameObject TreeStatePanel;
    public GameObject BuyStatePanel;

    [Header("Scripts")]
    public QuizManager QuizManagerScript; 
    public GardenManager GardenManagerScript;
    
    public void ChangeUIState(int statee)
    {
        // Always disable all panels except for the specific case
        if (statee != 2 || (statee == 2 && QuizManagerScript.QuizDayLimitCheck()))
        {
            DisableAllStatePanel();
        }

        switch (statee)
        {
            case 0:
                GardenStatePanel.SetActive(true);
                break;
            case 1:
                TasksStatePanel.SetActive(true);
                break;
            case 2:
                // Only activate QuizStatePanel if the QuizDayLimitCheck() is true
                if (QuizManagerScript.QuizDayLimitCheck())
                {
                    QuizStatePanel.SetActive(true);
                    QuizManagerScript.GenerateQuestion();
                }
                break;
            case 3:
                AboutStatePanel.SetActive(true);
                break;
            case 4:
                TreeStatePanel.SetActive(true);

                foreach (Button but in GardenManagerScript.TreeButtons)
                {
                    but.GetComponent<Image>().enabled = false;
                }
                break;
            case 5:
                BuyStatePanel.SetActive(true);
                break;
            case 6:
                Application.Quit();
                break;
        }
    }

    private void DisableAllStatePanel()
    {
        GardenStatePanel.SetActive(false);
        TasksStatePanel.SetActive(false);
        QuizStatePanel.SetActive(false);
        AboutStatePanel.SetActive(false);
        TreeStatePanel.SetActive(false);
        BuyStatePanel.SetActive(false);
    }
}
