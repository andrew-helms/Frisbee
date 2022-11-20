using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
