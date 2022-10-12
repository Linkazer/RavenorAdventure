using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CPN_Data_HealthHandler
{
    public int MaxHealth();
    public int MaxArmor();

    public int Defense();
    public int DefensiveRerolls();
}
