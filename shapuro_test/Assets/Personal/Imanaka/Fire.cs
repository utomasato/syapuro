using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private GameObject PL;
    [SerializeField]
    private GameObject Candle;
    [SerializeField]
    private float speed;

    private Vector3 SaveScale;

    private Vector3 CurrentScale;
    // Start is called before the first frame update
    void Start()
    {
        PL.transform.localScale = new Vector3(1, 1, 1);

        SaveScale = PL.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        //  PL.transform.localScale = Candle.transform.localScale;

        PressKey();
        SmallFire(Candle, 0.5f);
    }

    void PressKey()
    {
        Rigidbody rb;
        rb = GetComponent<Rigidbody>();
        CurrentScale = PL.transform.localScale;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float BigSize = ChangeSize(CurrentScale.y, 2.0f);
            Debug.Log(BigSize);
            CurrentScale.y = BigSize;
            PL.transform.localScale = CurrentScale;
        }
        if (Input.GetKey(KeyCode.D))
        {

            rb.AddForce(new Vector3(speed, 0, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {

            rb.AddForce(new Vector3(-speed, 0, 0) * Time.deltaTime);
        }
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {

            rb.velocity = Vector3.zero;
        }
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {

            rb.velocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.W))
        {

        }

    }

    float ChangeSize(float currentScale, float s)
    {
        return currentScale * s;
    }


    void SmallFire(GameObject target, float Size)
    {
        float y = target.transform.localScale.y;
        Vector3 targetscale = target.transform.localScale;
        if (y < 1.0f)
        {
            PL.transform.localScale = new Vector3(targetscale.x + Size, targetscale.y + Size, targetscale.z + Size);

        }
    }
}
