using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public Dialogue Variant1Dialogue;
    public Dialogue Variant2Dialogue;
    public Dialogue Variant3Dialogue;
    public bool ReturnDisabled;

    public string Variant1TextLocalized;
    public string Variant2TextLocalized;
    public string Variant3TextLocalized;
    public string OutcomeString;
}