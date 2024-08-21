using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Switch : MonoBehaviour
{
    public bool isPushed = false;
    private int locate = 0;
    private Vector3 y1;
    private float t = 0;
    [SerializeField] private FlameWall flame;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t > 0.02)
        {
            if (isPushed && locate < 10)
            {
                y1 = this.gameObject.transform.localPosition;
                gameObject.transform.localPosition = new Vector3(y1.x, y1.y - 0.015F, y1.z);
                locate++;
            }
            t = 0;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("candle"))
        {
            Debug.Log("ok");
            isPushed = true;
            if (flame != null)
            {
                flame.DyingOut();
            }
        }
    }
}
