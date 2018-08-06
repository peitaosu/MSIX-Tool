using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Win32;

namespace MSIX_PCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true){
                try{
                    string input = Console.ReadLine();
                    string[] command = input.Split('|');
                    if(command[0] == "exit")
                        break;
                    switch(command[0]){
                        case "file":
                            file(command);
                            break;
                        case "reg":
                            reg(command);
                            break;
                        case "info":
                            info();
                            break;
                        default:
                            break;
                    }
                }catch(Exception e)
                {
                    Console.WriteLine("[ERROR] {0}", e.Message);
                }
            }
        }

        static void info()
        {
            Console.WriteLine(@"file|exec\read\new\del|path");
            Console.WriteLine(@"reg|get\set|key|value|data");
        }

        static void file(string[] args){
            string operation = args[1];
            string path = args[2];
            switch(operation){
                case "exec":
                    Process process = Process.Start(path);
                    process.WaitForExit();
                    Console.WriteLine("Process {0} Exit Code {1}", path, process.ExitCode);
                    break;
                case "read":
                    string text = File.ReadAllText(path);
                    Console.WriteLine("File {0} Text Content {1}", path, text);
                    break;
                case "new":
                    File.Create(path).Dispose();
                    bool added = false;
                    if(File.Exists(path)){
                        added = true;
                    }
                    Console.WriteLine("File {0} Create Status {1}", path, added);
                    break;
                case "del":
                    File.Delete(path);
                    bool deleted = false;
                    if(!File.Exists(path)){
                        deleted = true;
                    }
                    Console.WriteLine("File {0} Delete Status {1}", path, deleted);
                    break;
                case "write":
                    StreamWriter sw = File.AppendText(path);
                    string test = Guid.NewGuid().ToString();
                    sw.WriteLine(test);
                    bool appended = false;
                    if(File.ReadLines(path).Last() == test)
                    {
                        appended = true;
                    }
                    Console.WriteLine("File {0} Write Status {1}", path, appended);
                    break;
                default:
                    break;
            }
        }

        static void reg(string[] args){
            string operation = args[1];
            string reg_key = args[2];
            string reg_value = args[3];
            string reg_data = args[4];
            switch(operation){
                case "get":
                    var result = Registry.GetValue(reg_key, reg_value, null);
                    Console.WriteLine("Reg {0} {1} Get {2}", reg_key, reg_value, result);
                    break;
                case "set":
                    Registry.SetValue(reg_key, reg_value, reg_data);
                    Console.WriteLine("Reg {0} {1} Set {2}", reg_key, reg_value, reg_data);
                    break;
                default:
                    break;
            }
        }
    }
}
