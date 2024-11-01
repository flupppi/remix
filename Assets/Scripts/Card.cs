using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CardState
{
    Idle,
    Dragged,
    Dropped
}

public class Card : MonoBehaviour
{
    [SerializeField] public Color color;
    public Color selectColor;
    [SerializeField] private TMP_Text titleTextField;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text revenueText;
    [SerializeField] private TMP_Text minimumCostText;
    [SerializeField] private TMP_Text influenceText;
    [SerializeField] private CardState cardState;
    [SerializeField] public Player mostInfluencePlayer = null;
    [SerializeField] public MeshRenderer meshRenderer;
    [SerializeField] private GameObject betPlayerTemplate;
    [SerializeField] private Transform betPlayerTextContainer;
    private Dictionary<Player, int> playerBets = new Dictionary<Player, int>();
    private Dictionary<Player, TMP_Text> betPlayerTexts = new Dictionary<Player, TMP_Text>();
    private Dictionary<Player, Image> betPlayerImage = new Dictionary<Player, Image>();
    public string cardTitle;
    public string cardDescription;
    public Sprite cardIcon;
    public int revenueIncrease;
    public int minimumCost;
    public int numberOfInfluence;

    void Start()
    {
        InitCard();
    }

    void Update()
    {
        UpdateCardColor();
    }

    public void InitCard()
    {
        titleTextField.text = cardTitle;
        descriptionText.text = cardDescription;
        revenueText.text = revenueIncrease.ToString();
        influenceText.text = numberOfInfluence.ToString();
        minimumCostText.text = minimumCost.ToString();
        icon.sprite = cardIcon;
        meshRenderer.material.color = color;
    }

    public void ChangeColor(bool selected)
    {
        if (selected)
        {
            meshRenderer.material.color = selectColor;
        }
        else
        {
            meshRenderer.material.color = color;
        }
    }

    internal void RaiseCard(bool v)
    {
        float raiseAmount = v ? 1.0f : 0.0f;
        Vector3 currentPos = transform.position;
        currentPos.y = raiseAmount;
        transform.position = currentPos;
    }

    public bool Bet(int bet, Player player)
    {
        if (bet < minimumCost)
        {
            GameManager.Instance.RejectBet();
            return false;
        }
        if (playerBets.ContainsKey(player))
        {
            playerBets[player] += bet;
        }
        else
        {
            playerBets[player] = bet;
            GameObject newBetGO = Instantiate(betPlayerTemplate, betPlayerTextContainer);
            newBetGO.transform.localPosition = new Vector3((5.0f * (playerBets.Count-1)), 0.0f, 0.0f);
            TMP_Text newBetText = newBetGO.GetComponentInChildren<TMP_Text>();
            newBetText.gameObject.SetActive(true);
            betPlayerTexts[player] = newBetText;
            Image newBetImage = newBetGO.GetComponent<Image>();
            betPlayerImage[player] = newBetImage;
            newBetImage.gameObject.SetActive(true);
        }

        UpdateBetTexts();
        UpdateBetImages();
        UpdateCardColor();
        return true;
    }

    private void UpdateBetTexts()
    {
        foreach (var playerBet in playerBets)
        {
            betPlayerTexts[playerBet.Key].text = $"{playerBet.Value}";
        }
    }
    private void UpdateBetImages()
    {
        foreach (var image in betPlayerImage)
        {
            betPlayerImage[image.Key].color = image.Key.playerColor;
        }
    }



    private void UpdateCardColor()
    {
        Player highestBetPlayer = null;
        int highestBet = 0;

        foreach (var playerBet in playerBets)
        {
            if (playerBet.Value > highestBet)
            {
                highestBet = playerBet.Value;
                highestBetPlayer = playerBet.Key;
            }
            else if(playerBet.Value == highestBet)
            {
                highestBetPlayer = null;
            }
            

        }

        if (highestBetPlayer != null)
        {
            mostInfluencePlayer = highestBetPlayer;
            ColorCard(highestBetPlayer.playerColor);
        }
        else
        {
            mostInfluencePlayer = null;

            ColorCard(Color.white);
        }

    }

    void ColorCard(Color color)
    {
        meshRenderer.material.color = color;
    }

    internal void DistributePayout()
    {
        if (mostInfluencePlayer != null)
        {
            mostInfluencePlayer.AddPayout(revenueIncrease, numberOfInfluence);
        }
    }


}


