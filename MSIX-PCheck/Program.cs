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
        static bool is_debug = false;

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
                        case "debug":
                            is_debug = true;
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
            Console.WriteLine(@"file|exec\read\new\delete\write|path");
            Console.WriteLine(@"reg|get\set|root|key|value|data");
        }

        static void debug(string log)
        {
            if (is_debug)
            {
                Console.WriteLine(log);
            }
        }

        static void file(string[] args){
            string operation = args[1];
            string path = args[2];
            switch(operation){
                case "exec":
                    Process process = Process.Start(path);
                    process.WaitForExit();
                    debug(string.Format("Process {0} Exit Code {1}", path, process.ExitCode));
                    break;
                case "read":
                    string text = File.ReadAllText(path);
                    debug(string.Format("File {0} Text Content {1}", path, text));
                    break;
                case "new":
                    File.Create(path).Dispose();
                    bool added = false;
                    if(File.Exists(path)){
                        added = true;
                    }
                    debug(string.Format("File {0} Create Status {1}", path, added));
                    break;
                case "delete":
                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException();
                    }
                    File.Delete(path);
                    bool deleted = false;
                    if(!File.Exists(path)){
                        deleted = true;
                    }
                    debug(string.Format("File {0} Delete Status {1}", path, deleted));
                    break;
                case "write":
                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException();
                    }
                    string test = Guid.NewGuid().ToString();
                    File.AppendAllText(path, test);
                    bool appended = false;
                    if(File.ReadAllText(path).EndsWith(test))
                    {
                        appended = true;
                    }
                    debug(string.Format("File {0} Write Status {1}", path, appended));
                    break;
                default:
                    break;
            }
        }

        static void reg(string[] args){
            string operation = args[1];
            string reg_root = args[2];
            string reg_key = args[3];
            string reg_value = args[4];
            string reg_data = args[5];

            RegistryKey root;
            switch (reg_root)
            {
                case "HKEY_LOCAL_MACHINE":
                    root = Registry.LocalMachine;
                    break;
                case "HKEY_CURRENT_USER":
                    root = Registry.CurrentUser;
                    break;
                case "HKEY_CLASSES_ROOT":
                    root = Registry.ClassesRoot;
                    break;
                case "HKEY_USERS":
                    root = Registry.Users;
                    break;
                case "HKEY_CURRENT_CONFIG":
                    root = Registry.CurrentConfig;
                    break;
                default:
                    root = Registry.CurrentUser;
                    break;
            }
            switch(operation){
                case "get":
                    RegistryKey get_key = root.OpenSubKey(reg_key, false);
                    var result = get_key.GetValue(reg_value);
                    debug(string.Format(@"Reg {0}\{1} {2} Get {3}", reg_root, reg_key, reg_value, result));
                    break;
                case "set":
                    RegistryKey set_key = root.OpenSubKey(reg_key, true);
                    set_key.SetValue(reg_value, reg_data);
                    debug(string.Format(@"Reg {0}\{1} {2} Set {3}", reg_root, reg_key, reg_value, reg_data));
                    break;
                default:
                    break;
            }
        }
    }
}
