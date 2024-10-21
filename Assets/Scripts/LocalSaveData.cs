using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalSaveData
{
    [SerializeField]
    private int intValue = 123;
    [SerializeField]
    private float floatValue = 456.7f;
    [SerializeField]
    private string stringValue = "hello world";

    // public으로 읽어올 수 있어야 json직렬화가 가능
    public int IntValue { get => intValue; }
    public float FloatValue { get => floatValue; }
    public string StringValue { get => stringValue; }
}
