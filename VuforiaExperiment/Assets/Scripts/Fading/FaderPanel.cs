using UnityEngine;
using UnityEngine.UI;

namespace Fading
{
    [RequireComponent(typeof(Image))]
    public class FaderPanel : Fader
    {
        private Image panelImage;

        private void Awake()
        {
            panelImage = GetComponent<Image>();
            Alpha = panelImage.color.a;
            Color = panelImage.color;
        }

        public override void StartFadeInAnimation() => StartCoroutine(FadeIn(panelImage));
        public override void StartFadeOutAnimation() => StartCoroutine(FadeOut(panelImage));
    }
}