using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIdocs : MonoBehaviour
{
    private VisualElement root;
    private GameManager manager;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        root.Q<Label>("point").text = "Á¡¼ö: " + manager.score;
        root.Q<Label>("lvscore").text = "." + manager.lv;
    }
}
