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
                Debug.Log("OK");
                rise = this.gameObject.transform.localPosition;
                this.gameObject.transform.localPosition = new Vector3(rise.x, rise.y + 0.2F, rise.z);
                rise = player.transform.localPosition;
                player.transform.localPosition = new Vector3(rise.x, rise.y + 0.2F, rise.z);

            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("何かにぶつかった");
        if (other.gameObject.Equals(player))
        {
            Debug.Log("プレイヤー");
            isRise = true;
        }
    }
}
