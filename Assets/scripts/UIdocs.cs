using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIdocs : MonoBehaviour
{
    public UIDocument hudScreen;
    public UIDocument titleScreen;
    private VisualElement root;
    private GameManager manager;

    [HideInInspector]
    public bool isInTitle = false;

    public float titleTextCycle = 0;
    public float liveCycle = 0;

    private void Awake()
    {
        root = hudScreen.rootVisualElement;

        manager = GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        root.Q<Label>("point").text = manager.score + " score";
        root.Q<Label>("lvscore").text = "." + manager.lv;
    }

    private void Update()
    {
        if (isInTitle)
        {
            titleTextCycle += 20f * Time.unscaledDeltaTime;
            titleScreen.rootVisualElement.Q<VisualElement>("zerOpen").transform.position = new Vector3(0, titleTextCycle);

            if (titleTextCycle > 550) titleTextCycle = 0;

            liveCycle += 10f * Time.unscaledDeltaTime;

            if (liveCycle <= 40)
            {
                titleScreen.rootVisualElement.Q<VisualElement>("livePanel").transform.position = new Vector3(0, -liveCycle);
                titleScreen.rootVisualElement.Q<VisualElement>("player").transform.rotation = Quaternion.Euler(new Vector3(0, 0, 24 + (0.125f * liveCycle)));
                var enemy = titleScreen.rootVisualElement.Q<VisualElement>("enemy");

                enemy.transform.position = new Vector3(0, liveCycle * 2f);
                enemy.transform.scale = new Vector3(5 - (0.025f * liveCycle), 2.8f + (0.005f * liveCycle));
            } else
            {
                titleScreen.rootVisualElement.Q<VisualElement>("livePanel").transform.position = new Vector3(0, -80 + liveCycle);
                titleScreen.rootVisualElement.Q<VisualElement>("player").transform.rotation = Quaternion.Euler(new Vector3(0, 0, 34 - (0.125f * liveCycle)));
                var enemy = titleScreen.rootVisualElement.Q<VisualElement>("enemy");

                enemy.transform.scale = new Vector3(3 + (0.025f * liveCycle), 3.2f - (0.005f * liveCycle));
                enemy.transform.position = new Vector3(0, 160 - (liveCycle * 2f));
            }

            if (liveCycle > 80) liveCycle = 0;
        }
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

    public void ShowTitle()
    {
        StartCoroutine(_showTitle());
    }

    IEnumerator _showTitle()
    {
        yield return new WaitForSecondsRealtime(1f);

        isInTitle = true;
        titleScreen.gameObject.SetActive(true);

        root.Q<VisualElement>("top_hud").RemoveFromClassList("top_hud_in");

        yield return new WaitForSecondsRealtime(0.2f);

        manager.soundManager.Play("effect.cut");

        titleScreen.rootVisualElement.Q<VisualElement>("blindTop").AddToClassList("blindTop_in");
        titleScreen.rootVisualElement.Q<VisualElement>("blindBottom").AddToClassList("blindBottom_in");

        yield return new WaitForSecondsRealtime(1f);

        titleScreen.rootVisualElement.Q<VisualElement>("player").transform.position = Vector3.zero;
        titleScreen.rootVisualElement.Q<VisualElement>("fadePanel").AddToClassList("fadePanel_in");

        titleScreen.rootVisualElement.Q<Button>("startButton").RegisterCallback<ClickEvent>(StartButton);

        for (int i = 0; i < manager.enemies.Count; i++)
        {
            var e = manager.enemies[i];

            manager.enemies.Remove(e);

            Destroy(e.gameObject);
        }

        int best = 0;
        if (PlayerPrefs.HasKey("bestScore")) best = PlayerPrefs.GetInt("bestScore");

        titleScreen.rootVisualElement.Q<Label>("best").text = "BEST: " + best.ToString() + " score";
        titleScreen.rootVisualElement.Q<Label>("last").text = "LAST: " + manager.lastScore + " score";
    }

    void StartButton(ClickEvent ev)
    {
        if (!isInTitle) return;

        StartCoroutine(_startButton());
    }

    IEnumerator _startButton()
    {
        isInTitle = false;
        for (int i = 0; i <= 10; i++)
        {
            titleScreen.rootVisualElement.Q<VisualElement>("enemy").transform.scale = new Vector3(5 - (0.5f * i), 2.8f - (0.28f * i));
            yield return new WaitForSecondsRealtime(0.03f);
        }

        yield return new WaitForSecondsRealtime(0.3f);

        titleScreen.rootVisualElement.Q<VisualElement>("fadePanel").RemoveFromClassList("fadePanel_in");

        manager.soundManager.Play("effect.dong");
        for (int i = 0; i <= 20; i++)
        {
            titleScreen.rootVisualElement.Q<VisualElement>("player").transform.position = new Vector3(-50 * i, 50 * i);
            yield return new WaitForSecondsRealtime(0.015f);
        }

        yield return new WaitForSecondsRealtime(0.4f);

        titleScreen.rootVisualElement.Q<VisualElement>("blindTop").RemoveFromClassList("blindTop_in");
        titleScreen.rootVisualElement.Q<VisualElement>("blindBottom").RemoveFromClassList("blindBottom_in");

        yield return new WaitForSecondsRealtime(0.4f);

        manager.GameStart();
        root.Q<VisualElement>("top_hud").AddToClassList("top_hud_in");

        titleScreen.gameObject.SetActive(false);
    }
}
