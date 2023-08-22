using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ServerCore;

namespace Server
{
    struct JobTimerElem : IComparable<JobTimerElem>
    {
        public int execTick; //실행시간
        public Action action;
        public int CompareTo(JobTimerElem other)
        {
            return other.execTick - execTick;
        }
    }
    class JobTimer
    {
        PriorityQueue<JobTimerElem> _pq = new PriorityQueue<JobTimerElem>();
        object _lock = new object();

        public static JobTimer Instance { get; } = new JobTimer();

        public void Push(Action action, int tickAfter = 0)
        {
            JobTimerElem job;
            //실행하는 타이밍
            job.execTick = System.Environment.TickCount + tickAfter;
            job.action = action;

            lock(_lock)
            {
                _pq.Push(job);
            }
        }

        public void Flush()
        {
            while(true)
            {
                int now = System.Environment.TickCount;

                JobTimerElem job;

                lock(_lock)
                {
                    //while 탈출
                    if (_pq.Count == 0)
                        break;

                    //엿보기
                    job = _pq.Peek();
                    if (job.execTick > now)
                        break;

                    _pq.Pop();
                }

                job.action.Invoke();
            }
        }
    }
}
