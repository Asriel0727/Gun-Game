using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoadFade : MonoBehaviour
{
    static public void FadeAndLoadLevel(int level, Color color, float fadeLength)
    {
        var canvas = (RectTransform)GameObject.Find("Canvas").transform;
        var fade = (RectTransform)new GameObject("Fade").AddComponent<Image>().transform;
        fade.SetParent(canvas);
        fade.SetAsLastSibling();
        fade.anchoredPosition = Vector2.zero;
        fade.sizeDelta = canvas.sizeDelta;
        fade.gameObject.AddComponent<LevelLoadFade>().DoFade(level, fadeLength);
    }

    public void DoFade(int level, float fadeLength)
    {
        StartCoroutine(IEDoFade(level, fadeLength));
    }

    public IEnumerator IEDoFade(int level, float fadeLength)
    {
        // Dont destroy the fade game object during level load
        DontDestroyOnLoad(gameObject);

        Image img = GetComponent<Image>();

        // Fadeout to start with
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);

        // Fade texture in
        var time = 0.0f;

        while (time < fadeLength)
        {
            time += Time.deltaTime;
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.InverseLerp(0.0f, fadeLength, time));

            yield return null;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);

        yield return null; ;

        // Complete the fade out (Load a level or reset player position)
        SceneManager.LoadScene(level);

        // Fade texture out
        time = 0.0f;

        while (time < fadeLength)
        {
            time += Time.deltaTime;
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.InverseLerp(fadeLength, 0.0f, time));

            yield return null;
        }

        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);

        yield return null;

        Destroy(gameObject);
    }
}
