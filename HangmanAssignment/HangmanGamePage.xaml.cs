using Microsoft.Maui.Controls;
using System;

namespace HangmanAssignment
{
    public partial class HangmanGamePage : ContentPage
    {
        private string selectedWord;
        private char[] currentProgress;
        private int incorrectGuesses;
        private string guessedLetters;
        private readonly string[] hangmanStages ={"hang1.png", "hang2.png", "hang3.png", "hang4.png","hang5.png", "hang6.png", "hang7.png", "hang8.png"};

        // Array of words from which the game will randomly select one for the player to guess
        private readonly string[] wordBank =
        {   
            "UNITY", "DEVELOPER", "DATABASE", "ALGORITHM","NETWORK", "SECURITY",
            "PROGRAMMING", "INTERFACE","COMPILER", "DEBUGGER", "VARIABLE", "FUNCTION"
        };

        public HangmanGamePage()
        {
            InitializeComponent();
            StartGame();
        }

        // Method to start a new game
        private void StartGame()
        {
            var random = new Random();
            selectedWord = wordBank[random.Next(wordBank.Length)].ToUpper();

            incorrectGuesses = 0;
            guessedLetters = string.Empty;

            // Initialize the current progress
            currentProgress = new char[selectedWord.Length];
            for (int i = 0; i < selectedWord.Length; i++)
                currentProgress[i] = '_';
             
            // Display the initial progress and hangman image
            WordToGuessDisplay.Text = new string(currentProgress);
            HangmanImage.Source = hangmanStages[incorrectGuesses];
            FeedbackMessage.Text = string.Empty;
        }

        private void OnGuessClicked(object sender, EventArgs e)
        {
            string guessInput = PlayerInput.Text?.ToUpper();
            if (string.IsNullOrEmpty(guessInput) || guessInput.Length != 1 || !char.IsLetter(guessInput[0]))
            {
                FeedbackMessage.Text = "Please enter a valid letter!";
                return;
            }

            char guessedChar = guessInput[0];

            // Check if the letter has already been guessed
            if (guessedLetters.Contains(guessedChar))
            {
                FeedbackMessage.Text = "You already guessed that letter!";
                return;
            }

            guessedLetters += guessedChar;
            bool isCorrect = false;

            for (int i = 0; i < selectedWord.Length; i++)
            {
                if (selectedWord[i] == guessedChar && currentProgress[i] == '_')
                {
                    currentProgress[i] = guessedChar;
                    isCorrect = true;
                }
            }

            WordToGuessDisplay.Text = new string(currentProgress);

            if (!isCorrect)
            {
                incorrectGuesses++;
                HangmanImage.Source = hangmanStages[incorrectGuesses];
            }

            if (incorrectGuesses == hangmanStages.Length - 1)
            {
                FeedbackMessage.Text = $"You died. The word was: {selectedWord}";
                DisableInput();
                return;
            }

            if (new string(currentProgress) == selectedWord)
            {
                FeedbackMessage.Text = "You survived! You guessed the word!";
                DisableInput();
                return;
            }

            FeedbackMessage.Text = $"Guessed letters: {guessedLetters}";
            PlayerInput.Text = string.Empty;
        }

        // Method to disable further input when the game ends
        private void DisableInput()
        {
            PlayerInput.IsEnabled = false;
        }
    }
}
