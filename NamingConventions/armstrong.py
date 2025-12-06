def calculate_armstrong_sum(number):
    digitSum = 0
    digitCount = len(str(number))
    temp = number

    while temp > 0:
        lastDigit = temp % 10
        digitSum += lastDigit ** digitCount
        temp //= 10

    return digitSum

numInput = int(input("Enter a number to check if it is an Armstrong number: "))

if numInput == calculate_armstrong_sum(numInput):
    print(numInput, "is an Armstrong number.")
else:
    print(numInput, "is NOT an Armstrong number.")
