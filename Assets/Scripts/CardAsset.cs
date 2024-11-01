using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SectorCardAsset", order = 1)]
public class SectorCardAsset : ScriptableObject
{

    public string prefabName;

    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;


    public string cardTitle;
    public string cardDescription;
    public Sprite cardIcon;
    public int revenueIncrease;
    public int minimumCost;
    public int numberOfInfluence;
}