using UnityEngine;
using UnityEngine.UI;

namespace Fading
{
    [RequireComponent(typeof(Image))]
    public class FaderPanel : Fader
    {
        private Image panelImage;
        [Range(0.0f, 1.0f)]
        private float panelAlpha;
        private Color panelColor;

        private void Awake()
        {
            panelImage = GetComponent<Image>();
            panelAlpha = panelImage.color.a;
            panelColor = panelImage.color;
        }

        public override void StartFadeInAnimation() => StartCoroutine(FadeIn(panelImage, panelAlpha, panelColor));
        public override void StartFadeOutAnimation() => StartCoroutine(FadeOut(panelImage, panelAlpha, panelColor));
    }
}