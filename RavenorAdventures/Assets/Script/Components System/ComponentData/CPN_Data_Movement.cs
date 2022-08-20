using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CPN_Data_Movement
{
    /// <summary>
    /// La vitesse du d�pacement.
    /// </summary>
    /// <returns></returns>
    public float Speed();

    /// <summary>
    /// La distance qui peut �tre parcourut en un seul tour.
    /// </summary>
    /// <returns></returns>
    public int MaxDistance();
}
