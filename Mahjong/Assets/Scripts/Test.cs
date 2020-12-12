using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private TextAsset text;
    [SerializeField] private Text testText;

    private void Start()
    {
        var oneLineString =  text.text.Replace(Environment.NewLine, "");
        var clearString = oneLineString.Replace(" ", "");
        testText.text = clearString;
        Debug.Log(clearString.Length);
    }
}
