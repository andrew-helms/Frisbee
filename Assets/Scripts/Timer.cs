using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private Stopwatch sw;
    // Start is called before the first frame update
    void Start()
    {
        sw = Stopwatch.StartNew();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = sw.Elapsed.ToString("mm\\:ss\\.ff");
    }
}
