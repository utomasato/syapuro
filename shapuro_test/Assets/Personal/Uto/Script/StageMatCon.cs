using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMatCon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Material mat = this.GetComponent<Renderer>().material;
        float x = this.transform.lossyScale.x;
        float y = this.transform.lossyScale.y;
        mat.mainTextureScale = new Vector2(x, y);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
