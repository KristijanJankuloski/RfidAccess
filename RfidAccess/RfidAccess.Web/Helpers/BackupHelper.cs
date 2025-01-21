using RfidAccess.Web.ViewModels.Base;
using System.Runtime.InteropServices;

namespace RfidAccess.Web.Helpers
{
    public static class BackupHelper
    {
        public static async Task<Result> BackupDatabase(string databasePath, string backupFileName)
        {
            return await Task.Run(() =>
            {
                DriveInfo[] drives = GetRemovableDrives();

                foreach (var drive in drives)
                {
                    long availableSpace = drive.AvailableFreeSpace;
                    long databaseSize = new FileInfo(databasePath).Length;

                    if (availableSpace > databaseSize)
                    {
                        string backupPath = Path.Combine(drive.RootDirectory.FullName, backupFileName);

                        File.Copy(databasePath, backupPath, true);
                        return Result.Success;
                    }
                    else
                    {
                        return Result.Failure("Not enough space on the USB drive.");
                    }
                }

                return Result.Failure("No suitable USB drive found.");
            });
        }

        private static DriveInfo[] GetRemovableDrives()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Removable).ToArray();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var drives = DriveInfo.GetDrives().Where(d => d.IsReady && d.RootDirectory.FullName.StartsWith("/media")).ToArray();
                return drives;
            }

            return Array.Empty<DriveInfo>();
        }
    }
}
