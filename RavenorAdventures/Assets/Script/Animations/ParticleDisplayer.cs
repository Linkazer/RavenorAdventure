using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisplayer : MonoBehaviour
{
    [SerializeField] private ParticleSystemRenderer particle;
    public int offset = 0;

    private void OnEnable()
    {
        particle.sortingOrder = -Mathf.RoundToInt(transform.position.y * 5) + offset; ;
    }
}
