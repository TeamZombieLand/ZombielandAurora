using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {

    public DAMAGEBODY damage_body { get; set; }
    public void TakeDamage(float damage,bool iAmKiller = true);
    
}

public enum DAMAGEBODY
{
    STATIC,BODY,GRASS,GROUND
}
