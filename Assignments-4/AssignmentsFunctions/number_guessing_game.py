import random


# Constants for game configuration
MIN_NUMBER = 1
MAX_NUMBER = 100


def is_valid_guess(guess_input: str, min_value: int = MIN_NUMBER, max_value: int = MAX_NUMBER) -> bool:
    if guess_input.isdigit():
        number = int(guess_input)
        return min_value <= number <= max_value
    return False


def get_valid_guess(prompt_message: str) -> int:
    while True:
        user_input = input(prompt_message)
        if is_valid_guess(user_input):
            return int(user_input)
        else:
            print(f"Invalid input! Please enter a number between {MIN_NUMBER} and {MAX_NUMBER}.")


def generate_random_number(min_value: int = MIN_NUMBER, max_value: int = MAX_NUMBER) -> int:
    return random.randint(min_value, max_value)


def evaluate_guess(guess: int, target_number: int) -> str:
    if guess < target_number:
        return 'too_low'
    elif guess > target_number:
        return 'too_high'
    else:
        return 'correct'


def display_feedback(evaluation_result: str) -> str:
    feedback_messages = {
        'too_low': "Too low. Guess again: ",
        'too_high': "Too high. Guess again: "
    }
    return feedback_messages.get(evaluation_result, "")


def play_guessing_game() -> None:
    target_number = generate_random_number()
    
    number_of_guesses = 0
    game_won = False
    
    current_guess = get_valid_guess(f"Guess a number between {MIN_NUMBER} and {MAX_NUMBER}: ")
    
    while not game_won:
        number_of_guesses += 1
        
        result = evaluate_guess(current_guess, target_number)
        
        if result == 'correct':
            print(f"You guessed it in {number_of_guesses} guesses!")
            game_won = True
        else:
            feedback_prompt = display_feedback(result)
            current_guess = get_valid_guess(feedback_prompt)


def main() -> None:
    play_guessing_game()


if __name__ == "__main__":
    main()
