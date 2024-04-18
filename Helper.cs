using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlackJackSimulator.Program;

namespace BlackJackSimulator
{
    internal class Helper
    {
        public static List<Card> CreateDeck()
        {
            string[] suits = { "Spades", "Clubs", "Hearts", "Diamonds" };

            string[] faceValuaes = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

            List<Card> deck = new List<Card>();

            foreach (string suit in suits)
            {
                foreach (string faceValue in faceValuaes)
                {
                    deck.Add(new Card(suit, faceValue));
                }
            }
            return deck;
        }
        public static Card DealCard(List<Card> deck, List<Card> graveyard)
        {
            var random = new Random();
            int index = random.Next(deck.Count);
            Card dealedCard = deck[index];
            deck.Remove(deck[index]);
            if (deck.Count == 0)
            {
                deck.AddRange(graveyard);
                graveyard = new List<Card>();
            }
            return dealedCard;
        }
        public static string UserChoice(List<Card> playersHand)
        {
            string userChoiceMethod = "hallo";
            while (true)
            {
                if (playersHand.Count == 2 && HandCountOneAceToELeven(playersHand) == 21)
                {
                    userChoiceMethod = "blackjack";
                    break;
                }
                else if (playersHand.Count == 2 && playersHand[0].FaceValue == playersHand[1].FaceValue)
                {
                    Console.WriteLine("Type H for HIT, ST for STAND, SP for SPLIT or D for DOUBLE!");
                    string userType = Console.ReadLine().ToLower();
                    if (userType == "h" || userType == "hi" || userType == "hit")
                    {
                        userChoiceMethod = "hit";
                        break;
                    }
                    else if (userType == "st" || userType == "sta" || userType == "stan" || userType == "stand")
                    {
                        userChoiceMethod = "stand";
                        break;
                    }
                    else if (userType == "sp" || userType == "spl" || userType == "spli" || userType == "split")
                    {
                        if (playersHand.Count > 9)
                        {
                            Console.WriteLine("Your total number of hands can not exceed 10!!!");
                            continue;
                        }
                        userChoiceMethod = "split";
                        break;
                    }
                    else if (userType == "d" || userType == "do" || userType == "dou" || userType == "doub" || userType == "doubl" || userType == "double")
                    {
                        userChoiceMethod = "double";
                        break;
                    }
                    else
                    {
                        Console.WriteLine("WRONG INPUT!!!");
                        continue;
                    }
                }
                else if (playersHand.Count == 2)
                {
                    Console.WriteLine("Type H for HIT, ST for STAND or D for DOUBLE!");
                    string userType = Console.ReadLine().ToLower();
                    if (userType == "h" || userType == "hi" || userType == "hit")
                    {
                        userChoiceMethod = "hit";
                        break;
                    }
                    else if (userType == "st" || userType == "sta" || userType == "stan" || userType == "stand")
                    {
                        userChoiceMethod = "stand";
                        break;
                    }
                    else if (userType == "d" || userType == "do" || userType == "dou" || userType == "doub" || userType == "doubl" || userType == "double")
                    {
                        userChoiceMethod = "double";
                        break;
                    }
                    else
                    {
                        Console.WriteLine("WRONG INPUT!!!");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Type H for HIT or ST for STAND!");
                    string userType = Console.ReadLine().ToLower();
                    if (userType == "h" || userType == "hi" || userType == "hit")
                    {
                        userChoiceMethod = "hit";
                        break;
                    }
                    else if (userType == "st" || userType == "sta" || userType == "stan" || userType == "stand")
                    {
                        userChoiceMethod = "stand";
                        break;
                    }
                    else
                    {
                        Console.WriteLine("WRONG INPUT!!!");
                        continue;
                    }
                }
            }
            return userChoiceMethod;
        }
        public static List<Card> SelectingNumberOfDecks(List<Card> oneDeck)
        {
            List<Card> multipliedDeck = new List<Card>();
            int numberOfDecks;
            while (true)
            {
                Console.WriteLine("How many normal card decks, do you want to add to the final deck?");
                try
                {
                    numberOfDecks = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("ERROR!!! Please type a whole number instead!!");
                    continue;
                }
                if (numberOfDecks < 1 || numberOfDecks > 10)
                {
                    Console.WriteLine("ERROR!!! The number has to be between 1 and 10 instead!!");
                    continue;
                }
                else
                {
                    for (int i = 0; i < numberOfDecks; i++)
                    {
                        multipliedDeck.AddRange(oneDeck);
                    }
                    break;
                }
            }
            return multipliedDeck;
        }
        public static (int, bool, bool, bool, bool) PlayersTurn(List<List<Card>> listOfAllPlayersHands, List<Card> playersHand, List<Card> deck, List<Card> graveyard)
        {
            int minValue = 0;
            int maxValue = 0;
            int endValue = 0;
            string maxValueString = "";
            bool gameOver = false;
            bool isDouble = false;
            bool playerHas21InFirstRound = false;
            bool isSplitted = false;
            string userChoice = UserChoice(playersHand);

            if (userChoice == "blackjack")
            {
                endValue = 21;
                playerHas21InFirstRound = true;
                gameOver = true;
            }
            else if (userChoice == "hit")
            {
                playersHand.Add(DealCard(deck, graveyard));
                Console.WriteLine("Your " + CardNumberConverter(playersHand.Count) + " is " + playersHand.Last());
                minValue = HandCountAcesToOne(playersHand);
                maxValue = HandCountOneAceToELeven(playersHand);
                if (minValue == maxValue)
                {
                    maxValueString = "";
                }
                else
                {
                    maxValueString = " or " + maxValue;
                }
                Console.WriteLine("Your total count is " + minValue + maxValueString);
                if (minValue > 21)
                {
                    WriteThreeLines();
                    Console.WriteLine("Your total count is " + minValue);
                    endValue = minValue;
                    gameOver = true;
                }
            }
            else if (userChoice == "stand")
            {
                WriteOneLine();
                minValue = HandCountAcesToOne(playersHand);
                maxValue = HandCountOneAceToELeven(playersHand);
                if (minValue == maxValue || maxValue > 21)
                {
                    endValue = minValue;
                }
                else
                {
                    endValue = maxValue;
                }
                Console.WriteLine("Your total count is: " + endValue);
                gameOver = true;
            }
            else if (userChoice == "split")
            {
                WriteOneLine();
                List<Card> newHand = new List<Card>();
                listOfAllPlayersHands.Add(newHand);
                listOfAllPlayersHands[listOfAllPlayersHands.Count - 1].Add(playersHand[1]);
                playersHand.Remove(playersHand[1]);
                playersHand.Add(DealCard(deck, graveyard));
                listOfAllPlayersHands[listOfAllPlayersHands.Count - 1].Add(DealCard(deck, graveyard));
                isSplitted = true;
                gameOver = true;
            }
            else if (userChoice == "double")
            {
                isDouble = true;
                WriteOneLine();
                playersHand.Add(DealCard(deck, graveyard));
                Console.WriteLine("Your " + CardNumberConverter(playersHand.Count) + " card is " + playersHand.Last());
                minValue = HandCountAcesToOne(playersHand);
                maxValue = HandCountOneAceToELeven(playersHand);
                if (minValue == maxValue || maxValue > 21)
                {
                    endValue = minValue;
                    gameOver = true;
                }
                else 
                { 
                    endValue = maxValue;
                    gameOver = true;
                }
                Console.WriteLine("Your total count is: " + endValue);
                if (minValue > 21)
                {
                    WriteThreeLines();
                    gameOver = true;
                }
            }
            else { Console.WriteLine("THE PROGRAM HAS FAILED YOU!"); }
            return (endValue, gameOver, isDouble, playerHas21InFirstRound, isSplitted);
        }
        public static (int, bool) DealersTurn(Card dealersHiddenCard, List<Card> dealersVisibleCards, List<Card> deck, List<Card> graveyard, int playersEndValue)
        {
            List<Card> dealersHand = dealersVisibleCards;
            int minValue = HandCountAcesToOne(dealersHand);
            int maxValue = HandCountOneAceToELeven(dealersHand);
            int endValue = 0;
            bool dealersTurnOver = false;
            if (minValue >= 17)
            {
                endValue = minValue;
                Console.WriteLine("The dealers hidden card is " + dealersHiddenCard + " which gives the dealer a total count of " + endValue);
                dealersTurnOver = true;
            }
            else if (maxValue >= 18 && maxValue <= 21)
            {
                endValue = maxValue;
                Console.WriteLine("The dealers hidden card is " + dealersHiddenCard + " which gives the dealer a total count of " + endValue);
                dealersTurnOver = true;
            }
            else
            {
                Card dealersNewCard = DealCard(deck, graveyard);
                dealersHand.Add(dealersNewCard);
                Console.WriteLine("The dealer draws the card, " + dealersNewCard + "!");
            }
            return (endValue, dealersTurnOver);
        }
        public static decimal TurnConclusion(int playersEndValue, int dealersEndValue, decimal userCredit, decimal userBet, bool isDouble, bool playerHas21InFirstRound)
        {
            decimal newUserCredit = 0;
            if (playerHas21InFirstRound == true)
            {
                newUserCredit = Decimal.Add(userCredit, Decimal.Multiply(userBet, 1.5m));
                Console.WriteLine("CONGRATULATION! You got Black Jack and won " + Decimal.Multiply(userBet, 1.5m) + " credit.");
            }
            else if (playersEndValue == dealersEndValue && playersEndValue <= 21)
            {
                newUserCredit = userCredit;
                Console.WriteLine("It is draw! You did not win or lose anything.");
            }
            else if (playersEndValue > 21 && isDouble == false)
            {
                newUserCredit = Decimal.Subtract(userCredit, userBet);
                Console.WriteLine("SHAME ON YOU! You lost the game and lost " + userBet + " credit.");
            }
            else if (playersEndValue > 21 && isDouble == true)
            {
                newUserCredit = Decimal.Subtract(userCredit, Decimal.Add(userBet, userBet));
                Console.WriteLine("SHAME ON YOU! You lost the game and lost " + Decimal.Add(userBet, userBet) + " credit.");
            }
            else if (playersEndValue <= 21 && dealersEndValue > 21 && isDouble == false)
            {
                newUserCredit = Decimal.Add(userCredit, userBet);
                Console.WriteLine("CONGRATULATION! You won the game and won " + userBet + " credit.");
            }
            else if (playersEndValue <= 21 && dealersEndValue > 21 && isDouble == true)
            {
                newUserCredit = Decimal.Add(userCredit, Decimal.Add(userBet, userBet));
                Console.WriteLine("CONGRATULATION! You won the game and won " + Decimal.Add(userBet, userBet) + " credit.");
            }
            else if (playersEndValue < dealersEndValue && dealersEndValue <= 21 && isDouble == true)
            {
                newUserCredit = Decimal.Subtract(userCredit, Decimal.Add(userBet, userBet));
                Console.WriteLine("SHAME ON YOU! You lost the game and lost " + Decimal.Add(userBet, userBet) + " credit.");
            }
            else if (playersEndValue > dealersEndValue && playersEndValue <= 21 && isDouble == true)
            {
                newUserCredit = Decimal.Add(userCredit, Decimal.Add(userBet, userBet));
                Console.WriteLine("CONGRATULATION! You won the game and won " + Decimal.Add(userBet, userBet) + " credit.");
            }
            else if (playersEndValue < dealersEndValue && dealersEndValue <= 21 && isDouble == false)
            {
                newUserCredit = Decimal.Subtract(userCredit, userBet);
                Console.WriteLine("SHAME ON YOU! You lost the game and lost " + userBet + " credit.");
            }
            else if (playersEndValue > dealersEndValue && playersEndValue <= 21 && isDouble == false)
            {
                newUserCredit = Decimal.Add(userCredit, userBet);
                Console.WriteLine("CONGRATULATION! You won the game and won " + userBet + " credit.");
            }
            else if (playersEndValue > 21 && dealersEndValue > 21 && isDouble == false)
            {
                newUserCredit = Decimal.Subtract(userCredit, userBet);
                Console.WriteLine("SHAME ON YOU! You lost the game and lost " + userBet + " credit.");
            }
            else if (playersEndValue > 21 && dealersEndValue > 21 && isDouble == true)
            {
                newUserCredit = Decimal.Subtract(userCredit, Decimal.Add(userBet, userBet));
                Console.WriteLine("SHAME ON YOU! You lost the game and lost " + Decimal.Add(userBet, userBet) + " credit.");
            }
            else
            {
                Console.WriteLine("The program has failed you!");
                userCredit = newUserCredit;
            }

            return newUserCredit;
        }
        public static decimal UserBet(decimal playerCredit)
        {
            decimal userBet = 0;
            while (true)
            {
                Console.WriteLine("How much of your credit do you want to bet?");
                try
                {
                    userBet = Convert.ToDecimal(Int32.Parse(Console.ReadLine()));
                }
                catch
                {
                    Console.WriteLine("ERROR!!! Please type a whole number instead!!");
                    continue;
                }

                if (userBet > playerCredit || userBet < 1)
                {
                    Console.WriteLine("Not enough credit!! Type a whole number between 1 and " + playerCredit);
                    continue;
                }
                else
                {
                    break;
                }
            }
            return userBet;
        }
        public static void EmptyHand(List<Card> hand, List<Card> graveyard)
        {
            while (hand.Count != 0)
            {
                graveyard.Add(hand[0]);
                hand.Remove(hand[0]);
            }
        }
        public static void EmptyHandButFirst(List<Card> hand, List<Card> graveyard)
        {
            while (hand.Count != 1)
            {
                graveyard.Add(hand[1]);
                hand.Remove(hand[1]);
            }
        }
        public static string RestOfCards(List<Card> playersHandMethod)
        {
            string handString = "";

            for (int i = 1; i < playersHandMethod.Count; i++)
            {
                handString = handString + " and " + playersHandMethod[i];
            }

            return handString;
        }
        public static int FaceValueConverterAceToEleven(string faceValue)
        {
            int faceValueIntMethod = 0;

            try
            {
                faceValueIntMethod = Int32.Parse(faceValue);
            }

            catch
            {
                if (faceValue == "Jack" || faceValue == "Queen" || faceValue == "King")
                {
                    faceValueIntMethod = 10;
                }
                else if (faceValue == "Ace")
                {
                    faceValueIntMethod = 11;
                }
                else
                {
                    Console.WriteLine("BIG ERROR!!!");
                }
            }

            return faceValueIntMethod;
        }
        public static int FaceValueConverterAceToOne(string faceValue)
        {
            int faceValueIntMethod = 0;

            try
            {
                faceValueIntMethod = Int32.Parse(faceValue);
            }

            catch
            {
                if (faceValue == "Jack" || faceValue == "Queen" || faceValue == "King")
                {
                    faceValueIntMethod = 10;
                }
                else if (faceValue == "Ace")
                {
                    faceValueIntMethod = 1;
                }
                else
                {
                    Console.WriteLine("BIG ERROR!!!");
                }
            }

            return faceValueIntMethod;
        }
        public static int HandCountAcesToOne(List<Card> hand)
        {
            int count = 0;
            for (int i = 0; i < hand.Count; i++)
            {
                count = FaceValueConverterAceToOne(hand[i].FaceValue) + count;
            }
            return count;
        }
        public static int HandCountOneAceToELeven(List<Card> hand)
        {
            int count = 0;
            bool moreThanOneAce = false;
            for (int i = 0; i < hand.Count; i++)
            {
                if (moreThanOneAce == false)
                {
                    count = FaceValueConverterAceToEleven(hand[i].FaceValue) + count;
                    if (hand[i].FaceValue == "Ace")
                    {
                        moreThanOneAce = true;
                    }
                }
                else
                {
                    count = FaceValueConverterAceToOne(hand[i].FaceValue) + count;
                }
            }
            return count;
        }
        public static string CardNumberConverter(int handsize)
        {
            if (handsize == 3)
            {
                return "third";
            }
            else if (handsize == 4)
            {
                return "fourth";
            }
            else if (handsize == 5)
            {
                return "fifth";
            }
            else if (handsize == 6)
            {
                return "sixth";
            }
            else if (handsize == 7)
            {
                return "seventh";
            }
            else if (handsize == 8)
            {
                return "eighth";
            }
            else if (handsize == 9)
            {
                return "nineth";
            }
            else if (handsize == 10)
            {
                return "tenth";
            }
            else { return "ERROR"; }
        }
        public static void WriteOneLine()
        {
            Console.WriteLine("-----------------------------------------------------------");
        }
        public static void WriteThreeLines()
        {
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("-----------------------------------------------------------");
        }
    }
}
