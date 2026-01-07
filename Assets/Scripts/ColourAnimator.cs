using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColourAnimator
{
    public static IEnumerator Animate(Graphic graphic, float min = 0.5f, float max = 1.0f, float increment = 0.1f, float delay = 0.05f)
    {
        Color c = graphic.color;
        int key = 0;
        float inc = increment;

        while (true)
        {
            switch (key)
            {
                case 0:
                    c.r += inc;
                    if (c.r < min || c.r > max)
                    {
                        c.r = Mathf.Clamp(c.r, min, max);
                        key = 1;
                    }
                    break;
                case 1:
                    c.g += inc;
                    if (c.g < min || c.g > max)
                    {
                        c.g = Mathf.Clamp(c.g, min, max);
                        key = 2;
                    }
                    break;
                case 2:
                    c.b += inc;
                    if (c.b < min || c.b > max)
                    {
                        c.b = Mathf.Clamp(c.b, min, max);
                        inc *= -1f;
                        key = 0;
                    }
                    break;
            }

            graphic.color = c;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
