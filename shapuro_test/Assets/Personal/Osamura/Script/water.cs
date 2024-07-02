using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water : MonoBehaviour
{
    public GameObject player;
    //   private bool isBoost = false;
    // Start is called before the first frame update
    bool inWater = true;
    private void OnTriggerEnter(Collider other)
    {
        if (inWater && other.gameObject.Equals(player))
        {
            Debug.Log("gameover");
            inWater = false;
            player.GetComponent<Fire_sub_test>().getParent().inWater();
        }
    }
}
