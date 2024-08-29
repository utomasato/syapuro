using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeySetting : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dash;
    [SerializeField] private TextMeshProUGUI jump;
    [SerializeField] private TextMeshProUGUI detach;

    public void Commit()
    {
        KeyBindings.keys.dashKay = dash.text;
        KeyBindings.keys.jumpKay = jump.text;
        KeyBindings.keys.transferKay = detach.text;
        KeyBindings.SaveConfig();
    }
}
