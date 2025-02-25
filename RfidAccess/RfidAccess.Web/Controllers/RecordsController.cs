﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RfidAccess.Web.Services.Export;
using RfidAccess.Web.Services.Records;

namespace RfidAccess.Web.Controllers
{
    [Authorize]
    public class RecordsController(IRecordService recordService, IExportService exportService) : Controller
    {
        private readonly IRecordService recordService = recordService;
        private readonly IExportService exportService = exportService;

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] int? page = 1,
            [FromQuery] string? code = null)
        {
            try
            {
                page ??= 1;
                int take = 10;
                int skip = ((int)page - 1) * take;
                if (skip < 0)
                {
                    TempData["Error"] = "Недозволено пребарување";
                    return RedirectToAction("Index");
                }
                var result = await recordService.GetPaginatedRecords(skip, take, code);
                if (result.IsFailed)
                {
                    TempData["Error"] = result.Message;
                    return RedirectToAction("Index", "Home");
                }

                return View(result.Value);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Export(DateTime start, DateTime end)
        {
            try
            {
                var result = await exportService.ExportRecords(start, end);
                if (result.IsFailed || result.Value == null)
                {
                    TempData["Error"] = result.Message;
                    return RedirectToAction("Index");
                }

                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string date = DateTime.Now.ToString("dd_MM_yyyy");
                string fileName = $"оброци_извештај_{date}.xlsx";
                return File(result.Value, contentType, fileName);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
