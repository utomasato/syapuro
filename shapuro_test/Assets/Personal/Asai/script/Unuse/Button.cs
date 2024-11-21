using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void Retry()
    {
        if (SceneManager.GetActiveScene().name == "Test")
        {
            SceneManager.LoadScene("Test");
        }
    }
    public void NextStage()
    {
        if (SceneManager.GetActiveScene().name == "Test")
        {
            SceneManager.LoadScene("Test");
        }
        else if (SceneManager.GetActiveScene().name == "Test")
        {
            SceneManager.LoadScene("Test");
        }
    }
    public void Test()
    {
        SceneManager.LoadScene("test");
    }
}