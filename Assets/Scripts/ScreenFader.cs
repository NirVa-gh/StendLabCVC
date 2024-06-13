using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField]
    float fade_speed = 2f;
    private Animator _animatorScreen;

    IEnumerator Start()
    {
        _animatorScreen = GameObject.Find("FadeEffect").GetComponent<Animator>();
        Image fade_image = GetComponent<Image>();
        Color color = fade_image.color;

        while(color.a < 1f)
        {
            color.a += fade_speed * Time.deltaTime;
            fade_image.color = color;
            yield return null;
        }
    }

}
