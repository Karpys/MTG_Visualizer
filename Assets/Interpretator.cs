using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpretator : MonoBehaviour
{
    // Start is called before the first frame update
    public List<CardScriptable> m_Cards = new List<CardScriptable>();
    public GameObject prefab = null;
    void Start()
    {
        foreach (CardScriptable cardScriptable in m_Cards)
        {
            Instantiate(prefab);
            prefab.GetComponent<SpriteRenderer>().sprite = cardScriptable.m_CardVisual;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
