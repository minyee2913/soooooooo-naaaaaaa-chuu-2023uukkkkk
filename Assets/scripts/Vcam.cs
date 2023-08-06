using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vcam : MonoBehaviour
{
    public CinemachineVirtualCamera virCam;
    public GameObject slowEffect;

    int dutchFace = 1;
    IEnumerator task = null;
    void Start()
    {
        virCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void SlowOn()
    {
        if (task != null)
        {
            StopCoroutine(task);
        }

        task = _slowOn();
        StartCoroutine(task);
    }

    IEnumerator _slowOn()
    {
        if (Random.Range(0f, 1f) > 0.5f)
        {
            dutchFace = 1;
        } else
        {
            dutchFace = -1;
        }

        slowEffect.SetActive(true);
        for (int i = 1; i <= 20; i++)
        {
            virCam.m_Lens.OrthographicSize = 5.3f - (0.02f * i);
            virCam.m_Lens.Dutch = i * 0.35f * dutchFace;

            yield return new WaitForSecondsRealtime(0.01f);
        }

        task = null;
    }

    public void SlowOut()
    {
        if (task != null)
        {
            StopCoroutine(task);
        }

        task = _slowOut();
        StartCoroutine(task);
    }

    IEnumerator _slowOut()
    {
        slowEffect.SetActive(false);
        for (int i = 1; i <= 20; i++)
        {
            virCam.m_Lens.OrthographicSize = 4.9f + (0.02f * i);
            virCam.m_Lens.Dutch = (7 - (i * 0.35f)) * dutchFace;

            yield return new WaitForSecondsRealtime(0.005f);
        }

        task = null;
    }
}
