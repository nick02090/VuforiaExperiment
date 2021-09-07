using UnityEngine;
using UnityEngine.UI;

namespace Fading
{
    [RequireComponent(typeof(Text))]
    public class FaderText : Fader
    {
        private Text text;

        private void Awake()
        {
            text = GetComponent<Text>();
            Alpha = text.color.a;
            Color = text.color;
        }

        public override void StartFadeInAnimation() => StartCoroutine(FadeIn(text));
        public override void StartFadeOutAnimation() => StartCoroutine(FadeOut(text));
    }
}