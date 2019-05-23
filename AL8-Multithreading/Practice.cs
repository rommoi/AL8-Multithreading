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

            DirectoryInfo dinfo = Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + "emails");
            for (int i = 1; i < 51; i++)
            {
                string filePath = dinfo.FullName + $"{i}" + ".txt";
                File.Create(filePath);
                using(StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine($"this is mail with index {i}. Hello!!! blablabla!!! ");
                }

            }
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
        }

        /// <summary>
        /// LA8.P5/5. Асинхронная “отсылка” “письма” с блокировкой вызывающего потока 
        /// и информировании об окончании рассылки (вывод на консоль информации 
        /// удачно ли завершилась отсылка). 
        /// </summary>
        public async static void LA8_P5_5()
        {           
        }
    }    
}
