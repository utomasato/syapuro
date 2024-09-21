using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event_text : MonoBehaviour
{
    public string test;
    public Text C_text;
    public enum Test
    {
        Mode_Jump,
        Mode_Dash,
        Mode_Trans,
        Mode_Controller
    }
    public Test test_test;
    // Start is called before the first frame update
    void Start()
    {
        KeyBindings.LoadConfig();
        if (test_test == Test.Mode_Jump)
        {
            C_text.text = test + testtesttest(KeyBindings.JumpKay);
        }
        if (test_test == Test.Mode_Dash)
        {
            C_text.text = test + testtesttest(KeyBindings.DashKay);
        }
        if (test_test == Test.Mode_Trans)
        {
            C_text.text = test + testtesttest(KeyBindings.TransferKay);
        }
        if (test_test == Test.Mode_Controller)
        {
            //C_text.text = test + "           ";
        }

        //C_text.text = test + " : " + KeyBindings.JumpKay + KeyBindings.DashKay + KeyBindings.TransferKay;
        //C_text.text = test + " : " + testtesttest(KeyBindings.JumpKay) + testtesttest(KeyBindings.DashKay) + testtesttest(KeyBindings.TransferKay);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private string testtesttest(string input)
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
