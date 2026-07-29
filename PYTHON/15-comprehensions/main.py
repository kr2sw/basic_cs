if __name__ == "__main__":
    numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

    # List comprehension
    squares = [n ** 2 for n in numbers]
    evens = [n for n in numbers if n % 2 == 0]
    even_squares = [n ** 2 for n in numbers if n % 2 == 0]

    print("Squares:", squares)
    print("Evens:", evens)
    print("Even squares:", even_squares)

    # Dict comprehension
    square_dict = {n: n ** 2 for n in numbers}
    even_square_dict = {n: n ** 2 for n in numbers if n % 2 == 0}
    print("Square dict:", square_dict)
    print("Even square dict:", even_square_dict)

    # Set comprehension
    duplicates = [1, 2, 2, 3, 3, 3, 4, 5, 5]
    unique_squares = {n ** 2 for n in duplicates}
    print("Unique squares:", unique_squares)

    # Nested comprehension
    matrix = [[1, 2, 3], [4, 5, 6], [7, 8, 9]]
    flat = [x for row in matrix for x in row]
    transposed = [[row[i] for row in matrix] for i in range(3)]
    print("Flat:", flat)
    print("Transposed:", transposed)

    # Conditional (if-else in comprehension)
    labels = ["even" if n % 2 == 0 else "odd" for n in range(1, 11)]
    print("Labels:", labels)

    # String comprehension
    text = "Hello World"
    upper_chars = [c.upper() for c in text if c.isalpha()]
    print("Upper chars:", upper_chars)
