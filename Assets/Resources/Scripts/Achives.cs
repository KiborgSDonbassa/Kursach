using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Achives : MonoBehaviour
{
    [SerializeField] private GameObject AchiveWindow;
    public static GameObject AchiveWindowStatic;
    public static int countOfBandits = 5;
    
    private void Awake()
    {
        AchiveWindowStatic = AchiveWindow;
        SetAchive();
    }
    private static void SetAchive()
    {
        TextMeshProUGUI AchiveText = AchiveWindowStatic.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Score = AchiveWindowStatic.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        AchiveText.SetText("Вбий розбійників");
        Score.SetText(countOfBandits.ToString() + "/5");
    }
    public static void OnKill()
    {
        TextMeshProUGUI AchiveText = AchiveWindowStatic.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Score = AchiveWindowStatic.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        countOfBandits = countOfBandits - 1;
        if (countOfBandits == 0)
        {
            AchiveText.color = Color.gray;
            Score.color = Color.gray;
            AchiveWindowStatic.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().gameObject.SetActive(true);
            Score.SetText(countOfBandits.ToString() + "/5");
            return;
        }
        Score.SetText(countOfBandits.ToString() + "/5");
    }

    public void SwitchWindow()
    {
        if (AchiveWindow.activeSelf == true)
            AchiveWindow.SetActive(false);
        else
            AchiveWindow.SetActive(true);
    }
}
