/*  Unique device ID Generator (based on Hardware and software specs.)
 *  +Mode selection: Full:: Combination of 3 hardware-based identifying IDs (MotherBoardID, MACAddress, ProcessorId) and a software-based identifying serial number (First drive's VolumeSerial)
 *  +Hashing device ID using PBKDF2 (SHA256 hashing Algorithm and static salting) for more security
 *  +Hashing process time tracking
 *  https://github.com/idpNET
 */
using PBKDF2_hashing;

namespace UniqueDeviceID
{
    /// <summary>
    /// Generates a unique device ID hash value based on hardware-based or software-based specs
    /// </summary>
    internal class Program : GenerateDeviceID
    {
        #region Class Methods
        static void Main(string[] args)
        {

            // Gets user's input (Mode selection)
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("MODE selection: FULL (type \"y\" and press Enter) or SEMI (type \"n\" and press Enter) ?");
            var selection = Console.ReadLine();

            Console.WriteLine("");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;

            // Keeps tracking of device ID hash processing time
            var time = HashGenerator.RunTimeMeasurement(() =>
            {
               
                // Generates device unique ID based on user mode selection
                if (selection == "y")
                {
                    FullMode = true;
                    var result = GetDeviceHashedID;


                    Console.WriteLine($"[FULL MODE] Unique Device ID hash is: {result}");
                }
                else if (selection == "n")
                {
                    FullMode = false;
                    var result = GetDeviceHashedID;
                    Console.WriteLine($"[SEMI MODE] Unique Device ID hash is: {result}");
                }
                else
                {
                    Console.WriteLine("Invalid input. FULL mode selected automatically.");
                    Console.WriteLine("");
                    FullMode = true;
                    var result = GetDeviceHashedID;
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
            // Stops auto-closing action of console after execution
            Console.ReadLine();

        }
        #endregion
    }
}