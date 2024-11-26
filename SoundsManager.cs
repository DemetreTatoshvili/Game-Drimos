using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsManager : MonoBehaviour
{
    public List<Button> Buttons;
    public AudioSource AudioSourceManager;
    public void Start()
    {
        foreach(Button but in Buttons)
        {
            but.onClick.AddListener(() => AudioSourceManager.Play());
        }
    }
}
