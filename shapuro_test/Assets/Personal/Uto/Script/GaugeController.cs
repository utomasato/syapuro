using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GaugeController : MonoBehaviour
{
    [SerializeField] private Fire player;
    [SerializeField] private GameObject candleGauge;
    private Image candleImg;
    [SerializeField] private Color32 normalColor = new Color32(255, 255, 255, 255); // 通常の色
    [SerializeField] private Color32 grayColor = new Color32(128, 128, 128, 255); // グレー色

    [SerializeField] private GameObject fireGauge;
    private Vector2 fireStartPosition;
    [SerializeField] private Vector3 fireEndPosition;
    private Image fireImg;

    RectTransform fireRectTransform;
    [SerializeField] private TextMeshProUGUI Counter;
    [SerializeField] private TextMeshProUGUI Mode;
    [SerializeField] private string[] sl = { "easy", "hard" };


    void Start()
    {
        candleImg = candleGauge.GetComponent<Image>();
        fireImg = fireGauge.GetComponent<Image>();
        fireRectTransform = fireGauge.GetComponent<RectTransform>();
        fireStartPosition = fireRectTransform.anchoredPosition;
        Mode.text = sl[SceneSelectionState.mode];
    }

    public void UpdateCandleGauge(float life)
    {
        candleImg.fillAmount = (life / 2.0f) * (life / 2.0f);
        fireRectTransform.anchoredPosition = Vector3.Lerp(fireEndPosition, fireStartPosition, (life / 2.0f) * (life / 2.0f));
    }

    public void UpdateFireGauge(float life)
    {
        fireImg.fillAmount = life;
    }

    public void FillFireGauge()
    {
        if (fireImg == null)
            fireImg = fireGauge.GetComponent<Image>();
        fireImg.fillAmount = 1.0f;
    }

    public void SetCandleGaugeGrayOut(bool isGrayOut)
    {
        if (isGrayOut)
        {
            candleImg.color = grayColor; // Fill部分の色をグレーに設定
        }
        else
        {
            if (candleImg == null)
                candleImg = candleGauge.GetComponent<Image>();
            candleImg.color = normalColor; // Fill部分の色を通常に設定
        }
    }

    public void UpdateCount(int lampCount)
    {
        Counter.text = lampCount.ToString();
    }
}
