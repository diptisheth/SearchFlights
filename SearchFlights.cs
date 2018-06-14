using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace SearchFlights
{
    class SearchFlights
    {
        class flightData
        {
            public string origin { get; set; }
            public string departureTime { get; set; }
            public string destination { get; set; }
            public string destinationTime { get; set; }
            public decimal price { get; set; }
        }

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
               
                Console.WriteLine("origin:" + args[0]);
                Console.WriteLine("destination:" + args[1]);
                Console.WriteLine();

               List<flightData> fData = new List<flightData>();

                //string[] files = Directory.GetFiles(@"C:\Dipti\SearchFlights\SearchFlights", "*.txt", SearchOption.TopDirectoryOnly);

                //used when executing application from VS
               //string[] files = Directory.GetFiles(@"../../", "*.txt", SearchOption.TopDirectoryOnly);

                //used when executing application using developer command prompt
                string[] files = { "Provider1.txt", "Provider2.txt","Provider3.txt" };
                try
                {
                    foreach (var file in files)
                    {                      
                        Console.WriteLine("Searching:" + file);
                        Console.WriteLine("Enter any key");
                        //read/write  text file to list object                       
                        ReadFile(fData, file, args[0], args[1]);                       
                    }

                    //order flight data first by price and then by departure time in ascending order
                    List<flightData> sortedData = fData.OrderBy(flight => flight.price).ThenBy(flight => flight.departureTime).ToList();

                    //remove duplicate entries
                    int i = 1;
                    while (i < sortedData.Count)
                    {
                        if (sortedData[i].origin == sortedData[i - 1].origin
                            && sortedData[i].destination == sortedData[i - 1].destination
                            && sortedData[i].departureTime == sortedData[i - 1].departureTime
                            && sortedData[i].destinationTime == sortedData[i - 1].destinationTime
                            && sortedData[i].price == sortedData[i - 1].price)
                            sortedData.RemoveAt(i);
                        else
                            i++;
                    }

                    //print sorted and distinct data as per the given format
                    //if no data found for given origin and destination, print message accordingly
                    if (sortedData.Count > 0)
                    {
                        for (int k = 0; k < sortedData.Count; k++)
                        {
                            Console.WriteLine("{0} --> {1} ({2} --> {3}) - ${4}", sortedData[k].origin, sortedData[k].destination, sortedData[k].departureTime, sortedData[k].destinationTime, sortedData[k].price);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Flights Found for {0} --> {1}", args[0], args[1]);
                    }


                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occured:" + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Please enter the arguments!");
                Console.ReadKey();
            }

        }

        static void ReadFile(List<flightData> fData, string file, string originArgs, string destinationArgs)
        {
            String line;
            String delim;
            //read each text file into class object
            try
            {
                StreamReader reader = new StreamReader(file);
                string headerLine = reader.ReadLine(); // this will remove the header row
                if (file.Contains("Provider3.txt"))
                {
                    delim = "|";
                }
                else
                {
                    delim = ",";
                }

                string[] fields = null;                
                line = reader.ReadLine();

                while (line != null)                
                {                                          
                    fields = line.Split(delim.ToCharArray());
                                      
                   if (fields[0] == originArgs && fields[2] == destinationArgs)
                   {                       
                        flightData data = new flightData();
                        data.origin = fields[0];
                      
                        if (file.Contains("Provider2.txt"))
                        {
                            DateTime dt = DateTime.ParseExact(fields[1], "M-dd-yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                            data.departureTime = dt.ToString("M/dd/yyyy H:mm:ss").Replace('-', '/');
                        }
                        else
                        {
                            DateTime dt = DateTime.ParseExact(fields[1], "M/dd/yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                            data.departureTime = dt.ToString("M/dd/yyyy H:mm:ss").Replace('-', '/');
                        }

                        data.destination = fields[2];                        
                       
                        if (file.Contains("Provider2.txt"))
                        {
                            DateTime dt = DateTime.ParseExact(fields[3], "M-dd-yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                            data.destinationTime = dt.ToString("M/dd/yyyy H:mm:ss").Replace('-', '/');
                        }
                        else
                        {
                            DateTime dt = DateTime.ParseExact(fields[3], "M/dd/yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                            data.destinationTime = dt.ToString("M/dd/yyyy H:mm:ss").Replace('-', '/');
                        }
                       
                       data.price =  decimal.Parse(Regex.Replace(fields[4], @"[^\d.]", ""));                       

                       fData.Add(data);                      
                    }                   

                    line = reader.ReadLine();
                }               

                reader.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured:" + ex.Message);
            }
        }
    }
}
