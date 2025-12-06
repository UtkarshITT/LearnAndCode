import random

def roll_dice(max_value):
    return random.randint(1, max_value)

def main():
    diceSides = 6
    isRolling = True

    while isRolling:
        userInput = input("Ready to roll? Press ENTER to roll or Q to Quit: ")

        if userInput.lower() != "q":
            rolledNumber = roll_dice(diceSides)
            print("You rolled a", rolledNumber)
        else:
            isRolling = False

main()
