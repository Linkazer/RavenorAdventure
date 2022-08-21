using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisplayer : SpriteDisplayer
{
    private ParticleSystemRenderer particle;
    [SerializeField] private Texture particleSprite;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (particleSprite != null)
        {
            SetParticleTexture();
        }
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
