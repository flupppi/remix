using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum TurnStates
{
    Betting,
    Voting
}
public class GameManager : MonoBehaviour
{

    public GameObject winPanel;
    [SerializeField] private AudioSource rejectAudio;
    [SerializeField] private int playerCount = 1;
    [SerializeField] private int counter = 0;
    [SerializeField] private int maxRounds = 11;
    [SerializeField] float bettingTime = 3.0f;
    [SerializeField] float votingTime = 1.0f;
    [SerializeField] int maxPopluationCount = 100;
    [SerializeField] GameObject voterParent;
    [SerializeField] TMP_Text player1VotersText;
    [SerializeField] TMP_Text player2VotersText;
    [SerializeField] TMP_Text neutralVotersText;
    [SerializeField] int neutralVoters = 100;
    [SerializeField] int player1Voters;
    [SerializeField] int player2Voters;
    [SerializeField] Button finishTurnButton;

    [SerializeField] private Player activePlayer;
    private Player firstActivePlayer;

    public List<Card> cards;
    public List<EventCard> events;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }



    [SerializeField] private TMP_Text counterText;

    [SerializeField] private List<Player> players;

    [SerializeField] private GameObject voterPrefab;


    public TurnStates turnState;
    //public UnityEvent OnFinishRound;

    // Start is called before the first frame update
    void Start()
    {
        winPanel.SetActive(false);
        int startPlayer = UnityEngine.Random.Range(0, players.Count);
        activePlayer = players[startPlayer];
        activePlayer.SetPlayerActive();
        firstActivePlayer = activePlayer;
        ChangePlayer();

        //SpawnPopulation();
    }

    void SpawnPopulation()
    {
        for (int i = 0; i < maxPopluationCount; i++) {
            Instantiate(voterPrefab, voterParent.transform);
            

        }
    }

    // Update is called once per frame
    void Update()
    {
        
        player1VotersText.text = players[0].voters.ToString();
        player2VotersText.text = players[1].voters.ToString();
        neutralVotersText.text = neutralVoters.ToString();


        
    }

    private void OnFinishGame()
    {
        winPanel.SetActive(true);
        TMP_Text win_text = winPanel.GetComponentInChildren<TMP_Text>();
        if (players[0].score > players[1].score)
        {
            win_text.text = "Player 1 Won";
            Debug.Log("Player 1 Won");
        }
        else if (players[0].voters < players[1].voters)
        {
            win_text.text = "Player 2 Won";

            Debug.Log("Player 2 Won");
        }
        else
        {
            win_text.text = "nobody Won";

            Debug.Log("No one won");
        }
        Debug.Log("Game Finished");
    }

    void OnFinishVotingRound()
    {
        turnState = TurnStates.Betting;
        Debug.Log("Finished Voting Round");
        bettingTime = 3.0f;
        counter++;
        counterText.text = counter.ToString();

        foreach (var player in players)
        {
            player.IncreaseBank();
        }
        if (players[0].voters > players[1].voters)
        {
            players[0].score += 1;
        }
        else if (players[0].voters < players[1].voters)
        {
            players[1].score += 1;
        }
        else
        {
            players[1].score += 1;
            players[0].score += 1;
        }
    }

    void OnFinishBettingRound()
    {
        Debug.Log("Finished Betting Round");
        turnState = TurnStates.Voting;
        foreach (var player in players)
        {
            player.ResetStats();
        }
        foreach (var card in cards)
        {
            
            card.DistributePayout();

        }

        OnFinishVotingRound();
    }

    public void FinishTurn()
    {
        foreach (var player in players) {
            player.SetPlayerInactive();
        }
        if (counter < maxRounds)
        {
           OnFinishBettingRound();

        }
        else { 
            OnFinishGame();
            return;
        }

       
        Debug.Log("Players Finished Turn");
        foreach (var player in players)
        {
            if (player!=firstActivePlayer)
            {
                
                player.SetPlayerActive();
            }
        }

        firstActivePlayer = activePlayer;
    }

    public void ChangePlayer()
    {
        foreach (Player player in players)
        {
            if (player != activePlayer)
            {
                activePlayer.SetPlayerInactive();
                activePlayer = player;
                activePlayer.SetPlayerActive();
                return;
            }
        }
            
    }

    internal void RaiseCard(Card selectedCard)
    {
        foreach(Card card in cards)
        {
            if (card != selectedCard)
            {
                card.RaiseCard(false);
                card.ChangeColor(true);
            }
            else
            {
                card.RaiseCard(true);
                card.ChangeColor(false);
            }
        }
    }

    internal void RejectBet()
    {
        rejectAudio.Play();
    }
}
