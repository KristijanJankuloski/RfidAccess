using RfidAccess.Web.ViewModels.Base;

namespace RfidAccess.Web.Helpers
{
    public static class BackupHelper
    {
        public static async Task<Result> BackupDatabase(string databasePath, string backupFileName)
        {
            return await Task.Run(() =>
            {
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (var drive in drives)
                {
                    if (drive.IsReady && drive.DriveType == DriveType.Removable)
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
                }

                return Result.Failure("No suitable USB drive found.");
            });
        }
    }
}
