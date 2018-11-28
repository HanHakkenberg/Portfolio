using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Waves/Wave")]
public class Wave : ScriptableObject{
    public List<Stage> atackStage = new List<Stage>();
}
