using System;

namespace BasicCS.Chapter08
{
    internal class Interfaces
    {
        static void Main()
        {
            // 인터페이스를 통한 다형성 (Polymorphism via interfaces)
            ILogger consoleLogger = new ConsoleLogger();
            ILogger fileLogger = new FileLogger();

            var runner = new Runner(consoleLogger);
            runner.Run("Task 1");

            runner.SetLogger(fileLogger);
            runner.Run("Task 2");

            // 여러 인터페이스 구현 (Multiple interface implementation)
            var car = new Car();
            car.Start();
            car.Stop();
            Console.WriteLine($"Can honk: {car is IHorn}");

            // IDisposable 예제
            using var reader = new FileReader("example.txt");
            reader.Read();

            // Default interface method (C# 8.0+)
            ISpeak english = new EnglishSpeaker();
            ISpeak korean = new KoreanSpeaker();
            english.SayHello();
            korean.SayHello();
        }
    }

    // 기본 인터페이스 (Basic interface)
    interface ILogger
    {
        void Log(string message);
    }

    class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"[Console] {message}");
    }

    class FileLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"[File]   {message}");
    }

    // 의존성 주입 (Dependency injection 예시)
    class Runner
    {
        private ILogger _logger;

        public Runner(ILogger logger) => _logger = logger;

        public void SetLogger(ILogger logger) => _logger = logger;

        public void Run(string taskName)
        {
            _logger.Log($"Running: {taskName}");
        }
    }

    // 여러 인터페이스 구현 (Multiple interfaces)
    interface IEngine
    {
        void Start();
        void Stop();
    }

    interface IHorn
    {
        void Honk();
    }

    class Car : IEngine, IHorn, IDisposable
    {
        public void Start() => Console.WriteLine("Engine started");
        public void Stop() => Console.WriteLine("Engine stopped");
        public void Honk() => Console.WriteLine("Beep beep!");

        public void Dispose() => Console.WriteLine("Car disposed");
    }

    // IDisposable 예제
    class FileReader : IDisposable
    {
        private readonly string _path;
        private bool _disposed;

        public FileReader(string path) => _path = path;

        public void Read() => Console.WriteLine($"Reading file: {_path}");

        public void Dispose()
        {
            if (!_disposed)
            {
                Console.WriteLine($"Closing file: {_path}");
                _disposed = true;
            }
        }
    }

    // Default interface method (C# 8.0+)
    interface ISpeak
    {
        void SayHello()
        {
            Console.WriteLine("Hello!");  // default implementation
        }
    }

    class EnglishSpeaker : ISpeak { }

    class KoreanSpeaker : ISpeak
    {
        public void SayHello() => Console.WriteLine("안녕하세요!");
    }
}
