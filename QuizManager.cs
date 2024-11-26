using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip CorrectAudioClip;
    public AudioClip WrongAudioClip;
    public AudioClip CompliteAudioClip;

    public AudioSource AudioSourceManager;

    [Header("QuizUI")]
    public TMP_Text ComplitedQuizQuantityLabel;
    public TMP_Text QuestionIndexLabel;
    public TMP_Text QuestionLabel;
    public TMP_Text QuizQuantityLabel;
    public TMP_Text TodayQuizQuantityLabel;

    public List<Button> AnswerButtons;

    [Header("QuestionsData")]
    public int QuestionIndex;
    public List<string> QuestionsData;
    private List<string> QuestionList;

    [Header("Scripts")]
    public UIManager UIManagerScript;
    public MoneyManager MoneyManagerScript;
    public QuestionsDataStore QuestionsDataStoreScript;

    [Header("Variables")]
    public int QuestionsLimit;
    public int ComplitedQuiz;
    public int TodayComplitedQuiz;
    public int DayLimit;

    private DateTime targetTime;

    void Start()
    {
        Debug.Log("Got QuestionsData");
        QuestionsData = new List<string>(QuestionsDataStoreScript.QuestionsData);

        // Check if we have a saved target time
        if (PlayerPrefs.HasKey("TargetTime"))
        {
            string savedTime = PlayerPrefs.GetString("TargetTime");
            targetTime = DateTime.Parse(savedTime);
        }

        if (PlayerPrefs.HasKey("ComplitedQuiz"))
        {
            ComplitedQuiz = PlayerPrefs.GetInt("ComplitedQuiz");
            TodayComplitedQuiz = PlayerPrefs.GetInt("TodayComplitedQuiz");
        }

        if (PlayerPrefs.HasKey("QuestionIndex"))
        {
            QuestionIndex = PlayerPrefs.GetInt("QuestionIndex");
        }

        // Reset daily progress if target time has passed
        if (DateTime.Now >= targetTime)
        {
            TodayComplitedQuiz = 0;
            PlayerPrefs.DeleteKey("TargetTime");
        }

        QuizQuantityLabel.text = "ქვიზი " + ComplitedQuiz.ToString();
        TodayQuizQuantityLabel.text = $"დღეისთვის {TodayComplitedQuiz}/{DayLimit}";

        TodayComplitedQuiz = PlayerPrefs.GetInt("TodayComplitedQuiz");
    }

    public void GenerateQuestion()
    {
        QuestionIndexLabel.text = "შეკითხვა" + (QuestionIndex - ComplitedQuiz * QuestionsLimit + 1).ToString();
        
        // Parse the current question and answers
        QuestionList = QuestionsData[QuestionIndex].Split('|').ToList();
        
        // Set the question label
        QuestionLabel.text = QuestionList[0];

        // Set the answers and add listeners
        for (int i = 1; i < 5; i++)
        {
            int index = i; // Capture the current value of `i` in a local variable
            AnswerButtons[i - 1].transform.GetChild(0).GetComponent<TMP_Text>().text = QuestionList[index];

            // Remove any old listeners to avoid stacking listeners
            AnswerButtons[i - 1].onClick.RemoveAllListeners();

            // Add a new listener for this specific button
            AnswerButtons[i - 1].onClick.AddListener(() => CheckQuestionAnswer(QuestionList[index]));
        }
    }

    public void CheckQuestionAnswer(string Answer)
    {
        // Log the result
        if (Answer == QuestionList[5])
        {
            MoneyManagerScript.ChangeMoney(10);
            Debug.Log("Correct!");

            AudioSourceManager.clip = CorrectAudioClip;
        }
        else
        {
            Debug.Log("Incorrect!");
            AudioSourceManager.clip = WrongAudioClip;
        }

        AudioSourceManager.Play();

        SkipQuestion(false);
    }

    public void CompliteQuiz()
    {
        ComplitedQuiz ++;
        TodayComplitedQuiz = ComplitedQuiz;

        QuizQuantityLabel.text = "ქვიზი " + ComplitedQuiz.ToString();
        TodayQuizQuantityLabel.text = $"დღეისათვის {TodayComplitedQuiz}/{DayLimit}";

        PlayerPrefs.SetInt("ComplitedQuiz", ComplitedQuiz);
        PlayerPrefs.SetInt("TodayComplitedQuiz", TodayComplitedQuiz);

        PlayerPrefs.SetInt("TodayComplitedQuiz", TodayComplitedQuiz);

        AudioSourceManager.clip = CompliteAudioClip;
        AudioSourceManager.Play();

        SetDayComplitedZero();
    }

    public bool QuizDayLimitCheck()
    {
        if (TodayComplitedQuiz < DayLimit)
        {
            return true;
        }

        return false;
    }

    public void SetDayComplitedZero()
    {
        if (TodayComplitedQuiz == 1)
        {
            // Always set the target time if no active target is stored
            if (!PlayerPrefs.HasKey("TargetTime"))
            {
                targetTime = DateTime.Now.AddDays(1);
                PlayerPrefs.SetString("TargetTime", targetTime.ToString());
            }
        }
    }

    public void SkipQuestion(bool IsPricedSkip)
    {
        if (IsPricedSkip)
        {
            if (MoneyManagerScript.Money - 45 >= 0)
            {
                MoneyManagerScript.ChangeMoney(-45);

                NextStep();
            }
            else
            {
                return;
            }
        }
        else
        {
            NextStep();
        }

        PlayerPrefs.SetInt("QuestionIndex", QuestionIndex);
    }

    public void NextStep()
    {
        // Move to the next question
        QuestionIndex++;

        // Check if there are more questions
        if (QuestionIndex - ComplitedQuiz * QuestionsLimit < QuestionsLimit)
        {
            GenerateQuestion();
        }
        else
        {
            // Change the UI state when the quiz is complete
            CompliteQuiz();
            UIManagerScript.ChangeUIState(1);
            Debug.Log("Quiz Completed!");
        }
    }
}
