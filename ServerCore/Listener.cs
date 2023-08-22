using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Listener
    {
        Socket _listenSocket;
        Func<Session> _sessionFactory;

        //문지기 생성
        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory, int register = 10, int backlog = 100)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory += sessionFactory;

            //문지기 교육
            _listenSocket.Bind(endPoint);

            //영업 시작
            //동시 접속자 수가 많을때 예약을 많이 만든다
            //backlog : 최대 대기수
            _listenSocket.Listen(backlog);

            for (int i = 0; i < register; i ++)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                //OnAcceptCompleted도 형식에 맞춰서 작성해줘야 한다
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
                //최초 등록
                RegisterAccept(args);
            }
           
           
        }

        //예약 함수
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            //재사용하기 위해 기존의 정보 초기화
            args.AcceptSocket = null;

            //AcceptAsync : 비동기 함수
            bool pending = _listenSocket.AcceptAsync(args);
            if (pending == false)
                OnAcceptCompleted(null, args);
        }
        //별도의 스레드가 생성되어 동시에 실행 된다 -> 멀티 쓰레드문제를 생각하면서 만들어야 한다!
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            //return한 후 들어오는 것 처리
            if(args.SocketError == SocketError.Success)
            {
                Session session = _sessionFactory.Invoke();
                session.Start(args.AcceptSocket);
                session.OnConnected(args.AcceptSocket.RemoteEndPoint);    
            }
            else
                Console.WriteLine(args.SocketError.ToString());
            
            //다음을 위해 다시 등록
            RegisterAccept(args);
        }

        ////손님 입장
        //public Socket Accept()
        //{          
        //    //블로킹 계열 함수는 피해야 한다 : 입장요청을 주기 전까지 무한 대기를 하기 때문
        //    return _listenSocket.Accept();
        //}
    }
}
