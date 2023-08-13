using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_Thread
{
    //재귀적 락을 허용할지(no)
    //재귀적 락을 허용하지 않으면 WriteThreadld를 bool값으로 넣어도 상관없음
    //스핀락 정책(5000번 -> Yield)
    class Lock
    {
        const int EMPTY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7FFF0000;
        const int READ_MASK = 0x0000FFFF;
        const int MAX_SPIN_COUNT = 5000;

        //[Unused(1)] [WriteThreadld(15)] [ReadCount(16)]
        int _flag = EMPTY_FLAG;
        int _writeCount = 0;        //재귀적 락을 허용할 때

        public void WriteLock()
        {
            //동일 쓰레드가 WriteLock을 이미 획득하고 있는지 확인
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if(Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                _writeCount++;
                return;
            }

            //아무도 WriteLock or ReadLock을 획득하고 있지 않을 때, 경합해서 소유권을 얻는다
            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;
            while (true)
            {
                for(int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    if(Interlocked.CompareExchange(ref _flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
                    {
                        _writeCount = 1;
                        return;
                    }
                        
                    //시도를 해서 성공하면 return
                    //WriteThreadld를 자신의 ThreadID로 채우기
                    if (_flag == EMPTY_FLAG)
                        _flag = desired;
                }

                Thread.Yield();
            }
        }
        public void WriteUnLock()
        {
            //WriteThreadld를 찾을때 _writeCount를 모두 비우고 돌려줌
            int lockCount = --_writeCount;
            if(lockCount == 0)
                Interlocked.Exchange(ref _flag, EMPTY_FLAG);
        }

        public void ReadLock()
        {
            //동일 쓰레드가 WriteLock을 이미 획득하고 있는지 확인
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                Interlocked.Increment(ref _flag);
                return;

            }
            //아무도 WriteLock을 획득하고 있지 않으면 Read Count를 1늘린다
            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    int expected = (_flag & READ_MASK);
                    //expected = 예상한 값, expected + 1 = 원하는 값 => 이게 일치 할때까지 순환
                    if (Interlocked.CompareExchange(ref _flag, expected + 1, expected) == expected)
                        return;                   
                }

                Thread.Yield();
            }
        }
        public void ReadUnLock()
        {
            Interlocked.Decrement(ref _flag);
        }

    }
}
