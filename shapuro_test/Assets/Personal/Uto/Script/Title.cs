using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private Color32 startColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Color32 endColor = new Color32(255, 255, 255, 64);
    [SerializeField] private float duration = 1.0f;
    private bool hasKeyBeenPressed = false;
    [SerializeField] private SceneChange sceneChange;
    [SerializeField] private string NextScene;
    [SerializeField] private UnityEngine.UI.Button button;
    private UnityEngine.UI.Button lastSelectedButton;
    private bool buttonClicked;

    [SerializeField] private AudioClip TitleBGM;
    [SerializeField] private AudioSource BGM;

    [SerializeField] private AudioClip ButtonSE;
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
        lastSelectedButton = button;
        buttonClicked = false;
        BGM = GetComponent<AudioSource>();
        BGM.volume = 0.1f;
        BGM.clip = TitleBGM;
        BGM.Play();
    }

    void Update()
    {
        /*
        tmp.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time / duration, 1.0f));
        if (!hasKeyBeenPressed && Input.anyKeyDown)
        {
            hasKeyBeenPressed = true;
            sceneChange.StartFadeOut(NextScene);
            Debug.Log("Pressed any key");
        }
        */
        if (!buttonClicked)
        {
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject; // 現在選択されているUI要素を取得 
            // 現在選択されているものがボタンでない場合、最後に選択されたボタンを再選択する
            if (currentSelected == null || currentSelected.GetComponent<UnityEngine.UI.Button>() == null)
            {
                if (lastSelectedButton != null)
                {
                    EventSystem.current.SetSelectedGameObject(lastSelectedButton.gameObject);
                }
            }
            else // 現在選択されているものがボタンであれば、それを記憶
            {
                lastSelectedButton = currentSelected.GetComponent<UnityEngine.UI.Button>();
            }
        }
    }

    public void Click()
    {
        buttonClicked = true;
    }
}
