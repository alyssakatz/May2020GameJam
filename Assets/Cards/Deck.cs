using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Playspace
{ 
    private System.Random DeckRNG;
    private Queue<CardInfo> Deck;
    private Stack<CardInfo> Discard;


    public enum Hand
    {
        LeftCard,
        MiddleCard,
        RightCard
    }

    public CardInfo? LeftCard;
    public CardInfo? MiddleCard;
    public CardInfo? RightCard;

    public Playspace(int[] cardsInDeck)
    {
        DeckRNG = new System.Random();
        Deck = new Queue<CardInfo>();
        foreach (int i in cardsInDeck)
        {
            Deck.Enqueue(Cards.CardList[i]);
        }
        ShuffleDeck();
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
                DiscardCard(CardAtPosition(position));
            }
            DrawCard(CardAtPosition(position));
        }
    }

    public void DiscardCard(CardInfo? card)
    {
        card = null;
    }

    public void DrawCard(CardInfo? card)
    {
        if(card == null)
        {
            if(Deck.Count == 0)
            {
                RefreshDiscard();
            }
            card = Deck.Dequeue();
        }
    }

    public CardInfo? CardAtPosition(Hand position)
    {
        switch(position)
        {
            case Hand.LeftCard: return LeftCard;
            case Hand.MiddleCard: return MiddleCard;
            case Hand.RightCard: return RightCard;
        }
        return null;
    }


}
