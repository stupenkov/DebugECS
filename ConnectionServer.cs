using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ECS;

namespace DebugECS
{
	public class ConnectionServer
	{
		private TcpClient _client;
		private bool _connected = false;
		private NetworkStream _networkStream;
		private BinaryFormatter _binaryFormatter = new BinaryFormatter();

		public event EventHandler<List<DataDebug>> DataReceived;

		public int Port { get; } = 8888;

		public string Server { get; } = "127.0.0.1";

		public void Connect()
		{
			Task.Run(() =>
			{
				while (true)
				{
					if (_connected == false)
					{
						TryConnect();
					}

					Thread.Sleep(1000);
				}
			});
		}

		private void TryConnect()
		{
			try
			{
				Console.WriteLine("Попытка подключения.");
				_client = new TcpClient();
				_client.Connect(Server, Port);
				_connected = true;
				_networkStream = _client.GetStream();

				while (_connected)
				{
					ReceiveData();
				};
			}
			catch (SocketException e)
			{
				Console.WriteLine($"Не удалось подключиться к серверу {Server}:{Port}");
			}
		}

		private void ReceiveData()
		{
			try
			{
				List<DataDebug> dataDebugs =(List<DataDebug>)_binaryFormatter.Deserialize(_networkStream);
				DataReceived?.Invoke(this, dataDebugs);
			}
			catch (Exception e)
			{
				Console.WriteLine("Соединение разорвано.");
				_connected = false;
				_networkStream.Close();
				_client.Close();
			}
		}
	}
}
