using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Event : MonoBehaviour
{
    public bool used = false;
    public Fire fire_script;
    public GameState GameState;
    public GameObject Event_img;
    public UnityEngine.UI.Button button;
    public enum Mode
    {
        Mode_Next01,
        Mode_Next02,
        Mode_End
    }
    public Mode mode;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("Player");
        fire_script = Gamestate.GetComponent<Fire>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && used == false && GameState.JudgeState("GamePlay"))
        {
            Event_img.SetActive(true);
            GameState.OpenExplain();
            used = true;
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }
    // void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.name == "Fire")
    //     {
    //         Event_img.SetActive(false);
    //     }
    // }
    public void next01()
    {
        Debug.Log("next01");
        if (Event_img.gameObject.name == "Event01_img")
        {
            Event_img.SetActive(false);
            //Debug.Log("aaa");
        }
    }
    public void next02()
    {
        Debug.Log("next02");
        if (Event_img.gameObject.name == "Event01_img")
        {
            Event_img.SetActive(true);
            EventSystem.current.SetSelectedGameObject(button.gameObject);
            //Debug.Log("aaa");
        }
    }
    public void end()
    {
        Debug.Log("end");
        if (Event_img.gameObject.name == "Event01_img")
        {
            StartCoroutine(ExecuteAfterOneFrame());
            //Event_img.SetActive(false);
            //GameState.CloseExplain();

            //Debug.Log("aaa");
        }
    }

    IEnumerator ExecuteAfterOneFrame() // 実行を１フレーム遅らせる
    {
        yield return null; // 1フレーム待つ
        GameState.CloseExplain();
        Event_img.SetActive(false);
        /*
        if (mode == Mode.Mode_Next01)
        {
            next01();
        }
        if (mode == Mode.Mode_Next02)
        {
            next02();
        }
        if (mode == Mode.Mode_End)
        {
            end();
        }
        */
    }

}