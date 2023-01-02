using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RunManager runManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!runManager.Paused)
        {
            runManager.AddTime(Time.deltaTime);
        }
        int minutes = Mathf.FloorToInt(runManager.CurrentTime / 60);
        int seconds = Mathf.FloorToInt(runManager.CurrentTime - minutes * 60);
        int milliseconds = (int)((runManager.CurrentTime - Mathf.Floor(runManager.CurrentTime)) * 100) % 100;
        text.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}.{milliseconds.ToString("00")}";
    }
}
