using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public bool isFadeIn; // true=FadeIn, false=FadeOut
    public GameObject panel; // �������� ������ Panel ������Ʈ

    void Start()
    {
        if (!panel)
        {
            Debug.LogError("Panel ������Ʈ�� ã�� �� �����ϴ�.");
            throw new MissingComponentException();
        }

        if (isFadeIn) // Fade In Mode -> �ٷ� �ڷ�ƾ ����
        {
            panel.SetActive(true); // Panel Ȱ��ȭ
            StartCoroutine(CoFadeIn(0));
        }
        else
        {
            panel.SetActive(false); // Panel ��Ȱ��ȭ
        }
    }

    public void FadeIn(float time)
    {
        StartCoroutine(CoFadeIn(time));
    }

    public void FadeOut(float time, Action action = null)
    {
        panel.SetActive(true); // Panel Ȱ��ȭ
        Debug.Log("FadeCanvasController_ Fade Out ����");
        StartCoroutine(CoFadeOut(time, action));
        Debug.Log("FadeCanvasController_ Fade Out ��");
    }

    IEnumerator CoFadeIn(float time)
    {
        yield return new WaitForSeconds(time);

        float elapsedTime = 0f; // ���� ��� �ð�
        float fadedTime = 0.5f; // �� �ҿ� �ð�

        while (elapsedTime <= fadedTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(1f, 0f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            Debug.Log("Fade In ��...");
            yield return null;
        }
        Debug.Log("Fade In ��");
        panel.SetActive(false); // Panel�� ��Ȱ��ȭ
        yield break;
    }

    IEnumerator CoFadeOut(float time, Action action = null)
    {
        yield return new WaitForSeconds(time);

        float elapsedTime = 0f; // ���� ��� �ð�
        float fadedTime = 0.5f; // �� �ҿ� �ð�

        while (elapsedTime <= fadedTime)
        {
            panel.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(0f, 2f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            Debug.Log("Fade Out ��...");
            yield return null;
        }

        Debug.Log("Fade Out ��");
        action?.Invoke(); // ���Ŀ� �ؾ� �ϴ� �ٸ� �׼��� �ִ� ���(null�� �ƴ�) �����Ѵ�
        StartCoroutine(CoFadeIn(0.5f));
        yield break;
    }
}
