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
    internal class GenerateDeviceID : HashGenerator
    {
        protected static bool FULLMode = true;
        private string? GetProcessorId()
        {
            string strProcessorId = string.Empty;
            var query = new SelectQuery("Win32_processor");
            var search = new ManagementObjectSearcher(query);

            foreach (ManagementObject info in search.Get())
                strProcessorId = info["processorId"].ToString();

            strProcessorId = Strings.InStr(1, strProcessorId, ".") > 0 ? strProcessorId.Replace(".", "-") : strProcessorId;
            return strProcessorId;
        }

        protected string? GetMACAddress()
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

        protected string? GetVolumeSerial(string strDriveLetter = "C")
        {

            var disk = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}:\"", strDriveLetter));
            disk.Get();
            var dskValue = disk["VolumeSerialNumber"].ToString();
            dskValue = Strings.InStr(1, dskValue, ".") > 0 ? dskValue.Replace(".", "-") : dskValue;
            return dskValue;
        }

        protected string? GetMotherBoardID()
        {

            string strMotherBoardID = string.Empty;
            var query = new SelectQuery("Win32_BaseBoard");
            var search = new ManagementObjectSearcher(query);
            foreach (ManagementObject info in search.Get())


                strMotherBoardID = info["SerialNumber"].ToString();
            strMotherBoardID = Strings.InStr(1, strMotherBoardID, ".") > 0 ? strMotherBoardID.Replace(".", "-") : strMotherBoardID;
            return strMotherBoardID;

        }


        public string DeviceID(bool Mode)
        {
            FULLMode = Mode ? true : false;
            var result = FULLMode ? GetProcessorId() + GetMACAddress() + GetVolumeSerial() + GetMotherBoardID() : GetProcessorId() + GetMACAddress() + GetMotherBoardID();
            var Hash = ComputeBytesHash(result.ToString(), out var salt);
            string HashToString = MergeBytesIntoString(Hash);
            return HashToString;
        }
    }
}
