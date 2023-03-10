using System.Collections;
using UnityEngine;

// Cartoon FX  - (c) 2015 Jean Moreno

// Automatically destructs an object when it has stopped emitting particles and when they have all disappeared from the screen.
// Check is performed every 0.5 seconds to not query the particle system's state every frame.
// (only deactivates the object if the OnlyDeactivate flag is set, automatically used with CFX Spawn System)

namespace Plugins.JMO_Assets.Cartoon_FX__legacy_.Scripts
{
    [RequireComponent(typeof(ParticleSystem))]
    public class CFX_AutoDestructShuriken : MonoBehaviour
    {
        // If true, deactivate the object instead of destroying it
        public bool OnlyDeactivate;

        private void OnEnable() => StartCoroutine(CheckIfAlive());

        private IEnumerator CheckIfAlive()
        {
            var ps = GetComponent<ParticleSystem>();

            while (ps != null)
            {
                yield return new WaitForSeconds(0.5f);

                if (ps.IsAlive(true)) continue;

                if (OnlyDeactivate)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    Destroy(gameObject);
                }

                break;
            }
        }
    }
}