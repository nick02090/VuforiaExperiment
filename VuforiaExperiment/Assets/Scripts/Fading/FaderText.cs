using UnityEngine;
using UnityEngine.UI;

namespace Fading
{
    [RequireComponent(typeof(Text))]
    public class FaderText : Fader
    {
        private Text text;
        [Range(0.0f, 1.0f)]
        private float textAlpha;
        private Color textColor;

        private void Awake()
        {
            text = GetComponent<Text>();
            textAlpha = text.color.a;
            textColor = text.color;
        }

        public override void StartFadeInAnimation() => StartCoroutine(FadeIn(text, textAlpha, textColor));
        public override void StartFadeOutAnimation() => StartCoroutine(FadeOut(text, textAlpha, textColor));
    }
}