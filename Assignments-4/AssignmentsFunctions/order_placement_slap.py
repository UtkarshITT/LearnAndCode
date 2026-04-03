"""
PROBLEM: 
- Mixes high-level abstractions with low-level implementation details
- Some operations are function calls, others are direct calculations
- Violates Single Level of Abstraction Principle

SOLUTION: 
- Extract all low-level details into separate functions
- Keep all operations in placeOrder() at the same level of abstraction
"""


# Configuration constants
DISCOUNT_RATE = 0.10
TAX_RATE = 0.18


def is_order_invalid(order_id):
    return order_id <= 0


def show_invalid_order_message():
    print("Order cannot be empty")



def calculate_discount(order_amount):
    return order_amount * DISCOUNT_RATE


def calculate_tax(order_amount):
    return order_amount * TAX_RATE


def calculate_final_amount(order_amount, tax, discount):
    return (order_amount + tax) - discount



def save_order(order_id, final_amount):
    print(f"Order {order_id} saved in database with amount ${final_amount:.2f}")


def show_success_message():
    print("Order placed successfully")



def place_order(order_id, order_amount):
    if is_order_invalid(order_id):
        show_invalid_order_message()
        return
    
    discount_amount = calculate_discount(order_amount)
    tax_amount = calculate_tax(order_amount)
    final_amount = calculate_final_amount(order_amount, tax_amount, discount_amount)

    save_order(order_id, final_amount)

    show_success_message()


def main():
    
    print("=" * 70)
    print("Order Placement System - Single Level of Abstraction Demonstration")
    print("=" * 70)
    
    # Test Case 1: Valid order
    print("\n--- Test Case 1: Valid Order ---")
    place_order(order_id=101, order_amount=1000)
    
    # Test Case 2: Invalid order (ID <= 0)
    print("\n--- Test Case 2: Invalid Order ---")
    place_order(order_id=0, order_amount=500)
    
    # Test Case 3: Another valid order
    print("\n--- Test Case 3: Another Valid Order ---")
    place_order(order_id=202, order_amount=2500)
    
    print("\n" + "=" * 70)
    print("SLAP Benefits:")
    print("- All operations in place_order() are at the same abstraction level")
    print("- Function reads like a high-level workflow")
    print("- Easy to understand what's happening without diving into details")
    print("- Each detail is encapsulated in its own appropriately named function")
    print("=" * 70)


if __name__ == "__main__":
    main()
