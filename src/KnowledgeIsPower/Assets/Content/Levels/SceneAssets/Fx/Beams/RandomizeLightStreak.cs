using UnityEngine;

namespace SceneAssets.Fx.Beams
{
    public class RandomizeLightStreak : MonoBehaviour
    {
        public Vector2 streakWidthRange = new Vector2(2, 2);

        private void Awake()
        {
            var lr = GetComponent<LineRenderer>();
            var l = GetComponent<Light>();
            float r = Random.Range(streakWidthRange.x, streakWidthRange.y);

            if (lr != null)
            {
                lr.startWidth = r;
                lr.endWidth = r;
            }

            if (l != null && l.type == LightType.Spot)
            {
                l.spotAngle = r * 3;
            }
        }
    }
}