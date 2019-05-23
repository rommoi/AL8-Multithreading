using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Advanced_Lesson_6_Multithreading
{
    class Practice
    {      
        /// <summary>
        /// LA8.P1/X. Написать консольные часы, которые можно останавливать и запускать с 
        /// консоли без перезапуска приложения.
        /// </summary>
        public static void LA8_P1_5()
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(DateTime.Now);
                    Thread.Sleep(1000);
                }
            });

            thread.IsBackground = true;
            thread.Start();

            while (true)
            {
                Console.WriteLine("\nNext command:\n");
                switch (Console.ReadLine())
                {
                    case "start":
                        thread.Resume();

                        break;
                    case "stop":
                        thread.Suspend();
                        break;
                    case "quit":
                        if(thread.ThreadState == ThreadState.Running) thread.Abort();
                        thread = null;
                        return;
                        break;
                }
            }
        }

        /// <summary>
        /// LA8.P2/X. Написать консольное приложение, которое “делает массовую рассылку”. 
        /// </summary>
        public static void LA8_P2_5()
        {

            DirectoryInfo dinfo = Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + "emails\\");
            Random rnd = new Random();

            DateTime start = DateTime.Now;
            for (int i = 1; i < 51; i++)
            {
                string filePath = dinfo.FullName + $"{i}" + ".txt";
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine($"this is mail with index {i}. Hello!!! blablabla!!! ");
                }

                Thread.Sleep(100 + rnd.Next(900));

                // WITHOUT THREADS DIFFERENCE BETWEEN THE FIRST AND THE LAST IS 30 seconds !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            DateTime finish = DateTime.Now;

            Console.WriteLine(finish - start);

            DirectoryInfo dinfoParralel = Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + "emails_parallel\\");

            DateTime startParallel = DateTime.Now;

            for (int i = 1; i < 51; i++)
            {
                string filePath = dinfoParralel.FullName + $"{i}" + ".txt";
                object obj = i;
                ThreadPool.QueueUserWorkItem((s) =>
                {
                    using (StreamWriter swrt = new StreamWriter(filePath))
                    {
                        swrt.WriteLine($"this is mail with index {obj}. Hello!!! blablabla!!! ");
                    }

                    Thread.Sleep(100 + rnd.Next(900));
                }, obj);

                //With parrallel much more faster!!!!!
            }

            DateTime finishParallel = DateTime.Now;

            Console.WriteLine(finishParallel - startParallel);
        }

        /// <summary>
        /// Написать код, который в цикле (10 итераций) эмулирует посещение 
        /// сайта увеличивая на единицу количество посещений для каждой из страниц.  
        /// </summary>
        public static void LA8_P3_5()
        {            
        }

        /// <summary>
        /// LA8.P4/X. Отредактировать приложение по “рассылке” “писем”. 
        /// Сохранять все “тела” “писем” в один файл. Использовать блокировку потоков, чтобы избежать проблем синхронизации.  
        /// </summary>
        public static void LA8_P4_5()
        {
            DirectoryInfo dinfoParralel = Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + "emails_parallel_lock\\");
            object lockObj = new object();

            Random rnd = new Random();

            string filePath = dinfoParralel.FullName + $"parallLocked.txt";
            StreamWriter swrt = new StreamWriter(filePath, true);
            
            DateTime startParallel = DateTime.Now;
            var eveList = new List<ManualResetEvent>();

            for (int i = 1; i < 51; i++)
            {
                object obj = i;
                var eve = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem((s) =>
                {

                    lock (lockObj)
                    {
                        swrt.WriteLine($"this is mail with index {obj}. Hello!!! blablabla!!! ");
                        swrt.WriteLine();
                        swrt.Flush();
                    }
                    eve.Set();
                    //Thread.Sleep(100 + rnd.Next(900));
                }, obj);
                eveList.Add(eve);
                
            }
            
            DateTime finishParallel = DateTime.Now;

            Console.WriteLine(finishParallel - startParallel);

            WaitHandle.WaitAll(eveList.ToArray());
            //Thread.Sleep(1000);
            swrt.Dispose();
        }

        /// <summary>
        /// LA8.P5/5. Асинхронная “отсылка” “письма” с блокировкой вызывающего потока 
        /// и информировании об окончании рассылки (вывод на консоль информации 
        /// удачно ли завершилась отсылка). 
        /// </summary>
        public async static void LA8_P5_5()
        {
            DirectoryInfo dinfoParralel = Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + "await_email\\");
            
            string filePath = dinfoParralel.FullName + $"awaitemail.txt";

            using(StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine("Hello! this is async email!!! blablabla... ");
                sw.WriteLine();
            }

            bool result;
            using (StreamReader sr = new StreamReader(filePath))
            {
                result = await SmptServer.SendEmail(sr.ReadToEnd());
            }
            Console.WriteLine(result);
        }
    }    

}
