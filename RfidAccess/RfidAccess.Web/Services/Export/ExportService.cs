using OfficeOpenXml;
using OfficeOpenXml.Style;
using RfidAccess.Web.DataAccess.Repositories.Records;
using RfidAccess.Web.Helpers;
using RfidAccess.Web.Models;
using RfidAccess.Web.Services.Schedules;
using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.Schedule;
using System.Drawing;

namespace RfidAccess.Web.Services.Export
{
    public class ExportService(IRecordRepository recordRepository, IScheduleService scheduleService) : IExportService
    {
        private readonly IRecordRepository recordRepository = recordRepository;
        private readonly IScheduleService scheduleService = scheduleService;

        public async Task<Result<byte[]>> ExportRecords(DateTime startDate, DateTime endDate)
        {
            List<Record> records = await recordRepository.GetFromDates(startDate, endDate);
            if (records.Count == 0)
            {
                return new Result<byte[]>("No records");
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Сите настани");

                worksheet.Cells[1, 1].Value = "Име";
                worksheet.Cells[1, 2].Value = "Презиме";
                worksheet.Cells[1, 3].Value = "Шифра";
                worksheet.Cells[1, 4].Value = "Време";
                worksheet.Cells[1, 5].Value = "Вкупно";
                worksheet.Cells[2, 5].Value = records.Count;
                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                for (int i = 0; i < records.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = records[i]?.Person?.FirstName ?? string.Empty;
                    worksheet.Cells[i + 2, 2].Value = records[i]?.Person?.LastName ?? string.Empty;
                    worksheet.Cells[i + 2, 3].Value = records[i]?.Code ?? string.Empty;
                    worksheet.Cells[i + 2, 4].Value = records[i]?.Time.ToString("HH:mm dd:MM:yyyy") ?? string.Empty;
                }

                var personSheet = package.Workbook.Worksheets.Add("По корисници");
                var personGroup = records.GroupBy(x => x.PersonId).ToList();
                personSheet.Cells[1, 1].Value = "Име";
                personSheet.Cells[1, 2].Value = "Презиме";
                personSheet.Cells[1, 3].Value = "Шифра";
                personSheet.Cells[1, 4].Value = "Број на влезови";
                using (var range = personSheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }
                for (int i = 0; i < personGroup.Count; i++)
                {
                    Record? firstRecord = personGroup[i].FirstOrDefault();
                    personSheet.Cells[i + 2, 1].Value = firstRecord?.Person?.FirstName ?? string.Empty;
                    personSheet.Cells[i + 2, 2].Value = firstRecord?.Person?.LastName ?? string.Empty;
                    personSheet.Cells[i + 2, 3].Value = firstRecord?.Code ?? string.Empty;
                    personSheet.Cells[i + 2, 4].Value = personGroup[i].Count();
                }

                TimeSlotViewModel? vm = (await scheduleService.GetTimeSlots()).Value;
                if (vm == null)
                {
                    using var streamBase = new MemoryStream();
                    package.SaveAs(streamBase);

                    return new Result<byte[]>(streamBase.ToArray());
                }

                DateTime tempDate = startDate;
                while(tempDate <= endDate)
                {
                    List<ConvertedTimeSlot> slots = TimeSlotHelper.GetDaySlots(vm, tempDate);
                    if (slots.Count == 0)
                    {
                        tempDate = tempDate.AddDays(1);
                        continue;
                    }

                    slots.ForEach(slot =>
                    {
                        List<Record> slotRecords = records.Where(x => x.Time >= slot.Start && x.Time <= slot.End).ToList();
                        if (slotRecords.Count == 0)
                            return;

                        var slotSheet = package.Workbook.Worksheets.Add($"{tempDate:dd.MM.yyyy}_{slot.Start:HH.mm}-{slot.End:HH.mm}");
                        slotSheet.Cells[1, 1].Value = "Име";
                        slotSheet.Cells[1, 2].Value = "Презиме";
                        slotSheet.Cells[1, 3].Value = "Шифра";
                        slotSheet.Cells[1, 4].Value = "Време";
                        slotSheet.Cells[1, 5].Value = "Вкупно";
                        slotSheet.Cells[2, 5].Value = slotRecords.Count;
                        using (var range = slotSheet.Cells[1, 1, 1, 4])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }

                        for (int i = 0; i < slotRecords.Count; i++)
                        {
                            slotSheet.Cells[i + 2, 1].Value = slotRecords[i]?.Person?.FirstName ?? string.Empty;
                            slotSheet.Cells[i + 2, 2].Value = slotRecords[i]?.Person?.LastName ?? string.Empty;
                            slotSheet.Cells[i + 2, 3].Value = slotRecords[i]?.Code ?? string.Empty;
                            slotSheet.Cells[i + 2, 4].Value = slotRecords[i]?.Time.ToString("HH:mm dd:MM:yyyy") ?? string.Empty;
                        }
                    });
                    tempDate = tempDate.AddDays(1);
                }

                using var stream = new MemoryStream();
                package.SaveAs(stream);

                return new Result<byte[]>(stream.ToArray());
            }
        }
    }
}
