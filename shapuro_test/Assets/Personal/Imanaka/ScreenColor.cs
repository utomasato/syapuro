using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenColor : MonoBehaviour
{

    [SerializeField]
    private GameObject GameOvercanvas;
    private GameManager gamemanager;//SetGameクラスのGameManagerクラスを使用
    // Start is called before the first frame update
    void Start()
    {
        gamemanager = new GameManager();
        GameOvercanvas.SetActive(false);
        // gamemanager.SetisGameOver(true); /*ここをコメント解除するとゲームオーバー画面になる*/
    }

    // Update is called once per frame
    void Update()
    {
        if (gamemanager.GetisGameOver())
        {
            GameOvercanvas.SetActive(true);
        }
    }
}
