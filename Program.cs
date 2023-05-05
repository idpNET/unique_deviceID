/*  Unique device ID Generator (based on Hardware and software specs.)
 *  +Mode selection: Full:: Combination of 3 hardware-based identifying IDs (MotherBoardID, MACAddress, ProcessorId) and a software-based identifying serial number (First drive's VolumeSerial)
 *  +Hashing device ID using PBKDF2 (SHA256 hashing Algorithm and static salting) for more security
 *  +Hashing process time tracking
 *  https://github.com/idpNET
 */
using PBKDF2_hashing;

namespace UniqueDeviceID
{
    internal class Program : HashGenerator
    {
        static void Main(string[] args)
        {
            // making new instance of GenerateDeviceID class
            GenerateDeviceID classInstance = new GenerateDeviceID();

            // getting user's input (Mode selection)
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("MODE selection: FULL (type \"y\" and press Enter) or SEMI (type \"n\" and press Enter) ?");
            var selection = Console.ReadLine();
            
            Console.WriteLine("");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;

            var time = HashGenerator.RunTimeMeasurement(() =>
            {
                // Generating device unique ID based on user mode selection
                if (selection == "y")
                {
                    var result = classInstance.DeviceID(true);

                    Console.WriteLine($"[FULL MODE] Unique Device ID hash is: {result}");
                }
                else if (selection == "n")
                {
                    var result = classInstance.DeviceID(false);
                    Console.WriteLine($"[SEMI MODE] Unique Device ID hash is: {result}");
                }else
                {
                    Console.WriteLine("Invalid input. FULL mode selected automatically.");
                    Console.WriteLine("");
                    var result = classInstance.DeviceID(true);
                    Console.WriteLine($"[FULL MODE] Unique Device ID hash is: {result}");
                }



            });
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            // Results
            Console.WriteLine("");
            Console.WriteLine("** Generating Device ID and Hashing process took " + time + " with " + Iterations + " times of iteration.");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;


        }
    }
}