using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FooterUI : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> CandleText;
    [SerializeField] TextMeshProUGUI jumpText;
    [SerializeField] TextMeshProUGUI dashText;
    [SerializeField] TextMeshProUGUI detachText;
    [SerializeField] List<TextMeshProUGUI> FireText;

    [SerializeField] private Color32 normalColor = new Color32(255, 255, 255, 255); // 通常の色
    [SerializeField] private Color32 grayColor = new Color32(128, 128, 128, 255); // グレー色

    private bool FireIsOnCandle = true;

    void Start()
    {
        //GrayOutInstructionTexts();
        SwitchInstructionTexts(true);

        if (KeyBindings.JumpKay == null)
            KeyBindings.LoadConfig();
        if (jumpText != null)
            jumpText.text = "ジャンプ : " + CapitalizeFirstLetter(KeyBindings.JumpKay);
        if (dashText != null)
            dashText.text = "ダッシュ : " + CapitalizeFirstLetter(KeyBindings.DashKay);
        if (detachText != null)
            detachText.text = "転生 : " + CapitalizeFirstLetter(KeyBindings.TransferKay);
    }

    public void SwitchInstructionTexts(bool isOnCandle)
    {
        FireIsOnCandle = isOnCandle;
        if (FireIsOnCandle)
        {
            foreach (TextMeshProUGUI text in FireText)
            {
                text.enabled = false;
            }
            foreach (TextMeshProUGUI text in CandleText)
            {
                text.enabled = true;
            }
        }
        else
        {
            foreach (TextMeshProUGUI text in CandleText)
            {
                text.enabled = false;
            }
            foreach (TextMeshProUGUI text in FireText)
            {
                text.enabled = true;
            }
        }
    }

    public void ActivateInstructionTexts()
    {
        foreach (TextMeshProUGUI text in CandleText)
        {
            text.color = normalColor;
        }
        foreach (TextMeshProUGUI text in FireText)
        {
            text.color = normalColor;
        }
    }

    public void GrayOutInstructionTexts()
    {
        foreach (TextMeshProUGUI text in CandleText)
        {
            text.color = grayColor;
        }
        foreach (TextMeshProUGUI text in FireText)
        {
            text.color = grayColor;
        }
    }

    public void SetJumpTextGrayOut(bool isGrayOut)
    {
        if (isGrayOut)
            jumpText.color = grayColor;
        else
            jumpText.color = normalColor;
    }

    private string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsDigit(input[0]))
        {
            return input;
        }
        input = input.Replace("right ", "R ");
        input = input.Replace("left ", "L ");
        return char.ToUpper(input[0]) + input.Substring(1);
    }
}
