"""
PROBLEMS:
- Has 3 responsibilities: validation, database save, and file backup
- Hard to test and maintain

SOLUTION: Split into separate functions with single responsibilities
"""


# Function 1: Responsible ONLY for validation
def validate_user(user):
    if user["name"] == "" or user["email"] == "":
        print("Invalid user data")
        return False
    return True


# Function 2: Responsible ONLY for database operations
def save_user_to_database(user):
    db.insert("Users", user)
    print(f"User {user['name']} saved to database")


# Function 3: Responsible ONLY for file backup
def backup_user_to_file(user):
    file_path = f"/backup/users/{user['id']}.txt"
    writeToFile(file_path, user)
    print(f"User {user['name']} backed up to {file_path}")


# Main function: Coordinates the operations
def save_user(user):
    if not validate_user(user):
        return
    
    save_user_to_database(user)
    backup_user_to_file(user)


