using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisplayer : MonoBehaviour
{
    [SerializeField] private ParticleSystemRenderer particle;
    public int offset = 0;

    /// <summary>
    /// Met à jour l'OrderInLayer de la particule.
    /// </summary>
    private void OnEnable()
    {
        particle.sortingOrder = -Mathf.RoundToInt(transform.position.y * 5) + offset; ;
    }
}
