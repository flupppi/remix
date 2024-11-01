using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EventCardAsset", order = 1)]
public class EventCardAsset : ScriptableObject
{

    public string prefabName;

    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;


    public string cardTitle;
    public string cardDescription;
    public Sprite cardIcon;
    public float revenueEffect;
    public float minimumCostEffect;
    public float numberOfInfluenceEffect;
    public List<SectorCardAsset> influencedCardType;
        
}