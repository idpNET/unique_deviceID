/*  Unique device ID Generator (based on Hardware and software specs.)
 *  +Mode selection: Full:: Combination of 3 hardware-based (MotherBoardID, MACAddress, ProcessorId) and a software-based identifying IDs (a drive's VolumeSerial)
 *  +Hashing device ID using PBKDF2 (SHA256 hashing Algorithm and static salting) for more security
 *  +Hashing process time tracking
 *  https://github.com/idpNET
 */

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using PBKDF2_hashing;
using System.Management;


namespace UniqueDeviceID
{
    /// <summary>
    /// Generates a unique hardware-based [or] software-based ID hash value using PBKDF2 (SHA256 hashing Algorithm and static salting)
    /// </summary>
    internal class GenerateDeviceID : HashGenerator
    {
        #region Variables Declaration 
        protected static bool FullMode = true;
        protected static string GetDeviceHashedID
        {
            get
            {
                return FullMode? DeviceID(true): DeviceID(false);
            }
        }




        #endregion

        #region Class Methods
        /// <summary>
        /// [hardware-based] Gets processor ID using ManagementObject
        /// </summary>
        /// <returns>Processor ID as string</returns>
        private static string? GetProcessorId()
        {
            string strProcessorId = string.Empty;
            var query = new SelectQuery("Win32_processor");
            var search = new ManagementObjectSearcher(query);

            foreach (ManagementObject info in search.Get())
                strProcessorId = info["processorId"].ToString();

            strProcessorId = Strings.InStr(1, strProcessorId, ".") > 0 ? strProcessorId.Replace(".", "-") : strProcessorId;
            return strProcessorId;
        }

        /// <summary>
        /// [hardware-based] Gets MAC address ID using ManagementClass
        /// </summary>
        /// <returns>MAC address ID as string</returns>
        private static string? GetMACAddress()
        {
            var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            var moc = mc.GetInstances();
            string MACAddress = string.Empty;
            foreach (ManagementObject mo in moc)
            {

                if (MACAddress.Equals(string.Empty))
                {
                    if (Conversions.ToBoolean(mo["IPEnabled"]))
                        MACAddress = mo["MacAddress"].ToString();

                    mo.Dispose();
                }

                MACAddress = MACAddress.Replace(":", string.Empty);

            }
            MACAddress = Strings.InStr(1, MACAddress, ".") > 0 ? MACAddress.Replace(".", "-") : MACAddress;
            return MACAddress;
        }

        /// <summary>
        /// [software-based] Gets Volume Serial value using ManagementObject
        /// </summary>
        /// <param name="strDriveLetter"></param>
        /// <returns>Volume Serial value as string</returns>
        private static string? GetVolumeSerial(string strDriveLetter = "C")
        {

            var disk = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}:\"", strDriveLetter));
            disk.Get();
            var dskValue = disk["VolumeSerialNumber"].ToString();
            dskValue = Strings.InStr(1, dskValue, ".") > 0 ? dskValue.Replace(".", "-") : dskValue;
            return dskValue;
        }

        /// <summary>
        /// [hardware-based] Gets motherboard ID using ManagementObject
        /// </summary>
        /// <returns>Motherboard ID as string</returns>
        private static string? GetMotherBoardID()
        {

            string strMotherBoardID = string.Empty;
            var query = new SelectQuery("Win32_BaseBoard");
            var search = new ManagementObjectSearcher(query);
            foreach (ManagementObject info in search.Get())


                strMotherBoardID = info["SerialNumber"].ToString();
            strMotherBoardID = Strings.InStr(1, strMotherBoardID, ".") > 0 ? strMotherBoardID.Replace(".", "-") : strMotherBoardID;
            return strMotherBoardID;

        }

        /// <summary>
        /// Gets unique device ID hash value
        /// </summary>
        /// <param name="Mode"></param>
        /// <remarks>Modes: 1- FUll mode which uses both the hardware and software specs for device ID generation  2- Semi mode which only uses hardware specs for device ID generation</remarks>
        /// <returns>Device ID hash value as string</returns>
        private static string DeviceID(bool Mode)
        {
            FullMode = Mode ? true : false;
            var result = FullMode ? GetProcessorId() + GetMACAddress() + GetVolumeSerial() + GetMotherBoardID() : GetProcessorId() + GetMACAddress() + GetMotherBoardID();
            var Hash = ComputeBytesHash(result.ToString());
            string HashToString = MergeBytesIntoString(Hash);
            return HashToString;
        }
        #endregion
    }
}
