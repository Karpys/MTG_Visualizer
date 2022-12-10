using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "ScriptableObjects/Card", order = 0)]
public class CardScriptable : ScriptableObject
{
    public Sprite m_CardVisual = null;
}