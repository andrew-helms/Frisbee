using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelEndMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private RunManager runManager;

    public void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 > 2)
        {
            nextLevelButton.interactable = false;
        }
    }

    public void NextLevel()
    {
        Debug.Log("Next level!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetHighScore()
    {
        float bestTime = PlayerPrefs.GetFloat($"level{SceneManager.GetActiveScene().buildIndex}", float.MaxValue);
        if (runManager.CurrentTime < bestTime)
        {
            bestTime = runManager.CurrentTime;
            PlayerPrefs.SetFloat($"level{SceneManager.GetActiveScene().buildIndex}", bestTime);
        }

        int minutes = Mathf.FloorToInt(bestTime / 60);
        int seconds = Mathf.FloorToInt(bestTime - minutes * 60);
        int milliseconds = (int)((bestTime - Mathf.Floor(bestTime)) * 100) % 100;
        bestTimeText.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}.{milliseconds.ToString("00")}";
    }
}
