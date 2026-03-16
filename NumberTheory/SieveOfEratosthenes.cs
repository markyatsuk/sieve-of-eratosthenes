namespace NumberTheory;

public static class SieveOfEratosthenes
{
    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a sequential approach.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersSequentialAlgorithm(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Input must be greater than 0.");
        }

        List<int> primes = [];
        bool[] isPrime = new bool[n + 1];
        for (int i = 2; i <= n; i++)
        {
            isPrime[i] = true;
        }

        for (int i = 2; i * i <= n; i++)
        {
            if (isPrime[i])
            {
                for (int j = i * i; j <= n; j += i)
                {
                    isPrime[j] = false;
                }
            }
        }

        for (var i = 0; i < isPrime.Length; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a modified sequential approach.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersModifiedSequentialAlgorithm(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Input must be greater than 0.");
        }

        List<int> basePrimes = [];
        List<int> primes = [];
        bool[] isPrime = new bool[n + 1];
        int sqrtN = (int)Math.Sqrt(n);

        for (int i = 2; i <= n; i++)
        {
            isPrime[i] = true;
        }

        // Search for prime numbers in the range from 2 to sqrt{n}​ using the classical Sieve of Eratosthenes method (base prime numbers).
        for (int i = 2; i <= sqrtN; i++)
        {
            if (isPrime[i])
            {
                for (int j = i * i; j <= sqrtN; j += i)
                {
                    isPrime[j] = false;
                }

                basePrimes.Add(i);
            }
        }

        foreach (int p in basePrimes)
        {
            int start = Math.Max(p * p, (sqrtN + 1 + p - 1) / p * p);
            for (int j = start; j <= n; j += p)
            {
                isPrime[j] = false;
            }
        }

        for (var i = 0; i < isPrime.Length; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a concurrent approach by data decomposition.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersConcurrentDataDecomposition(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Input must be greater than 0.");
        }

        List<int> basePrimes = [];
        List<int> primes = [];
        bool[] isPrime = new bool[n + 1];
        int sqrtN = (int)Math.Sqrt(n);
        int threadCount = Environment.ProcessorCount;
        int batchSize = (n - sqrtN) / threadCount;
        Thread[] threads = new Thread[threadCount];

        for (int i = 2; i <= n; i++)
        {
            isPrime[i] = true;
        }

        // Search for prime numbers in the range from 2 to sqrt{n}​ using the classical Sieve of Eratosthenes method (base prime numbers).
        for (int i = 2; i <= sqrtN; i++)
        {
            if (isPrime[i])
            {
                for (int j = i * i; j <= sqrtN; j += i)
                {
                    isPrime[j] = false;
                }

                basePrimes.Add(i);
            }
        }

        for (int i = 0; i < threadCount; i++)
        {
            int threadIndex = i;
            threads[threadIndex] = new Thread(() =>
            {
                int start = sqrtN + (threadIndex * batchSize) + 1;
                int end = (threadIndex == threadCount - 1) ? n : start + batchSize;

                foreach (int p in basePrimes)
                {
                    int j = Math.Max(p * p, (sqrtN + 1 + p - 1) / p * p);
                    for (; j <= end; j += p)
                    {
                        isPrime[j] = false;
                    }
                }
            });
        }

        foreach (Thread thread in threads)
        {
            thread.Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        for (var i = 0; i < isPrime.Length; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using a concurrent approach by "basic" primes decomposition.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersConcurrentBasicPrimesDecomposition(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Input must be greater than 0.");
        }

        List<int> basePrimes = [];
        List<int> primes = [];
        bool[] isPrime = new bool[n + 1];
        int sqrtN = (int)Math.Sqrt(n);
        int threadCount = Environment.ProcessorCount;
        Thread[] threads = new Thread[threadCount];

        for (int i = 2; i <= n; i++)
        {
            isPrime[i] = true;
        }

        // Search for prime numbers in the range from 2 to sqrt{n}​ using the classical Sieve of Eratosthenes method (base prime numbers).
        for (int i = 2; i <= sqrtN; i++)
        {
            if (isPrime[i])
            {
                for (int j = i * i; j <= sqrtN; j += i)
                {
                    isPrime[j] = false;
                }

                basePrimes.Add(i);
            }
        }

        int primesPerThread = basePrimes.Count / threadCount;

        for (int i = 0; i < threadCount; i++)
        {
            int threadIndex = i;
            threads[threadIndex] = new Thread(() =>
            {
                int startPrime = threadIndex * primesPerThread;
                int endPrime = (threadIndex == threadCount - 1) ? basePrimes.Count : startPrime + primesPerThread;
                int start = sqrtN + 1;
                int end = n;

                for (int pi = startPrime; pi < endPrime; pi++)
                {
                    int p = basePrimes[pi];
                    int j = Math.Max(p * p, (start + p - 1) / p * p);
                    for (; j <= end; j += p)
                    {
                        isPrime[j] = false;
                    }
                }
            });
        }

        foreach (Thread thread in threads)
        {
            thread.Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        for (var i = 0; i < isPrime.Length; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    /// <summary>
    /// Generates a sequence of prime numbers up to the specified limit using thread pool and signaling construct.
    /// </summary>
    /// <param name="n">The upper limit for generating prime numbers.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing prime numbers up to the specified limit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input <paramref name="n"/> is less than or equal to 0.</exception>
    public static IEnumerable<int> GetPrimeNumbersConcurrentWithThreadPool(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Input must be greater than 0.");
        }

        List<int> basePrimes = [];
        List<int> primes = [];
        bool[] isPrime = new bool[n + 1];
        int sqrtN = (int)Math.Sqrt(n);

        for (int i = 2; i <= n; i++)
        {
            isPrime[i] = true;
        }

        // Search for prime numbers in the range from 2 to sqrt{n}​ using the classical Sieve of Eratosthenes method (base prime numbers).
        for (int i = 2; i <= sqrtN; i++)
        {
            if (isPrime[i])
            {
                for (int j = i * i; j <= sqrtN; j += i)
                {
                    isPrime[j] = false;
                }

                basePrimes.Add(i);
            }
        }

        CountdownEvent countdown = new CountdownEvent(basePrimes.Count);

        foreach (int p in basePrimes)
        {
            int prime = p; // Capture the current value of p for the closure

            _ = ThreadPool.QueueUserWorkItem(_ =>
            {
                var start = Math.Max(prime * prime, (sqrtN + 1 + prime - 1) / prime * prime);
                for (var j = start; j <= n; j += prime)
                {
                    isPrime[j] = false;
                }

                countdown.Signal();
            });
        }

        countdown.Wait();

        for (var i = 0; i < isPrime.Length; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
            }
        }

        return primes;
    }
}
