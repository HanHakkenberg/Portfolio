using UnityEngine;

[CreateAssetMenu(menuName ="Data/General Stats")]
public class GeneralStats : ScriptableObject 
{

    [Header("General Stats")]
    public string myName;
    [Space(10)]
    public int myLevel = 1;
    public int myMaxLevel;
    [Space(10)]
    public int myBuildCost;
    public Stat myUpgradeCost;
}
