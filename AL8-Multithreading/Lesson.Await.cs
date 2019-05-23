using Advanced_Lesson_6_Diagnostic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Advanced_Lesson_6_Multithreading
{
    public static partial class Lesson
    {
        public static dynamic myTextBox = new
        {
            Text = ""
        };

        //Текущий поток блокируется
        public static void AwaitThreadPlayer()
        {
            var player = new AwaitThreadPlayer();
            var thrd = player.Load(new string[5]);
            //thrd.Join();
            player.Play();
        }

        
        //Текущий поток блокируется
        public static void AwaitTaskPlayerExample()
        {
            var player = new AwaitTaskPlayer();
            var task = player.Load("folder");
            task.Wait();
            player.Play();                
        }

        //Текущий поток не блокируется
        public static void AwaitTaskPlayerExample2()
        {
            var c1 = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var player = new AwaitTaskPlayer();
            var task = player.Load("folder");
            task.ContinueWith((t) =>
            {
                var c2 = SynchronizationContext.Current;
                player.Play();
            });           
        }

        
        public static async Task AsyncAwaitTaskPlayerExample()
        {
            var c1 = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var player = new AwaitTaskPlayer();            
            await player.Load("folder");
            await player.Play();
        }

        public static void SynchronizationContextExample()
        {
            SynchronizationContext originalContext = SynchronizationContext.Current;
            ThreadPool.QueueUserWorkItem(delegate
            {
                string text = File.ReadAllText(@"c:\temp\log.txt");
                originalContext.Post(delegate
                {
                    myTextBox.Text = text;
                }, null);
            });
        }

        public static async void SC_async_await()
        {
            await SomethingAsync();

            RestOfMethod();
        }

        public static void SC_async_await_under_hood()
        {
            var task = SomethingAsync();
            var currentSyncContext = SynchronizationContext.Current;

            task.ContinueWith(delegate
            {
                if (currentSyncContext == null) RestOfMethod();
                else currentSyncContext.Post(delegate { RestOfMethod(); }, null);

            }, TaskScheduler.Current);
        }

        private static async Task SomethingAsync()
        {
            throw new NotImplementedException();
        }

        private static void RestOfMethod()
        {
            throw new NotImplementedException();
        }
    }

    public class AwaitThreadPlayer
    {
        public Thread Load(string[] songs)
        {
            var thread = new Thread(() =>
            {
                for (int i = 0; i < songs.Length; i++)
                {
                    Console.WriteLine($"#--> Song #{i + 1} loading");
                    Thread.Sleep(1000);
                }
            });

            thread.Start();

            return thread;
        }

        public void Play()
        {
            Console.WriteLine("Player is playing...");
        }
    }
    

    public class AwaitTaskPlayer
    {
        public string[] Songs { get; set; }

        public Task Play()
        {
            var c3 = SynchronizationContext.Current;
            return Task.Run(() =>
            {
                var c4 = SynchronizationContext.Current;
                Console.WriteLine("Playing...");
            });
        }

        public Task Load(string folder)
        {
            var c3 = SynchronizationContext.Current;
            return Task.Run(() =>
            {
                var c4 = SynchronizationContext.Current;
                Console.WriteLine($"Loading songs from {folder} ...");
                this.Songs = new string[5];
            });
        }
    }

    public class AsyncAwaitTaskPlayer
    {
        public string[] Songs { get; set; }

        public Task Play()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Playing...");
            });
        }

        public Task Load(string folder)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Loading songs from {folder} ...");
                this.Songs = new string[5];
            });
        }
    } 
}
