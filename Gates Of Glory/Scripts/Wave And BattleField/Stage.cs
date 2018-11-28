using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Waves/Stage")]
public class Stage : ScriptableObject{
    public List<WaveManager.NewSoldier> soldiers = new List<WaveManager.NewSoldier>();
}
