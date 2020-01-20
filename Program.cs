using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using ECS;
using ECS.Drawing;

namespace DebugECS
{
	class Program
	{
		private const int port = 8888;
		private const string server = "127.0.0.1";

		static void Main(string[] args)
		{
			ConnectionServer connection = new ConnectionServer();
			connection.DataReceived += Connection_DataReceived;
			connection.Connect();
			while (true) 
			{
			}
		}

		private static void Connection_DataReceived(object sender, List<DataDebug> e)
		{
			Console.Clear();
			foreach (var item in e)
			{
				Console.BackgroundColor = ConsoleColor.Yellow;
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine(item.Entity);
				foreach (var i in item.EntityComponents)
				{
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine("\t" + i.Key + "  ");
					foreach (var v in i.Value)
					{
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine("\t\t" + v + " ");
					}
				}
			}
		}
	}
}
