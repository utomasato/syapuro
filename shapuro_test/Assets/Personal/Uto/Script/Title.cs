using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Title : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private Color32 startColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Color32 endColor = new Color32(255, 255, 255, 64);
    [SerializeField] private float duration = 1.0f;
    private bool hasKeyBeenPressed = false;
    [SerializeField] private SceneChange sceneChange;
    [SerializeField] private string NextScene;

    void Update()
    {
        tmp.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / duration, 1.0f));
        if (!hasKeyBeenPressed && Input.anyKeyDown)
        {
            hasKeyBeenPressed = true;
            sceneChange.StartFadeOut(NextScene);
            Debug.Log("Pressed any key");
        }
    }
}
