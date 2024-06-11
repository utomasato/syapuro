/*
蝋燭の当たり判定部分
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_sub_test : MonoBehaviour
{
    [SerializeField] private Fire_test parent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (parent.GetIsOnCandle() && other.gameObject.CompareTag("lamp")) // 憑依状態でランプに当たったら
        {
            other.gameObject.GetComponent<Lamp_test>().Ignition();
        }

        if (!parent.GetIsOnCandle() && other.gameObject.CompareTag("FirePosition")) // 火の玉状態で蝋燭に当たったら
        {
            parent.Transfer(other.gameObject.GetComponent<FirePosition_test>());
        }
    }
}
