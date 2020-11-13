using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public enum EffectStartMode
{
    RunOnStart,
    RunOnVisible
}
/// <summary>
/// UI 淡入淡出效果試做 2020.09 Swings
/// </summary>
public class UIFadeInOut : MonoBehaviour
{
    public bool FadeIn, FadeOut;

    public float FadeSpeed = 10.0f;
    public float HoldOnSecond = 2.0f;
    public EffectStartMode startMode = EffectStartMode.RunOnStart;

    public  UnityEvent MethodToNextScene;

    private Coroutine process = null;
    private UnityEngine.UI.Image _target;

    public void FinishFadeIn()
    {
        if (process!=null)
        {
            StopCoroutine(process);
            process = null;
        }
        FadeIn = false;

        // need fade out ?
        if (FadeOut)
        {
            process = StartCoroutine(FadeOutObject());
        }
    }
    protected void FinishFadeOut()
    {
        StopCoroutine(process);
        process = null;
        FadeOut = false;

        if(MethodToNextScene!= null)
        {
            MethodToNextScene.Invoke();
        }
    }

    void Start() //void OnBecameVisible()
    {
        _target = this.gameObject.GetComponent<UnityEngine.UI.Image>();

        if (startMode != EffectStartMode.RunOnStart)
        {
            return;
        }

        if (FadeIn)
        {
            InitTransparent();
            process = StartCoroutine(FadeInObject());
        }
    }
    void Update()
    {
        if (startMode != EffectStartMode.RunOnVisible)
        {
            return;
        }

        if(process!= null) // already in process
        {
            return;
        }

        if (FadeIn && _target.gameObject.activeInHierarchy)
        {
            InitTransparent();
            process = StartCoroutine(FadeInObject());
        }
    }

    private void InitTransparent()
    {
        _target.color = new Color(_target.color.r, _target.color.g, _target.color.b, 0.0f);
    }

    public IEnumerator FadeInObject()
    {
        while(FadeIn)
        {
            Color objectColor = _target.color;
            float fadeAmout = objectColor.a + (FadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmout);
            _target.color = objectColor;

            yield return null;

            if (fadeAmout >= 1.0f)
            {
                FinishFadeIn();
            }
        }
    }

    public IEnumerator FadeOutObject()
    {
        yield return new WaitForSeconds(HoldOnSecond);

        while (FadeOut)
        {
            Color objectColor = _target.color;
            float fadeAmout = objectColor.a - (FadeSpeed * Time.deltaTime * 2);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmout);
            _target.color = objectColor;

            yield return null;

            if (fadeAmout <= 0.1f)
            {
                FinishFadeOut();
            }
        }
    }
}
