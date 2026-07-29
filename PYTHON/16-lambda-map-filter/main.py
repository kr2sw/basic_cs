from functools import reduce


if __name__ == "__main__":
    numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

    # map
    squares = list(map(lambda x: x ** 2, numbers))
    cubes = list(map(lambda x: x ** 3, numbers))
    print("Squares:", squares)
    print("Cubes:", cubes)

    # filter
    evens = list(filter(lambda x: x % 2 == 0, numbers))
    greater_than_5 = list(filter(lambda x: x > 5, numbers))
    print("Evens:", evens)
    print("Greater than 5:", greater_than_5)

    # reduce
    sum_all = reduce(lambda a, b: a + b, numbers)
    product = reduce(lambda a, b: a * b, numbers)
    max_val = reduce(lambda a, b: a if a > b else b, numbers)
    print("Sum:", sum_all)
    print("Product:", product)
    print("Max:", max_val)

    # sorted with key
    words = ["banana", "apple", "cherry", "date"]
    sorted_by_len = sorted(words, key=lambda w: len(w))
    sorted_by_last = sorted(words, key=lambda w: w[-1])
    print("Sorted by length:", sorted_by_len)
    print("Sorted by last char:", sorted_by_last)

    # Practical: sort dict by value
    scores = {"Alice": 85, "Bob": 72, "Charlie": 95, "Diana": 88}
    by_score = sorted(scores.items(), key=lambda item: item[1], reverse=True)
    print("Ranking:", by_score)

    # map with multiple iterables
    nums1 = [1, 2, 3, 4]
    nums2 = [10, 20, 30, 40]
    sums = list(map(lambda a, b: a + b, nums1, nums2))
    print("Element-wise sum:", sums)
