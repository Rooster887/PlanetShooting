using System.Collections;
using UnityEngine;

namespace J8N9.PlanetShooting
{
    public class FadeInBlock : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;

        // フェードイン開始
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            float alphaVal = spriteRenderer.color.a;
            Color tmp = spriteRenderer.color;

            while (spriteRenderer.color.a < 1)
            {
                alphaVal += 0.1f;
                tmp.a = alphaVal;
                spriteRenderer.color = tmp;

                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}