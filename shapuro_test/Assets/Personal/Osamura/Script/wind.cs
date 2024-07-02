using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{
    public GameObject player;
    //   private bool isBoost = false;
    // Start is called before the first frame update
    bool inWind = true;
    private void OnTriggerEnter(Collider other)
    {
        if (inWind && other.gameObject.Equals(player) &&
        !player.GetComponent<Fire_sub_test>().getParent().IsBigFire)
        {
            Debug.Log("gameover");
            inWind = false;
            player.GetComponent<Fire_sub_test>().getParent().inWind();
        }
    }
}
