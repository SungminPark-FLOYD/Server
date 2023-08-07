namespace ServerCore
{
    class Program
    {
        //volatile 휘발성 메모리 => 권장하지 않음
        volatile static bool _stop = false;
        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작!");

            while(_stop == false)
            {
                //누군가가 stop 신호 해주길 기다림
            }

            Console.WriteLine("쓰레드 종료!");
        }
        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain);
            t.Start();

            //1초 동안 대기
            Thread.Sleep(1000);

            _stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기중");

            //종료
            t.Wait();

            Console.WriteLine("종료 성공");
        }
    }
}