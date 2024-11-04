
using flupppi.CoolHex;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class RailHexLoader : HexMapLoader
{
    public List<GameObject> railPrefabs;

    public void Start()
    {
        if (MapData == null || railPrefabs == null || railPrefabs.Count == 0 || HexPrefab == null)
        {
            Debug.LogError("MapData is not assigned, or railPrefabs list is empty.");

            Debug.LogError("MapData or HexPrefab is not assigned.");
            return;
        }
        if (hexMapParent == null)
        {
            // Load the hex map based on the data in the asset
            hexMapParent = new GameObject("HexMap");
        }

        // Set up the prefab with data like weight, blocked status, etc.
        foreach (var hexData in MapData.HexFields)
        {
            int q = (int)hexData.Position.x;
            int r = (int)hexData.Position.y;

            // Calculate the world postion using q, r
            Vector3 position = HexUtils.CalculateHexPosition(q, r);

            GameObject randomRailPrefab = railPrefabs[UnityEngine.Random.Range(0, railPrefabs.Count)];

            var hexInstance = Instantiate(randomRailPrefab, position, Quaternion.Euler(0, 180, 0), hexMapParent.transform);

            HexInstance hexComponent = hexInstance.GetComponent<HexInstance>();
            if (hexComponent != null)
            {
                hexComponent.Setup(hexData);
            }
        }
        hexMapParent.transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
