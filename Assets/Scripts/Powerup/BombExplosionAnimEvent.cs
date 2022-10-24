using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionAnimEvent : MonoBehaviour
{
    [SerializeField] private Bomb bomb;

    public void Explode()
    {
        bomb.FillAffectedCells();
        
    }
}
