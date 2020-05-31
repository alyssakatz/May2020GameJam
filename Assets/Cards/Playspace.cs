using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;

public class Playspace : Singleton<Playspace>
{ 
    private System.Random DeckRNG;
    private Queue<CardInfo> Deck;
    private Stack<CardInfo> Discard;
    private static bool _isInitialized = false;

    public enum Hand
    {
        LeftCard,
        MiddleCard,
        RightCard
    }

    public CardInfo? LeftCard;
    public CardInfo? MiddleCard;
    public CardInfo? RightCard;
    private CardInfo? _nullCard = null;

    public void Initialize(int[] cardsInDeck)
    {
        DeckRNG = new System.Random();
        Deck = new Queue<CardInfo>();
        Discard = new Stack<CardInfo>();
        foreach (int i in cardsInDeck)
        {
            Deck.Enqueue(Cards.CardList[i]);
        }
        ShuffleDeck();
        _isInitialized = true;
    }

    public void ShuffleDeck()
    {
        Deck.OrderBy(o => DeckRNG.Next());
    }

    public void RefreshDiscard()
    {
        while(Discard.Count > 0)
        {
            Deck.Enqueue(Discard.Pop());
        }
        ShuffleDeck();
    }

    public void DrawCards(bool withDiscard)
    {
        foreach (Hand position in Enum.GetValues(typeof(Hand)))
        {
            if (withDiscard)
            {
                DiscardCard(ref CardAtPosition(position));
            }
            DrawCard(ref CardAtPosition(position));
        }
    }

    public void DiscardCard(ref CardInfo? card)
    {
        if(card != null)
        {
            Discard.Push((CardInfo) card);
            card = null;
        }
    }

    public void DrawCard(ref CardInfo? card)
    {
        if (Deck.Count == 0)
        {
            RefreshDiscard();
        }
        card = card ?? Deck.Dequeue();
    }

    public ref CardInfo? CardAtPosition(Hand position)
    {
        switch(position)
        {
            case Hand.LeftCard: return ref LeftCard;
            case Hand.MiddleCard: return ref MiddleCard;
            case Hand.RightCard: return ref RightCard;
        }
        return ref _nullCard;
    }
}
