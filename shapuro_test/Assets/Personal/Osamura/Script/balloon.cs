using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloon : MonoBehaviour
{
    public GameObject player;
    private bool isRise = false;
    private float t;
    private Vector3 rise;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        t = Time.deltaTime;
        if (t >= 0.1)
        {
            t = 0F;
            if (isRise)
            {
                rise = this.gameObject.transform.localPosition;
                this.gameObject.transform.localPosition = new Vector3(rise.x, rise.y + 0.2F, rise.z);
                rise = player.transform.localPosition;
                player.transform.localPosition = new Vector3(rise.x, rise.y + 0.2F, rise.z);

            }
        }
    }
    /*    void OnTriggerEnter(Collision other)
        {
            isRise = (other.gameObject.Equals(player)/*&&player.GetComponent<player>().isBoost);
        }*/
}
