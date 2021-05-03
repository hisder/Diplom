using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldValidator
{
    public static void ApplyFullValidation(List<TMP_InputField> inputFields)
    {
        foreach (var inputField in inputFields)
        {
            inputField.onValidateInput += delegate (string text, int charIndex, char addedChar) { return InputValidate(addedChar, inputField); };
            inputField.onEndEdit.AddListener(delegate { ResultValidate(inputField); });
        }
    }

    public static char InputValidate(char addedChar, TMP_InputField inputField)
    {
        int addedCharValue;
        bool isDotInInputString = false;

        if (addedChar == ',')
        {
            foreach (var letter in inputField.text)
            {
                isDotInInputString = letter == ',';

                if (isDotInInputString)
                {
                    return '\0';
                }
            }

            return addedChar;
        }

        if (int.TryParse(addedChar.ToString(), out addedCharValue))
        {
            return addedChar;
        }
        else
        {
            return '\0';
        }
    }

    public static void ResultValidate(TMP_InputField inputField)
    {
        float finalValue;

        if (inputField.text == "")
        {
            inputField.text = "0";
            return;
        }

        if (inputField.text[0] == ',')
        {
            inputField.text = $"{0}{inputField.text}";
        }

        if (float.TryParse(inputField.text, out finalValue))
        {
            inputField.text = finalValue > 1 ? "1" : inputField.text;
        }
    }
}
