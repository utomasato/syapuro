using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Wall : MonoBehaviour
{
    public GameObject Fs;
    public bool isFire = true;
    private int i = 0;
    private int j = 0;
    private Vector3 Flocate,Fscale;
    private float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        Flocate = this.gameObject.transform.localPosition;
        Fscale = this.gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        j = i;
        if(t>0.01){
            t = 0;
            if (!isFire && i < 100) i++;
            if (isFire && i > 0) i--;
            if (i != j)
            {
                gameObject.transform.localScale = new Vector3(Fscale.x, Fscale.y - 0.01F * i, Fscale.z);
                gameObject.transform.localPosition = new Vector3(Flocate.x, Flocate.y - 0.005F * i, Flocate.z);
            }
        }
        if(Fs!=null){
            isFire = !Fs.transform.Find("Button").gameObject.GetComponent<Fire_Switch>().isPushed;
        }
    }
}
