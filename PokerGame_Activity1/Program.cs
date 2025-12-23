using System;
using System.Collections.Generic;

class Program
{
    struct Card
    {
        public string value;
        public string suit;
    }

    static void Main()
    {
        // ACTIVITY 1: CONSOLE POKER GAME

        string[] values = { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };
        string[] suits = { "Diamonds", "Hearts", "Clubs", "Spades" };
        Random rand = new Random();


        // list para sa scoreboard (for commit and push)
        List<string> scoreboard = new List<string>();

        Console.WriteLine("[ WELCOME TO THE CONSOLE POKER GAME ]");
        Console.Write("Please enter your name: ");
        string name = Console.ReadLine();

        // Display Rules and Prizes
        Console.WriteLine("\n------------------------------------------------");
        Console.WriteLine($"Hi {name}!\nHere are the rules and prizes for this Poker Game:");
        Console.WriteLine(" Starting Balance: 500 coins.");
        Console.WriteLine(" Cost per round:   100 coins.");
        Console.WriteLine(" Winning Prizes:");
        Console.WriteLine(" \n------------------------------------------------");
        Console.WriteLine("Straight Flush: 1000 \nFlush: 500\nStraight: 400\nFull House: 300 \n3-of-a-Kind: 150 \n2-Pairs: 100 \n1-Pair: 50");
        Console.WriteLine("------------------------------------------------");

        // Error Handling para sa yes or no input
        string startChoice = "";
        while (startChoice != "yes" && startChoice != "no")
        {
            Console.Write("\nDo you want to start the game? (yes/no): ");
            startChoice = Console.ReadLine().ToLower();
            if (startChoice != "yes" && startChoice != "no")
            {
                Console.WriteLine("(!) Invalid input. Please type 'yes' or 'no'.");
            }
        }

        if (startChoice == "no")
        {
            Console.WriteLine("Maybe next time! Goodbye.");
            return;
        }

        int balance = 500;
        bool playAgain = true;
        int roundNumber = 1; //to keep track of the history of rounds played

        // Main Game Loop
        while (playAgain)
        {

            //the added new feature (For Push and commit)
            Console.Clear();
            if (scoreboard.Count > 0)
            {
                Console.WriteLine("=== GAME SCOREBOARD HISTORY ===");
                foreach (string record in scoreboard)
                {
                    Console.WriteLine(record);
                }
                Console.WriteLine("===============================\n");
            }

            Console.WriteLine("========================================");
            Console.WriteLine($"Player: {name} | Balance: {balance} coins");
            Console.WriteLine("========================================");
            int initialCoinsThisRound = balance;
            Console.WriteLine("Deducting 100 coins for this round...");
            balance -= 100;

            // Create Deck
            Card[] deck = new Card[52];
            int index = 0;
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    deck[index].value = values[i];
                    deck[index].suit = suits[j];
                    index++;
                }
            }

            // Deal 5 random cards
            Console.WriteLine("\nDealing your hand...");
            Card[] hand = new Card[5];
            string handCardsString = "";
            for (int i = 0; i < 5; i++)
            {
                int rindex = rand.Next(52);
                hand[i] = deck[rindex];
                Console.WriteLine($" -> {hand[i].value} of {hand[i].suit}");
                handCardsString += hand[i].value + " of " + hand[i].suit + (i < 4 ? ", " : "");
            }

            // Analyze Hand
            int[] vcount = new int[13];
            int[] scount = new int[4];
            foreach (var card in hand)
            {
                for (int j = 0; j < 13; j++) if (card.value == values[j]) vcount[j]++;
                for (int j = 0; j < 4; j++) if (card.suit == suits[j]) scount[j]++;
            }

            int pairs = 0, threeOfKind = 0;
            bool isFlush = false, isStraight = false;
            for (int i = 0; i < 13; i++)
            {
                if (vcount[i] == 2) pairs++;
                if (vcount[i] == 3) threeOfKind++;
            }
            for (int i = 0; i < 4; i++) if (scount[i] == 5) isFlush = true;
            for (int i = 0; i < 9; i++)
                if (vcount[i] > 0 && vcount[i + 1] > 0 && vcount[i + 2] > 0 && vcount[i + 3] > 0 && vcount[i + 4] > 0) isStraight = true;

            // Determine Result and Prize
            int prize = 0;
            string handName = "";
            if (isFlush && isStraight) { prize = 1000; handName = "STRAIGHT FLUSH"; }
            else if (isFlush) { prize = 500; handName = "FLUSH"; }
            else if (isStraight) { prize = 400; handName = "STRAIGHT"; }
            else if (threeOfKind > 0 && pairs > 0) { prize = 300; handName = "FULL HOUSE"; }
            else if (threeOfKind > 0) { prize = 150; handName = "THREE OF A KIND"; }
            else if (pairs == 2) { prize = 100; handName = "TWO PAIRS"; }
            else if (pairs == 1) { prize = 50; handName = "ONE PAIR"; }
            else { prize = 0; handName = "NOTHING"; }

            Console.WriteLine("\n-----------------------------");
            Console.WriteLine($"RESULT: {handName}");
            if (prize > 0)
            {
                Console.WriteLine($"Congratulations! You won {prize} coins!");
                balance += prize;
            }
            else
            {
                Console.WriteLine("Sorry, no winning combination this time.");
            }
            Console.WriteLine($"New Balance: {balance} coins");
            Console.WriteLine("-----------------------------");

            // ADDED FEATURE: Pag-compile ng data para sa history entry (changes for push and commit)

            string historyEntry = $"Round {roundNumber}: [Coins: {initialCoinsThisRound}] -> Cards: [{handCardsString}] -> Result: {handName} -> Updated Balance: {balance}";
            scoreboard.Add(historyEntry);
            roundNumber++;

            // Check if bankrupt na yung user
            if (balance < 100)
            {
                Console.WriteLine("\n!!! GAME OVER !!!");
                Console.WriteLine("You have gone BANKRUPT! You don't have enough coins to play the next round.");
                Console.WriteLine("Better luck next time!");
                break;
            }

            // Error Handling 
            bool validChoice = false;
            while (!validChoice)
            {
                Console.WriteLine("\n[Want to play another round?]");
                Console.WriteLine("Press [Enter] to continue");
                Console.WriteLine("Press [1] to Exit / Quit");
                Console.Write("Your choice: ");
                string input = Console.ReadLine();

                if (input == "")
                {
                    validChoice = true;
                }
                else if (input == "1")
                {
                    playAgain = false;
                    validChoice = true;
                }
                else
                {
                    Console.WriteLine("(!) Invalid input. Please just press Enter or type 1.");
                }
            }
        }

        Console.WriteLine($"\nFinal Balance: {balance}. Thanks for playing, {name}!");
        Console.WriteLine("Press any key to close the window...");
        Console.ReadKey();
    }
}
