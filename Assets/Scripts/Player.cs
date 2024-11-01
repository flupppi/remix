using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public bool isActive = false;
    [SerializeField] public Color playerColor;
    [SerializeField] private string playerName = "Player1";
    [SerializeField] private TMP_Text playerTitle;
    [SerializeField] private Image playerIcon;
    [SerializeField] public TMP_Text bankText;
    [SerializeField] public int bankAmount;

    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public int score;
    [SerializeField] public TMP_Text revenueText;
    [SerializeField] public int revenue;
    public int voters;

    [SerializeField] public TMP_Text betText;
    [SerializeField] public int bet;

    [SerializeField] public Button placeButton;
    [SerializeField] public Button incrementButton;
    [SerializeField] public Button decrementButton;

    [SerializeField] public Image backgroundPanel;
    Camera camera;
    public Card selectedCard;

    public void IncrementBetAmount()
    {
        if (bet < bankAmount)
        {
            bet++;
        }
    }
    public void DecrementBetAmount()
    {
        if (bet > 0)
        {
            bet--;
        }
    }

    public void PlaceBet()
    {
        if (selectedCard == null)
        {
        }
        else
        {
            selectedCard.RaiseCard(false);

            // do something with the selected card. 
            if (selectedCard.Bet(bet, this))
            {
                bankAmount -= bet;
            }
            bet = 0;
            // unselect card
            selectedCard = null;
        }
        GameManager.Instance.ChangePlayer();
        Debug.Log("Bet has been Placed");
    }


    void Start()
    {
        playerTitle.text = playerName;
        camera = FindFirstObjectByType<Camera>();
    }

    void Update()
    {
        betText.text = bet.ToString();
        scoreText.text = score.ToString();
        bankText.text = bankAmount.ToString();
        revenueText.text = revenue.ToString();
    }

    public void SetPlayerActive()
    {
        isActive = true;
        incrementButton.gameObject.SetActive(true);
        decrementButton.gameObject.SetActive(true);
        placeButton.gameObject.SetActive(true);
        backgroundPanel.color = Color.green;
    }
    public void SetPlayerInactive()
    {
        isActive = false;
        incrementButton.gameObject.SetActive(false);
        decrementButton.gameObject.SetActive(false);
        placeButton.gameObject.SetActive(false);
        backgroundPanel.color = Color.grey;
    }


    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 4;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity,layerMask ))
            {
                Transform objectHit = hit.transform;
                if (objectHit != null)
                {
                    if (objectHit.transform.parent?.GetComponent<Card>() != null)
                    {
                        selectedCard = objectHit.transform.parent.GetComponent<Card>();
                    }
                }
                
                // Do something with the object that was hit by the raycast.
            }
            GameManager.Instance.RaiseCard(selectedCard);
        }
    }

    internal void AddPayout(int revenueIncrease, int numberOfInfluence)
    {
        voters += numberOfInfluence;
        revenue += revenueIncrease;
    }

    public void IncreaseBank(){
        bankAmount += revenue;
    }

    internal void ResetStats()
    {
        voters = 0;
        revenue = 0;
    }
}
