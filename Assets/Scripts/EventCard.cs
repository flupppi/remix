using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EventCard : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private TMP_Text titleTextField;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text revenueText;
    [SerializeField] private TMP_Text minimumCostText;
    [SerializeField] private TMP_Text influenceText;
    [SerializeField] private CardState cardState;
    [SerializeField] private Player mostInfluencePlayer = null;




    public string cardTitle;
    public string cardDescription;
    public Sprite cardIcon;
    public float revenueIncreaseEffect;
    public float minimumCostEffect;
    public float numberOfInfluenceEffect;
    [SerializeField] MeshRenderer meshRenderer;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InitCard()
    {
        titleTextField.text = cardTitle;
        descriptionText.text = cardDescription;
        revenueText.text = revenueIncreaseEffect.ToString();
        influenceText.text = numberOfInfluenceEffect.ToString();
        minimumCostText.text = minimumCostEffect.ToString();
        icon.sprite = cardIcon;

        meshRenderer.material.color = color;
        
    }
}
