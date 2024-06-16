using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderLength : MonoBehaviour
{
    public GameObject player;
    public GameObject test;
    private Slider slider;



    private float t;
    private float max;
    private float len;


    // Start is called before the first frame update
    void Start()
    {
        slider = this.gameObject.GetComponent<Slider>();
        max = player.GetComponent<Fire_test>().candle.life;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t > 0.1)
        {
            t = 0;
            slider.value = player.GetComponent<Fire_test>().candle.life / max;
            if (slider.value <= 0.01)
            {
                slider.value = 0F;
                test.gameObject.GetComponent<Image>().enabled = false;
            }
        }
    }
}