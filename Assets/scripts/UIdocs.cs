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
        root.Q<Label>("point").text = manager.score + " score";
        root.Q<Label>("lvscore").text = "." + manager.lv;
    }

    public void LvUp()
    {
        StartCoroutine(lvUp());
    }

    IEnumerator lvUp()
    {
        for (float i = 1; i < 1.1; i+=0.01f)
        {
            root.Q<Label>("lvscore").transform.scale = new Vector2 (i, i);

            yield return new WaitForSeconds(0.015f);
        }

        for (float i = 1.1f; i >= 1; i -= 0.01f)
        {
            root.Q<Label>("lvscore").transform.scale = new Vector2(i, i);

            yield return new WaitForSeconds(0.015f);
        }
    }
}
