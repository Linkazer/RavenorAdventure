using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisplayer : MonoBehaviour
{
    [SerializeField] protected Renderer rnd;

    protected Material mat;

    private ParticleSystemRenderer particle;
    [SerializeField] private Texture particleSprite;

    protected void OnEnable()
    {
        if (rnd == null)
        {
            rnd = GetComponent<Renderer>();
            if (rnd == null)
            {
                enabled = false;
                return;
            }
        }

        mat = rnd.material;
        rnd.material = Instantiate(mat);

        if (particleSprite != null)
        {
            SetParticleTexture();
        }
    }

    private void Update()
    {
        rnd.sortingOrder = -Mathf.RoundToInt(transform.position.y * 5);
    }

    private void SetParticleTexture()
    {
        if (rnd is ParticleSystemRenderer)
        {
            particle = rnd as ParticleSystemRenderer;

            particle.material.SetTexture("_MainTex", particleSprite);
        }
    }
}
