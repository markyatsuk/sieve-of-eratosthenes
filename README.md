Sieve of Eratosthenes (Parallel Implementation)
📌 Description

This project implements the Sieve of Eratosthenes algorithm for finding prime numbers, including both sequential and parallel approaches.

🧠 How it works

The algorithm is divided into two main stages:

1. Base prime calculation

Prime numbers are calculated in the range from 2 to √n using the classical Sieve of Eratosthenes.

2. Parallel processing

The remaining range (√n to n) is processed using parallel algorithms:

Splitting the range across threads
Assigning subsets of primes to threads
Using a thread pool for dynamic task execution
