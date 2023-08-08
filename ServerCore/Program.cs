﻿using System.Threading;

namespace ServerCore
{
    class Program
    {
        #region 컴파일러 최적화
        ////volatile 휘발성 메모리 => 권장하지 않음   
        //volatile static bool _stop = false;
        //static void ThreadMain()
        //{
        //    Console.WriteLine("쓰레드 시작!");

        //    while(_stop == false)
        //    {
        //        //누군가가 stop 신호 해주길 기다림
        //    }

        //    Console.WriteLine("쓰레드 종료!");
        //}
        //static void Main(string[] args)
        //{
        //    Task t = new Task(ThreadMain);
        //    t.Start();

        //    //1초 동안 대기
        //    Thread.Sleep(1000);

        //    _stop = true;

        //    Console.WriteLine("Stop 호출");
        //    Console.WriteLine("종료 대기중");

        //    //종료
        //    t.Wait();

        //    Console.WriteLine("종료 성공");
        //}
        #endregion

        #region 캐시 이론
        //static void Main(string[] args)
        //{
        //    int[,] arr = new int[10000, 10000];

        //    {
        //        long now = DateTime.Now.Ticks;
        //        for (int y = 0; y < 10000; y++)
        //            for (int x = 0; x < 10000; x++)
        //                arr[y, x] = 1;
        //        long end = DateTime.Now.Ticks;
        //        Console.WriteLine($"[y, x] 순서 걸린 시간 {end - now}");
        //    }

        //    {
        //        long now = DateTime.Now.Ticks;
        //        for (int y = 0; y < 10000; y++)
        //            for (int x = 0; x < 10000; x++)
        //                arr[x, y] = 1;
        //        long end = DateTime.Now.Ticks;
        //        Console.WriteLine($"[x, y] 순서 걸린 시간 {end - now}");
        //    }
        //}
        #endregion

        #region 메모리 배리어
        //A) 코드 재배치 억제
        //B) 가시성

        //1)Full Memory Barrier(ASM MFENCE, Thread.MemoryBarrier()) : Store/Load 둘다 막는다
        //2)Store Memory Barrier(ASM SFENCE) : Store만 막는다
        //3)Load Memory Barrier(ASM LFENCE) : Load만 막는다

        //static int x = 0;
        //static int y = 0;
        //static int r1 = 0;
        //static int r2 = 0;

        //static void Thread_1()
        //{
        //    y = 1;  //Store y

        //    Thread.MemoryBarrier();

        //    r1 = x; //Load x
        //}
        //static void Thread_2()
        //{
        //    x = 1;  //Store x

        //    Thread.MemoryBarrier();

        //    r2 = y; //Load y
        //}
        //static void Main(string[] args)
        //{
        //    int count = 0;
        //    while (true)
        //    {
        //        count++;
        //        x = y = r1 = r2 = 0;

        //        Task t1 = new Task(Thread_1);
        //        Task t2 = new Task(Thread_2);
        //        t1.Start();
        //        t2.Start();

        //        Task.WaitAll(t1, t2);

        //        if (r1 == 0 && r2 == 0)
        //            break;
        //    }

        //    Console.WriteLine($"{count}번째 에서 종료!");
        //}
        #endregion

        #region Interlocked

        //1) 원자성 보장
        //2) 순서 보장
        //static int number = 0;

        //static void Thread_1()
        //{

        //    for(int i =0; i < 100000; i++)
        //    {
        //        Interlocked.Increment(ref number);
        //    }

        //}

        //static void Thread_2()
        //{
        //    for (int i = 0; i < 100000; i++)
        //    {
        //        Interlocked.Decrement(ref number);
        //    }

        //}

        //static void Main(string[] args)
        //{
        //    Task t1 = new Task(Thread_1);
        //    Task t2 = new Task(Thread_2);
        //    t1.Start();
        //    t2.Start();

        //    Task.WaitAll(t1, t2);

        //    Console.WriteLine(number);
        //}
        #endregion

        #region Lock
        //static int number = 0;
        //static object _obj = new object();

        //static void Thread_1()
        //{

        //    for (int i = 0; i < 100000; i++)
        //    {
        //        //상호배제 Mutual Exclusive
        //        //1) 싱글쓰레드 처럼 사용할 수 있음
        //        //2)Exit를 해주지 않으면 DeadLock상황이 나타날 수 있음
        //        //3)해결방법 -> try - catch 문 활용

        //        //Monitor.Enter(_obj); //문을 잠구는 행위
        //        //number++;
        //        //Monitor.Exit(_obj); //잠금을 풀어준다

        //        lock(_obj)
        //        {
        //            number++;
        //        }
        //    }

        //}

        //static void Thread_2()
        //{
        //    lock (_obj)
        //    {
        //        number--;
        //    }

        //}

        //static void Main(string[] args)
        //{
        //    Task t1 = new Task(Thread_1);
        //    Task t2 = new Task(Thread_2);
        //    t1.Start();
        //    t2.Start();

        //    Task.WaitAll(t1, t2);

        //    Console.WriteLine(number);
        //}
        #endregion
    }
}