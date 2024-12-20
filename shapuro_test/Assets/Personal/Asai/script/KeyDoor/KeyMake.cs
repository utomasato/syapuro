using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyMake : MonoBehaviour
{
    public Image Keyimage;
    public GameObject Keycanvas;
    public GameState GameState;
    public Fire fire_script;
    public KeyDoor KeyDoor;
    public float CreateTime;

    private AudioSource SE;
    [SerializeField] private AudioClip KeyMakeSE;
    void Start()
    {
        GameObject Gamestate1 = GameObject.Find("Player");
        fire_script = Gamestate1.GetComponent<Fire>();
        GameObject Gamestate2 = GameObject.Find("GameState");
        GameState = Gamestate2.GetComponent<GameState>();
        SE = this.gameObject.AddComponent<AudioSource>();
        SE = this.gameObject.GetComponent<AudioSource>();

    }
    void OnTriggerStay(Collider other)
    {
        if (!GameState.JudgeState("Pause"))
        {
            if (other.gameObject.name == "Fire" && (Input.GetKey("left " + KeyBindings.DashKay) || Input.GetKey("right " + KeyBindings.DashKay) || Input.GetKey(KeyBindings.DashButton))/*Input.GetKey(KeyCode.Space)*/ && KeyDoor.JudgeMode("Mode_Stay") && fire_script.GetIsOnCandle())
            {
                Createkey(2);
            }
            else if (other.gameObject.name == "Fire" && KeyDoor.JudgeMode("Mode_Stay") && fire_script.GetIsOnCandle())
            {
                Createkey(1);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (!GameState.JudgeState("Pause"))
        {
            if (other.gameObject.name == "Fire" && KeyDoor.JudgeMode("Mode_Stay") && fire_script.GetIsOnCandle())
            {
                Keycanvas.gameObject.SetActive(false);
            }
        }
    }
    void Createkey(int test)
    {
        if (test == 2)
        {
            CreateTime += Time.deltaTime * 10.0f;
            Keyimage.fillAmount = CreateTime / 2.0f;
            Keycanvas.gameObject.SetActive(true);
        }
        else
        {
            CreateTime += Time.deltaTime;
            Keyimage.fillAmount = CreateTime / 2.0f;
            Keycanvas.gameObject.SetActive(true);
        }
        if (CreateTime > 2f)
        {
            KeyDoor.TransMode("Mode_First");
            Keycanvas.gameObject.SetActive(false);
        }
    }
    public void KeyMakeSE_Func()
    {
        SE.volume = 0.1f;
        SE.PlayOneShot(KeyMakeSE);
    }
}
