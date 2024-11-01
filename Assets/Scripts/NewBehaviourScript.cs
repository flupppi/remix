using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // The GameObject to instantiate.
    public GameObject cardEntityToSpawn;
    public GameObject eventCardPrefab;

    // An instance of the ScriptableObject defined above.
    

    public List<SectorCardAsset> sectorsOfInfluence;
    public List<SectorCardAsset> sectorsOfPower;
    public List<EventCardAsset> eventCards;
    // This will be appended to the name of the created entities and increment when each is created.
    int instanceNumber = 1;

    void Start()
    {
        SpawnEntities(sectorsOfInfluence, cardEntityToSpawn);  
        SpawnEntities(sectorsOfPower, cardEntityToSpawn);
        SpawnEventCards(eventCards, eventCardPrefab);
    }

    void SpawnEntities(List<SectorCardAsset> sectors, GameObject cardGO)
    {
        int currentSpawnPointIndex = 0;
        foreach (SectorCardAsset spawnManagerValues in sectors) {
            for (int i = 0; i < spawnManagerValues.numberOfPrefabsToCreate; i++)
            {
                // Creates an instance of the prefab at the current spawn point.
                GameObject currentEntity = Instantiate(cardGO, spawnManagerValues.spawnPoints[currentSpawnPointIndex], Quaternion.identity);
                // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
                currentEntity.name = spawnManagerValues.prefabName + instanceNumber;
                Card card = currentEntity.GetComponent<Card>();
                card.cardTitle = spawnManagerValues.cardTitle;
                card.cardIcon = spawnManagerValues.cardIcon;
                card.cardDescription = spawnManagerValues.cardDescription;
                card.minimumCost = spawnManagerValues.minimumCost;
                card.numberOfInfluence = spawnManagerValues.numberOfInfluence;
                card.revenueIncrease = spawnManagerValues.revenueIncrease;
                // Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
                currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnManagerValues.spawnPoints.Length;

                card.InitCard();
                GameManager.Instance.cards.Add(card);


                instanceNumber++;

            }
        }
    }


    void SpawnEventCards(List<EventCardAsset> sectors, GameObject cardGO)
    {
        int currentSpawnPointIndex = 0;
        foreach (EventCardAsset spawnManagerValues in sectors)
        {
            for (int i = 0; i < spawnManagerValues.numberOfPrefabsToCreate; i++)
            {
                // Creates an instance of the prefab at the current spawn point.
                GameObject currentEntity = Instantiate(cardGO, spawnManagerValues.spawnPoints[currentSpawnPointIndex], Quaternion.identity);
                // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
                currentEntity.name = spawnManagerValues.prefabName + instanceNumber;
                EventCard card = currentEntity.GetComponent<EventCard>();
                card.cardTitle = spawnManagerValues.cardTitle;
                card.cardIcon = spawnManagerValues.cardIcon;
                card.cardDescription = spawnManagerValues.cardDescription;
                card.minimumCostEffect = spawnManagerValues.minimumCostEffect;
                card.numberOfInfluenceEffect = spawnManagerValues.numberOfInfluenceEffect;
                card.revenueIncreaseEffect = spawnManagerValues.revenueEffect;
                // Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
                currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnManagerValues.spawnPoints.Length;

                card.InitCard();
                GameManager.Instance.events.Add(card);
                instanceNumber++;

            }
        }
    }


}