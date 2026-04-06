using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TTSteelAndroidAPI.Data;
using static TTSteelAndroidAPI.Model.ProductionExecution.Production;

[ApiController]
[Route("api/")]
public class ProductionExecutionController : ControllerBase
{
    private readonly DbConnectionContext _databaseContext;
    private readonly ILogger<ProductionExecutionController> _logger;
    string _query = string.Empty;
    public ProductionExecutionController(
        DbConnectionContext databaseContext,
        ILogger<ProductionExecutionController> logger)
    {
        _databaseContext = databaseContext;
        _logger = logger;
    }

    [HttpGet("GetSlitRewindCutCounts")]
    public async Task<IActionResult> GetUnitCounts()
    {
        try
        {
            _logger.LogInformation("Fetching unit counts from stored procedure GetSlitRewindCutCounts");

            using var conn = _databaseContext.CreateConnection();
            conn.Open();
            _query = "call GetSlitRewindCutCounts";
            // Execute stored procedure (SAP HANA with quoted name)
            var result = await conn.QueryFirstOrDefaultAsync<UnitCountsDto>(_query);

            var unitCounts = result ?? new UnitCountsDto();

            _logger.LogInformation(
                "Successfully retrieved counts - Slitting: {Slitting}, Cutting: {Cutting}, Rewinding: {Rewinding}",
                unitCounts.SlittingCount,
                unitCounts.CuttingLengthCount,
                unitCounts.RewindingCount
            );

            return Ok(new ApiResponse<UnitCountsDto>
            {
                Success = true,
                Message = "Unit counts retrieved successfully",
                Data = unitCounts
            });
        }
        catch (Exception ex)
        {
            // Log full exception details
            _logger.LogError(ex, "Error while fetching unit counts from SAP HANA");

            // Determine if it's an SAP/HANA related error
            string userMessage = "An error occurred while fetching unit counts";
            if (ex.Message.Contains("SAP") || ex.Message.Contains("HANA") ||
                ex.Message.Contains("dberror") || ex.Message.Contains("SQL"))
            {
                userMessage = $"SAP HANA error: {ex.Message}";
            }

            return StatusCode(500, new ApiResponse<UnitCountsDto>
            {
                Success = false,
                Message = userMessage,
                Data = null
            });
        }
    }
    [HttpGet("GetEachJobOwnCounts")]
    public async Task<IActionResult> GetUnitSourceCounts([FromQuery] string unitCode)
    {
        try
        {
            _logger.LogInformation("Fetching source counts for UnitCode: {UnitCode}", unitCode);

            if (string.IsNullOrWhiteSpace(unitCode))
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "UnitCode is required",
                    Data = null
                });
            }

            using var conn = _databaseContext.CreateConnection();
            conn.Open();
            _query = "call COV_GetSourceCountsByUnitCode('"+unitCode+"')";
            // Execute stored procedure with parameter
            var result = await conn.QueryFirstOrDefaultAsync<SourceCountsDto>(_query);

            var counts = result ?? new SourceCountsDto();

            _logger.LogInformation(
                "Retrieved counts for {UnitCode} - Own: {OwnCount}, Jobwork: {JobworkCount}",
                unitCode, counts.OwnCount, counts.JobworkCount
            );

            return Ok(new ApiResponse<SourceCountsDto>
            {
                Success = true,
                Message = "Counts retrieved successfully",
                Data = counts
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching source counts for UnitCode: {UnitCode}", unitCode);

            string userMessage = "An error occurred while fetching counts";
            if (ex.Message.Contains("SAP") || ex.Message.Contains("HANA") || ex.Message.Contains("dberror"))
            {
                userMessage = $"SAP HANA error: {ex.Message}";
            }

            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = userMessage,
                Data = null
            });
        }
    }

    [HttpGet("GetScheduleOfJobOwn")]
    public async Task<IActionResult> GetScheduleDetails([FromQuery] string unitCode, [FromQuery] string source)
    {
        try
        {
            _logger.LogInformation("Fetching schedule details for UnitCode: {UnitCode}, Source: {Source}", unitCode, source);

            // Input validation
            if (string.IsNullOrWhiteSpace(unitCode) || string.IsNullOrWhiteSpace(source))
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Both unitCode and source are required"
                });
            }

            using var conn = _databaseContext.CreateConnection();
            conn.Open();
            _query = "call COV_GetScheduleDetails('" + unitCode + "','" + source + "')";
            // Execute stored procedure with parameters
            var result = await conn.QueryAsync<ScheduleDto>(_query);

            var scheduleList = result?.ToList() ?? new List<ScheduleDto>();

            _logger.LogInformation("Retrieved {Count} schedule records", scheduleList.Count);

            return Ok(new ApiResponse<IEnumerable<ScheduleDto>>
            {
                Success = true,
                Message = "Schedule details retrieved successfully",
                Data = scheduleList
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching schedule details for UnitCode: {UnitCode}, Source: {Source}", unitCode, source);

            string userMessage = "An error occurred while fetching schedule details";
            if (ex.Message.Contains("SAP") || ex.Message.Contains("HANA") || ex.Message.Contains("dberror"))
            {
                userMessage = $"SAP HANA error: {ex.Message}";
            }

            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = userMessage
            });
        }
    }
    [HttpGet("GetScheduleSizeCount")]
    public async Task<IActionResult> GetScheduleSizeCount([FromQuery] string schNo)
    {
        try
        {
            _logger.LogInformation("Fetching work order count for Schedule No: {SchNo}", schNo);

            // Input validation
            if (string.IsNullOrWhiteSpace(schNo))
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Schedule number (schNo) is required"
                });
            }

            using var conn = _databaseContext.CreateConnection();
            conn.Open();
            _query = "call COV_GetScheduleSizeCount('" + schNo + "')";
            // Execute stored procedure
            var result = await conn.QueryFirstOrDefaultAsync<CountDto>(_query);

            var count = result ?? new CountDto { TotalCount = 0 };

            _logger.LogInformation("Retrieved count {TotalCount} for schedule {SchNo}", count.TotalCount, schNo);

            return Ok(new ApiResponse<CountDto>
            {
                Success = true,
                Message = "Work order count retrieved successfully",
                Data = count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching work order count for schedule {SchNo}", schNo);

            string userMessage = "An error occurred while fetching work order count";
            if (ex.Message.Contains("SAP") || ex.Message.Contains("HANA") || ex.Message.Contains("dberror"))
            {
                userMessage = $"SAP HANA error: {ex.Message}";
            }

            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = userMessage
            });
        }
    }

    [HttpGet("GetJobBatchDetails")]
    public async Task<IActionResult> GetJobBatchDetails([FromQuery] string schNo)
    {
        try
        {
            _logger.LogInformation("Fetching job batch details for Schedule No: {SchNo}", schNo);

            // Input validation
            if (string.IsNullOrWhiteSpace(schNo))
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Schedule number (schNo) is required"
                });
            }

            using var conn = _databaseContext.CreateConnection();
            conn.Open();
            _query = "call COV_GetJobBatchDetails('" + schNo + "')";
            // Execute stored procedure
            var result = await conn.QueryAsync<JobBatchDto>(_query);

            var jobBatchList = result?.ToList() ?? new List<JobBatchDto>();

            _logger.LogInformation("Retrieved {Count} job batch records for schedule {SchNo}", jobBatchList.Count, schNo);

            return Ok(new ApiResponse<IEnumerable<JobBatchDto>>
            {
                Success = true,
                Message = "Job batch details retrieved successfully",
                Data = jobBatchList
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching job batch details for schedule {SchNo}", schNo);

            string userMessage = "An error occurred while fetching job batch details";
            if (ex.Message.Contains("SAP") || ex.Message.Contains("HANA") || ex.Message.Contains("dberror"))
            {
                userMessage = $"SAP HANA error: {ex.Message}";
            }

            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = userMessage
            });
        }
    }

    [HttpGet("GetScheduleSizeDetails")]
    public async Task<IActionResult> GetScheduleSizeDetails([FromQuery] string schNo)
    {
        try
        {
            _logger.LogInformation("Fetching schedule size details for Schedule No: {SchNo}", schNo);

            // Input validation
            if (string.IsNullOrWhiteSpace(schNo))
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Schedule number (schNo) is required"
                });
            }

            using var conn = _databaseContext.CreateConnection();
            conn.Open();
            _query= "call COV_GetScheduleSizeDetails('" + schNo + "')"; 
            // Execute stored procedure
            var result = await conn.QueryAsync<ScheduleSizeDto>(_query);

            var scheduleSizeList = result?.ToList() ?? new List<ScheduleSizeDto>();

            _logger.LogInformation("Retrieved {Count} schedule size records for {SchNo}", scheduleSizeList.Count, schNo);

            return Ok(new ApiResponse<IEnumerable<ScheduleSizeDto>>
            {
                Success = true,
                Message = "Schedule size details retrieved successfully",
                Data = scheduleSizeList
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching schedule size details for schedule {SchNo}", schNo);

            string userMessage = "An error occurred while fetching schedule size details";
            if (ex.Message.Contains("SAP") || ex.Message.Contains("HANA") || ex.Message.Contains("dberror"))
            {
                userMessage = $"SAP HANA error: {ex.Message}";
            }

            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = userMessage
            });
        }
        [HttpPost("CreateProductionExecutionWithC3")]
        public async Task<IActionResult> CreateProductionExecutionWithC3([FromBody] PrdExeItem request)
        {
            using var conn = _databaseContext.CreateConnection();
            conn.Open();
            using var trans = conn.BeginTransaction();

            try
            {
                _logger.LogInformation("Starting PRDEXE creation in SAP for SchNo: {SchNo}", request.PrdExe.U_SchNo);

                // ========== 1. Prepare payload for SAP ==========
                var sapPayload = request.PrdExe;
                sapPayload.DocEntry = 0;  // Let SAP assign
                sapPayload.DocNum = 0;
                // Remove C3 collection (not sent to SAP)
                sapPayload.CCO_TRNS_PRDEXE_C3Collection = null;

                // ========== 2. Call SAP Service Layer to create PRDEXE ==========
                var sapResult = await _sapService.PostAsync("PRDEXE", sapPayload);
                var resultJson = JObject.Parse(sapResult);
                var sapDocEntry = resultJson["DocEntry"]?.Value<int>();
                var sapDocNum = resultJson["DocNum"]?.Value<int>();

                if (sapDocEntry == null || sapDocEntry == 0)
                    throw new Exception("SAP PRDEXE creation failed – no DocEntry returned");

                _logger.LogInformation("SAP PRDEXE created. DocEntry: {DocEntry}, DocNum: {DocNum}", sapDocEntry, sapDocNum);

                // ========== 3. Update C2 lines in SAP with bundle quantities (if any) ==========
                if (request.BundleDetails != null && request.BundleDetails.Any())
                {
                    foreach (var bundle in request.BundleDetails)
                    {
                        // Build PATCH payload for a single C2 line
                        var patchPayload = new { U_ActPkt = bundle.BundleQty };
                        var endpoint = $"PRDEXE({sapDocEntry})/CCO_TRNS_PRDEXE_C2Collection({bundle.LineId})";
                        await _sapService.PatchAsync("PRDEXE",sapDocEntry.toString(),patchPayload);
                    }
                    _logger.LogInformation("Updated {Count} C2 lines in SAP", request.BundleDetails.Count);
                }

                // ========== 4. Generate batches and insert C3 lines locally ==========
                if (request.C3List != null && request.C3List.Any())
                {
                    var formattedDate = request.PostDate.ToString("yyyyMMdd");

                    for (int i = 0; i < request.C3List.Count; i++)
                    {
                        var item = request.C3List[i];
                        item.DocEntry = sapDocEntry.Value;
                        item.LineId = i + 1;

                        // Call batch generation stored procedure
                        var batchNum = await conn.ExecuteScalarAsync<string>(
                            $@"CALL ""BatchGeneration_PRDEXE""('{formattedDate}', '{request.MachineCode}')",
                            transaction: trans,
                            commandTimeout: 120
                        );
                        item.U_OPBatch = batchNum;

                        // Insert into local C3 table
                        var c3Query = BuildC3InsertQuery(item, request.MachineCode, request.PostDate);
                        await conn.ExecuteAsync(c3Query, transaction: trans);
                    }
                    _logger.LogInformation("Inserted {Count} C3 lines locally", request.C3List.Count);
                }

                trans.Commit();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Production execution created in SAP and C3 processed locally",
                    Data = new { sapDocEntry, sapDocNum }
                });
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _logger.LogError(ex, "Error in combined PRDEXE creation");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message.Contains("SAP") ? $"SAP error: {ex.Message}" : "An error occurred"
                });
            }
        }
    }

    [HttpPost("CreateProductionExecutionWithC3")]
    public async Task<IActionResult> CreateProductionExecutionWithC3([FromBody] PrdExeItem request)
    {
        using var conn = _databaseContext.CreateConnection();
        conn.Open();
        using var trans = conn.BeginTransaction();

        try
        {
            _logger.LogInformation("Starting PRDEXE creation in SAP for SchNo: {SchNo}", request.PrdExe.U_SchNo);

            // ========== 1. Prepare payload for SAP ==========
            var sapPayload = request.PrdExe;
            sapPayload.DocEntry = 0;  // Let SAP assign
            sapPayload.DocNum = 0;
            // Remove C3 collection (not sent to SAP)
            sapPayload.CCO_TRNS_PRDEXE_C3Collection = null;

            // ========== 2. Call SAP Service Layer to create PRDEXE ==========
            var sapResult = await _sapService.PostAsync("PRDEXE", sapPayload);
            var resultJson = JObject.Parse(sapResult);
            var sapDocEntry = resultJson["DocEntry"]?.Value<int>();
            var sapDocNum = resultJson["DocNum"]?.Value<int>();

            if (sapDocEntry == null || sapDocEntry == 0)
                throw new Exception("SAP PRDEXE creation failed – no DocEntry returned");

            _logger.LogInformation("SAP PRDEXE created. DocEntry: {DocEntry}, DocNum: {DocNum}", sapDocEntry, sapDocNum);

            // ========== 3. Update C2 lines in SAP with bundle quantities (if any) ==========
            if (request.BundleDetails != null && request.BundleDetails.Any())
            {
                foreach (var bundle in request.BundleDetails)
                {
                    // Build PATCH payload for a single C2 line
                    var patchPayload = new { U_ActPkt = bundle.BundleQty };
                    var endpoint = $"PRDEXE({sapDocEntry})/CCO_TRNS_PRDEXE_C2Collection({bundle.LineId})";
                    await _sapService.PatchAsync("PRDEXE",sapDocEntry.toString(), patchPayload);
                }
                _logger.LogInformation("Updated {Count} C2 lines in SAP", request.BundleDetails.Count);
            }

            // ========== 4. Generate batches and insert C3 lines locally ==========
            if (request.C3List != null && request.C3List.Any())
            {
                var formattedDate = request.PostDate.ToString("yyyyMMdd");

                for (int i = 0; i < request.C3List.Count; i++)
                {
                    var item = request.C3List[i];
                    item.DocEntry = sapDocEntry.Value;
                    item.LineId = i + 1;

                    // Call batch generation stored procedure
                    var batchNum = await conn.ExecuteScalarAsync<string>(
                        $@"CALL ""BatchGeneration_PRDEXE""('{formattedDate}', '{request.MachineCode}')",
                        transaction: trans,
                        commandTimeout: 120
                    );
                    item.U_OPBatch = batchNum;

                    // Insert into local C3 table
                    var c3Query = BuildC3InsertQuery(item, request.MachineCode, request.PostDate);
                    await conn.ExecuteAsync(c3Query, transaction: trans);
                }
                _logger.LogInformation("Inserted {Count} C3 lines locally", request.C3List.Count);
            }

            trans.Commit();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Production execution created in SAP and C3 processed locally",
                Data = new { sapDocEntry, sapDocNum }
            });
        }
        catch (Exception ex)
        {
            trans.Rollback();
            _logger.LogError(ex, "Error in combined PRDEXE creation");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message.Contains("SAP") ? $"SAP error: {ex.Message}" : "An error occurred"
            });
        }
        [HttpPost("CreateGoodsIssue")]
        public async Task<IActionResult> CreateGoodsIssue([FromQuery] int docEntry)
        {
            using var conn = _databaseContext.CreateConnection();
            conn.Open();

            using var trans = conn.BeginTransaction();

            try
            {
                _logger.LogInformation("Creating Goods Issue for PRDEXE DocEntry: {DocEntry}", docEntry);
                _query = "call COV_GetGoodsIssueData(" + docEntry + ")";
                // ================= 1. FETCH DATA USING STORED PROCEDURE =================
                var data = (await conn.QueryAsync<GoodsIssueDataDto>(_query)).ToList();

                if (!data.Any())
                {
                    _logger.LogWarning("No goods issue data found for DocEntry: {DocEntry}", docEntry);
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "No open/selected lines found for this production execution"
                    });
                }

                // ================= 2. BUILD SAP PAYLOAD =================
                var firstRow = data.First();
                var payload = new SapReceiptOIGN
                {
                    DocDate = DateTime.Now,
                    Comments = $"Auto Goods Issue from PRDEXE - {docEntry}",
                    U_DocType = (firstRow.U_Source == "Jobwork" || firstRow.U_Source == "Job-Work") ? "J"
                              : (firstRow.U_Source == "Own" || firstRow.U_Source == "OWN") ? "N"
                              : "",
                    U_SrcObj = firstRow.Object,
                    U_SchNo = firstRow.U_SchNo,
                    DocumentLines = data
                        .GroupBy(x => new { x.ItemCode, x.U_WhsCode })
                        .Select(g => new SapReceiptLine
                        {
                            ItemCode = g.Key.ItemCode,
                            WarehouseCode = g.Key.U_WhsCode,
                            Quantity = g.Sum(x => x.Quantity),
                            BatchNumbers = g.Select(x => new SapBatch
                            {
                                BatchNumber = x.BatchNum,
                                Quantity = x.Quantity
                            }).ToList()
                        }).ToList()
                };

                // ================= 3. CALL SAP SERVICE LAYER =================
                var sapResult = await _sapService.PostAsync("InventoryGenExits", payload);
                var resultJson = JObject.Parse(sapResult);
                var sapDocEntry = resultJson["DocEntry"]?.Value<int>();

                if (sapDocEntry == null || sapDocEntry == 0)
                    throw new Exception("SAP Goods Issue failed – no DocEntry returned");

                // ================= 4. UPDATE LOCAL C3 TABLE =================
                var updateSql = $@"
            UPDATE ""@CCO_TRNS_PRDEXE_C3"" 
            SET ""U_GIDE"" = "+sapDocEntry+"  WHERE ""DocEntry"" = "+docEntry+"";

                await conn.ExecuteAsync(updateSql, transaction: trans);

                trans.Commit();

                _logger.LogInformation("Goods Issue created. SAP DocEntry: {SapDocEntry} for PRDEXE {DocEntry}", sapDocEntry, docEntry);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Goods Issue created & C3 updated successfully",
                    Data = new { sapDocEntry }
                });
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _logger.LogError(ex, "Error creating Goods Issue for DocEntry: {DocEntry}", docEntry);

                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message.Contains("SAP") ? $"SAP error: {ex.Message}" : "An error occurred while creating Goods Issue"
                });
            }
     
        }
        [HttpPost("CreateGoodsReceipt")]
        public async Task<IActionResult> CreateGoodsReceipt([FromQuery] int docEntry, [FromQuery] string sch_no)
        {
            using var conn = _databaseContext.CreateConnection();
            conn.Open();

            using var trans = conn.BeginTransaction();

            try
            {
                _logger.LogInformation("Creating Goods Receipt for PRDEXE DocEntry: {DocEntry}, Schedule No: {SchNo}", docEntry, sch_no);
                _query = "call COV_GetGoodsReceiptData(" + docEntry + ")";
                // ================= 1. FETCH DATA USING STORED PROCEDURE =================
                var data = (await conn.QueryAsync<GoodsReceiptDataDto>(_query, transaction: trans)).ToList();

                if (!data.Any())
                    return BadRequest("No data found for Goods Receipt");

                // ================= 2. BUILD SAP PAYLOAD =================
                var firstRow = data.First();
                var payload = new SapReceiptOIGN
                {
                    DocDate = DateTime.Now,
                    Comments = $"Auto GR from PRDEXE - {docEntry}",
                    U_DocType = (firstRow.U_Source == "Jobwork" || firstRow.U_Source == "Job-Work") ? "J"
                              : (firstRow.U_Source == "Own" || firstRow.U_Source == "OWN") ? "N"
                              : "",
                    U_SrcObj = firstRow.Object,
                    U_SchNo = firstRow.U_SchNo,
                    DocumentLines = data.Select(x => new SapReceiptLine
                    {
                        ItemCode = x.U_OPItem,
                        WarehouseCode = x.U_FGWhs,
                        Quantity = (decimal)(x.U_NetWt ?? 0),
                        BatchNumbers = new List<SapBatch>
                {
                    new SapBatch
                    {
                        BatchNumber = x.U_OPBatch,
                        Quantity = (decimal)(x.U_NetWt ?? 0)
                    }
                }
                    }).ToList()
                };

                // ================= 3. CALL SAP =================
                var sapResult = await _sapService.PostAsync("InventoryGenEntries", payload);
                var resultJson = JObject.Parse(sapResult);
                var sapDocEntry = resultJson["DocEntry"]?.Value<int>();

                if (sapDocEntry == null)
                    throw new Exception("SAP Goods Receipt failed");

                // ================= 4. UPDATE C3, HD, AND SCHEDULE =================
                var updateC3Sql = $@"UPDATE ""@CCO_TRNS_PRDEXE_C3"" SET ""U_GRDE"" = {sapDocEntry} WHERE ""DocEntry"" = {docEntry}";
                await conn.ExecuteAsync(updateC3Sql, transaction: trans);

                var updateHdSql = $@"UPDATE ""@CCO_TRNS_PRDEXE_HD"" SET ""U_SchSts"" = 'Completed' WHERE ""U_SchNo"" = '{sch_no}' AND ""DocEntry"" = {docEntry}";
                await conn.ExecuteAsync(updateHdSql, transaction: trans);

                var updateSchSql = $@"UPDATE ""@CCO_TRNS_PRCSCH_HD"" SET ""U_SchSts"" = 'Completed' WHERE ""U_SchNo"" = '{sch_no}'";
                await conn.ExecuteAsync(updateSchSql, transaction: trans);

                trans.Commit();

                return Ok(new
                {
                    message = "Goods Receipt created & C3 updated successfully",
                    sapDocEntry = sapDocEntry
                });
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(500, new
                {
                    message = "Error creating Goods Receipt",
                    error = ex.Message
                });
            }
        }

        [HttpGet("GetDbList")]
        public async Task<IActionResult> GetDbList()
        {
            try
            {
                _logger.LogInformation("Fetching database schema info");

                using var conn = _databaseContext.CreateConnection();
                conn.Open();
                _query = "call COV_GetDbList";
                var result = await conn.QueryFirstOrDefaultAsync(_query);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Database info retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching database list");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error fetching database info",
                    Data = null
                });
            }
        }

        [HttpGet("GetWorkOrderLableDetails")]
        public async Task<IActionResult> GetWorkOrderLableDetails([FromQuery] string? DocEntry)
        {
            try
            {
                _logger.LogInformation("Fetching work order label details for DocEntry: {DocEntry}", DocEntry);

                if (string.IsNullOrWhiteSpace(DocEntry))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "DocEntry is required"
                    });
                }

                using var conn = _databaseContext.CreateConnection();
                conn.Open();
                _query = "call COV_GetWorkOrderLabelDetails('" + DocEntry + "')";
                var result = await conn.QueryAsync<WorkOrderLabelDto>(_query);

                var list = result?.ToList() ?? new List<WorkOrderLabelDto>();

                return Ok(new ApiResponse<IEnumerable<WorkOrderLabelDto>>
                {
                    Success = true,
                    Message = "Work order label details retrieved successfully",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching work order label details for DocEntry: {DocEntry}", DocEntry);
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error fetching work order label details"
                });
            }
        }

        [HttpGet("GetLast7DaysProduction")]
        public async Task<IActionResult> GetLast7DaysProduction()
        {
            try
            {
                _logger.LogInformation("Fetching last 7 days production data");

                using var conn = _databaseContext.CreateConnection();
                conn.Open();
                _query = "call COV_GetLast7DaysProduction";
                var result = await conn.QueryAsync<Last7DaysProductionDto>(_query);

                var list = result?.ToList() ?? new List<Last7DaysProductionDto>();

                return Ok(new ApiResponse<IEnumerable<Last7DaysProductionDto>>
                {
                    Success = true,
                    Message = "Last 7 days production data fetched successfully",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching last 7 days production data");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error fetching last 7 days data"
                });
            }
        }
    }

}