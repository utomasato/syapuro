using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Event : MonoBehaviour
{
    public bool used = false;
    public Fire fire_script;
    public GameState GameState;
    public GameObject Event_img;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("Player");
        fire_script = Gamestate.GetComponent<Fire>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && used == false)
        {
            Event_img.SetActive(true);
            GameState.OpenExplain();
            used = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(ExecuteAfterOneFrame());
        }
    }
    // void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.name == "Fire")
    //     {
    //         Event_img.SetActive(false);
    //     }
    // }
    public void next()
    {
        Debug.Log("aaa");
        if (Event_img.gameObject.name == "Event01_img")
        {
            Event_img.SetActive(false);
            GameState.CloseExplain();
            //Debug.Log("aaa");
        }
    }

    IEnumerator ExecuteAfterOneFrame() // 実行を１フレーム遅らせる
    {
        yield return null; // 1フレーム待つ
        next();
    }

}