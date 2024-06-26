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

    private bool IsBigFire;//炎の威力を強めているか
    // Start is called before the first frame update
    void Start()
    {
        PL.transform.localScale = new Vector3(1, 1, 1);

        SaveScale = PL.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {


        PressKey();
        if (!IsBigFire)
        {
            SmallFire(Candle, 0.5f);
        }


    }

    void PressKey()
    {
        Rigidbody rb;
        rb = GetComponent<Rigidbody>();
        CurrentScale = PL.transform.localScale;

        if (Input.GetKeyDown(KeyCode.Space))
        {

            IsBigFire = true;


            BigFire(Candle);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsBigFire = false;
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

    void BigFire(GameObject Target)//何をもとにして炎の大きさを強めるか
    {
        Vector3 TargScale = Target.transform.localScale;
        float Size = ChangeSize(TargScale.y, 1.2f);
        TargScale.y = Size;
        PL.transform.localScale = TargScale;
    }
    float ChangeSize(float currentScale, float s)
    {
        return currentScale * s;
    }


    void SmallFire(GameObject target, float Size)
    {
        float y = target.transform.localScale.y;
        Vector3 targetscale = target.transform.localScale;

        PL.transform.localScale = targetscale;


    }
}
