using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycreate : MonoBehaviour
{
    public GameObject lock_image;
    public Fire fire_script;
    public RockedDoor RockedDoor;
    public float CreateTime;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("Player");
        fire_script = Gamestate.GetComponent<Fire>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && Input.GetKey(KeyCode.Space) && RockedDoor.Rock == false && fire_script.GetIsCandle())
        {
            Createkey();
        }
    }
    void Createkey()
    {
        CreateTime += Time.deltaTime;
        if (CreateTime > 2f)
        {
            RockedDoor.Rock = true;
            lock_image.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (CreateTime > 1.8f)
        {
            //RockedDoor.Rock = true;
        }
        else if (CreateTime > 1.2f)
        {
            //RockedDoor.Rock = true;
        }
        else if (CreateTime > 0.6f)
        {
            //RockedDoor.Rock = true;
        }
    }
}
