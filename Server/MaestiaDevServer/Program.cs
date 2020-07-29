using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

using Core;

public class Program
{
    private static Listener _mainListener;

    public static MySqlConnection SQLconnection;

    private static void Main(string[] args)
    {
        Console.Title = "Maestia Server Emulator";

        AppDomain.CurrentDomain.FirstChanceException += FirstChanceException;

        Log.WriteInfo("Starting Maestia Emulator...");

        //string connStr = "server=localhost;user=root;database=maestia;port=3306;password=";
        //SQLconnection = new MySqlConnection(connStr);

        try
        {
            //SQLconnection.Open();

            //Log.WriteInfo("Connection to database successful.");

            _mainListener = new Listener(21001);
            _mainListener.Start();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                foreach (var method in type.GetMethods())
                    foreach (PacketAttribute customAttribute in method.GetCustomAttributes(typeof(PacketAttribute), false))
                        _mainListener.SetHandler(Delegate.CreateDelegate(typeof(Action<Packet, Client>), method) as Action<Packet, Client>, customAttribute.MainId, customAttribute.SubId);

            Process.GetCurrentProcess().WaitForExit();
        }
        catch(Exception ex)
        {
            //Log.WriteError("Could not connect to database!");
            Log.WriteError(ex.ToString());
            Console.ReadKey();
        }
    }

    private static void FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
    {
        Log.WriteError(e.Exception.Message);

        Debug.WriteLine("[{0}] {1}", DateTime.Now.ToLongTimeString(), e.Exception.ToString().Replace("\n", "\n\t\t"));
    }
}
