using System;
using System.IO;
using System.Threading;

namespace RetrykafkaConsumer
{
    public struct Measurement
    {
        public string TaskName { get; set; }
        public long ElapsedMs { get; set; }
        public bool IsBackground { get; set; }
        public bool IsThreadPoolThread { get; set; }
        public int ManagedThreadId { get; set; }
        public bool IsExThrown { get; set; }
    }

    public static class Logger
    {
        private static readonly string path = Path.Combine(Environment
                    .GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Projects/try-stuff/RetrykafkaConsumer/log.txt");

        private static readonly object lockObj = new object();

        public static void LogThreadInfo(string taskName)
        {
            lock (lockObj)
            {
                var thread = Thread.CurrentThread;

                // TODO: maybe use {DateTimeOffset.Now}
                var msg = $"\n{DateTimeOffset.Now}" +
                    $"\n{taskName}\n" +
                    $"Background: {thread.IsBackground}\n" +
                    $"Thread Pool: {thread.IsThreadPoolThread}\n" +
                    $"Thread ID: {thread.ManagedThreadId}\n";

                using StreamWriter outputFile = new StreamWriter(path, true);

                outputFile.Write(msg);

                Console.WriteLine(msg);
            }
        }

        public static void LogInfo(string msg)
        {
            using StreamWriter outputFile = new StreamWriter(path, true);

            var withTimestamp = $"\n{DateTime.Now:hh.mm.ss.ffffff} ms\n" + msg;

            outputFile.Write(withTimestamp);

            Console.WriteLine(withTimestamp);
        }

        public static void ShowAvailableThreadInfo()
        {
            ThreadPool.GetAvailableThreads(out int worker, out int io);

            Console.WriteLine("Thread pool threads available at startup: ");
            Console.WriteLine("Worker threads: {0:N0}", worker);
            Console.WriteLine("Asynchronous I/O threads: {0:N0}", io);
        }
    }
}
