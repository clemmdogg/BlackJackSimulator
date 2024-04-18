using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static BlackJackSimulator.Helper;

namespace BlackJackSimulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            decimal playerCredit = 500;
            List<Card> oneDeck = CreateDeck();
            List<Card> playersHand = new List<Card>();
            List<Card> graveyard = new List<Card>();
            List<List<Card>> listOfAllPlayersHands = new List<List<Card>>();
            List<Card> dealersVisibleCards = new List<Card>();
            listOfAllPlayersHands.Add(playersHand);

            WriteThreeLines();
            Console.WriteLine("Hello and welcome to the Black Jack simulator!");

            List<Card> deck = SelectingNumberOfDecks(oneDeck);

            while (playerCredit >= 1 && playerCredit < 2000000000)
            {
                Console.WriteLine("You have " + playerCredit + " left in credit to bet for!");
                decimal userBet = UserBet(playerCredit);
                Card dealersHiddenCard = DealCard(deck, graveyard);
                playersHand.Add(DealCard(deck, graveyard));
                dealersVisibleCards.Add(DealCard(deck, graveyard));
                playersHand.Add(DealCard(deck, graveyard));

                for (int i = 0; i < listOfAllPlayersHands.Count; i++)
                {
                    WriteOneLine();
                    Console.WriteLine("You have drawn the cards: " + listOfAllPlayersHands[i][0] + RestOfCards(listOfAllPlayersHands[i]));
                    Console.WriteLine("The dealers visable card is: " + dealersVisibleCards[0] + RestOfCards(dealersVisibleCards));
                    WriteOneLine();

                    int playersEndValue = 0;
                    bool playerTurnOver = false;
                    bool isDouble = false;
                    bool playerHas21InFirstRound = false;
                    bool isSplitted = false;
                    while (playerTurnOver == false)
                    {
                        (playersEndValue, playerTurnOver, isDouble, playerHas21InFirstRound, isSplitted) = PlayersTurn(listOfAllPlayersHands, listOfAllPlayersHands[i], deck, graveyard);
                    }
                    if (isSplitted == true)
                    {
                        i = -1;
                        continue;
                    }
                    int dealersEndValue = 0;
                    bool dealerTurnOver = false;
                    dealersVisibleCards.Add(dealersHiddenCard);
                    while (dealerTurnOver == false)
                    {
                        (dealersEndValue, dealerTurnOver) = DealersTurn(dealersHiddenCard, dealersVisibleCards, deck, graveyard, playersEndValue);
                    }
                    playerCredit = TurnConclusion(playersEndValue, dealersEndValue, playerCredit, userBet, isDouble, playerHas21InFirstRound);

                    WriteThreeLines();
                    EmptyHand(listOfAllPlayersHands[i], graveyard);
                    if (i+1 == listOfAllPlayersHands.Count)
                    {
                        EmptyHand(dealersVisibleCards, graveyard);
                        graveyard.Add(dealersHiddenCard);
                    }
                    else
                    {
                        dealersVisibleCards.Remove(dealersHiddenCard);
                    }
                }
                /// Remove all extra hands.
                listOfAllPlayersHands.Clear();
                listOfAllPlayersHands.Add(playersHand);
            }
        }
        public class Card
        {
            public String Suit { get; }
            public String FaceValue { get; }
            public Card(string suit, string faceValue)
            {
                Suit = suit;
                FaceValue = faceValue;
            }
            public override string ToString()
            {
                return $"{FaceValue}-of-{Suit}";
            }
        }
    }
}