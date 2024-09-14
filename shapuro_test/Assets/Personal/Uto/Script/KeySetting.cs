using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeySetting : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dashDropdown;
    [SerializeField] TMP_Dropdown jampDropdown;
    [SerializeField] TMP_Dropdown detachDropdown;
    [SerializeField] List<Image> imgs;
    [SerializeField] private Color32 normalColor = new Color32(255, 255, 255, 255); // 通常の色
    [SerializeField] private Color32 warningColor = new Color32(255, 255, 0, 255); // 警告の色

    [SerializeField]
    List<string> keyNames = new List<string>()
    {
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
        "shift", "left shift", "right shift",  "ctrl" ,"left ctrl", "right ctrl",  "alt", "left alt", "right alt",  "cmd", "left cmd", "right cmd",
        "space", "return", /*"escape",*/ "tab",
    };

    [SerializeField] private TextMeshProUGUI dash;
    [SerializeField] private TextMeshProUGUI jump;
    [SerializeField] private TextMeshProUGUI detach;

    [SerializeField] private StageSelect stageSelect;

    void Start()
    {
        KeyBindings.LoadConfig();
        dashDropdown.ClearOptions();
        dashDropdown.AddOptions(keyNames);
        dashDropdown.value = keyNames.IndexOf(KeyBindings.DashKay);

        jampDropdown.ClearOptions();
        jampDropdown.AddOptions(keyNames);
        jampDropdown.value = keyNames.IndexOf(KeyBindings.JumpKay);

        detachDropdown.ClearOptions();
        detachDropdown.AddOptions(keyNames);
        detachDropdown.value = keyNames.IndexOf(KeyBindings.TransferKay);
    }

    public void Commit()
    {
        KeyBindings.ResetConfig();
        KeyBindings.keys.dashKay = dash.text;
        KeyBindings.keys.jumpKay = jump.text;
        KeyBindings.keys.transferKay = detach.text;
        KeyBindings.SaveConfig();
        stageSelect.CloseMenu();
    }

    public void Reset()
    {
        dashDropdown.value = keyNames.IndexOf("shift");
        jampDropdown.value = keyNames.IndexOf("space");
        detachDropdown.value = keyNames.IndexOf("return");
    }

    public void CheckValues()
    {
        int[] values = { dashDropdown.value, jampDropdown.value, detachDropdown.value, keyNames.IndexOf("w"), keyNames.IndexOf("a"), keyNames.IndexOf("s"), keyNames.IndexOf("d") };
        foreach (Image img in imgs)
        {
            img.color = normalColor;
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = i + 1; j < values.Length; j++)
            {
                if (values[i] == values[j])
                {
                    if (i < 3)
                        imgs[i].color = warningColor;
                    if (j < 3)
                        imgs[j].color = warningColor;
                }
            }
        }
    }
}
