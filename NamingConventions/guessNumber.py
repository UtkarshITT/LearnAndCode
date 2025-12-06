import random

def isValidGuess(userGuess):
    return userGuess.isdigit() and 1 <= int(userGuess) <= 100

def main():
    correctNumber = random.randint(1, 100)
    guessedCorrectly = False
    attempts = 0

    guess = input("Guess a number between 1 and 100: ")

    while not guessedCorrectly:
        if not isValidGuess(guess):
            guess = input("Invalid! Please enter a number between 1 and 100: ")
            continue
        else:
            attempts += 1
            guess = int(guess)

        if guess < correctNumber:
            guess = input("Too low! Try again: ")
        elif guess > correctNumber:
            guess = input("Too high! Try again: ")
        else:
            print("Correct! You guessed it in", attempts, "attempts.")
            guessedCorrectly = True

main()
