using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireWall : MonoBehaviour
{
    public GameObject fire;
    public Fire fire_script;
    public float time = 0;
    public bool FireStart = false;
    void Start()
    {
        GameObject Gamestate = GameObject.Find("Player");
        fire_script = Gamestate.GetComponent<Fire>();
    }
    void FixedUpdate()
    {
        if (FireStart == true && time <= 3)
        {
            time += Time.deltaTime;
            if (time >= 3)
            {
                this.transform.position = new Vector3(0f, -1000f, 0f);
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && FireStart == false && fire_script.GetIsOnCandle())
        {
            time += Time.deltaTime;
            if (time >= 1)
            {
                FireStart = true;
                fire.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}
