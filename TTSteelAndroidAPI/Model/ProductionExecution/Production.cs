using System.Text.Json.Serialization;

namespace TTSteelAndroidAPI.Model.ProductionExecution
{
    public class Production
    {
        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
        }
        public class UnitCountsDto
        {
            public int SlittingCount { get; set; }
            public int CuttingLengthCount { get; set; }
            public int RewindingCount { get; set; }
        }
        public class SourceCountsDto
        {
            public int OwnCount { get; set; }
            public int JobworkCount { get; set; }
        }
        public class ScheduleDto
        {
            public string U_SchNo { get; set; }
            public string U_CustName { get; set; }
            public string U_Grade { get; set; }
            public decimal? U_TIPSQty { get; set; }   // adjust type as needed
            public string U_CoilNo { get; set; }
        }
        public class CountDto
        {
            public int TotalCount { get; set; }
        }
        public class JobBatchDto
        {
            public string U_IPBatch { get; set; }
            public int? U_IPLn { get; set; }
            public string U_ItemCode { get; set; }
            public string U_Level { get; set; }
            public decimal? U_SchPcs { get; set; }
            public decimal? U_SchQty { get; set; }
            public decimal? U_Pkts { get; set; }
            public int? U_PcPerPkt { get; set; }
            public string U_UOM { get; set; }
            public DateTime? U_RcptDate { get; set; }
            public decimal? U_TrimWid { get; set; }
            public decimal? U_CLen { get; set; }
            public string U_CLenUOM { get; set; }
            public string U_WhseCode { get; set; }
            public string U_MillCode { get; set; }
            public string U_MillName { get; set; }
            public string U_JBCode { get; set; }
            public decimal? U_MaxYield { get; set; }
            public string U_Surface { get; set; }
            public string U_Coating { get; set; }
            public string U_Edge { get; set; }
            public string U_Oiling { get; set; }
            public string U_JBName { get; set; }
            public string U_SchNo { get; set; }
            public string U_CoilNo { get; set; }
            public string U_Grade { get; set; }
            public decimal? U_Thick { get; set; }
            public decimal? U_Width { get; set; }
            public string U_Form { get; set; }
            public string U_Type { get; set; }
            public int? DocNum { get; set; }
            public string BarcdSts { get; set; }
            public string Sts { get; set; }
        }
        public class ScheduleSizeDto
        {
            public int? U_IPLn { get; set; }
            public int? DocNum { get; set; }
            public string U_Roll { get; set; }
            public string U_IPItem { get; set; }
            public string U_IPBatch { get; set; }
            public string U_MTS { get; set; }
            public string U_WIP { get; set; }
            public string U_OPType { get; set; }
            public string U_OPItem { get; set; }
            public string EqSpec { get; set; }
            public string U_Type { get; set; }
            public string U_Grade { get; set; }
            public string U_Form { get; set; }
            public decimal? U_Thick { get; set; }
            public decimal? U_Width { get; set; }
            public decimal? U_Length1 { get; set; }
            public decimal? U_Length2 { get; set; }
            public decimal? U_Pitch { get; set; }
            public decimal? U_Pcs { get; set; }
            public decimal? U_SchPcs { get; set; }
            public decimal? U_Pkts { get; set; }
            public int? U_PcPerPkt { get; set; }
            public decimal? U_UnitWt { get; set; }
            public decimal? U_Qty { get; set; }
            public decimal? U_SchQty { get; set; }
            public string U_UOM { get; set; }
            public int? U_PassNum { get; set; }
            public decimal? U_PassLen { get; set; }
            public string U_PassUOM { get; set; }
            public string U_SplInstn { get; set; }
            public decimal? U_MinPkWt { get; set; }
            public decimal? U_MaxPkWt { get; set; }
            public string U_Surface { get; set; }
            public string U_Coating { get; set; }
            public string U_Oiling { get; set; }
            public string U_Edge { get; set; }
            public int? AssgnLn { get; set; }
            public int? AssgnOPLn { get; set; }
            public int? AssgnSOLn { get; set; }
            public string U_SODE { get; set; }
            public string U_SODN { get; set; }
            public string U_QDCDE { get; set; }
            public string U_QDCNo { get; set; }
            public string U_QDCObj { get; set; }
            public string U_CustCode { get; set; }
            public string U_CustName { get; set; }
            public string U_WhseCode { get; set; }
            public string U_VPDE { get; set; }
            public string U_VPPrcLn { get; set; }
            public string U_SeqCode { get; set; }
            public string U_Rmks { get; set; }
        }


        // Main PRDEXE object
        public class PrdExeItem
        {
            public int DocNum { get; set; }

            public int DocEntry { get; set; }
            public string U_Source { get; set; }
            //public string U_WhsCode { get; set; }
            //public string U_FGWhs { get; set; }
            // public string U_SchId { get; set; }
            //public string U_SchNo { get; set; }
            public DateTime U_SchDt { get; set; }
            public string? U_ProdType { get; set; }
            public string? U_UnitCode { get; set; }
            public DateTime? U_ProdDt { get; set; }
            public string? U_SchSts { get; set; }
            public string? U_ProcCode { get; set; }
            public string? U_OpID { get; set; }
            public string? U_Operator { get; set; }

            //public string? U_PSchNo { get; set; }
            //public string? U_PSchSts { get; set; }


            public DateTime? U_StartDate { get; set; }
            public DateTime? U_EndDate { get; set; }
            public short? U_StartTime { get; set; }
            public short? U_EndTime { get; set; }

            public string? U_WOEntry { get; set; }

            // public object U_CoilNo { get; set; }
            public string? U_ShiftA { get; set; }
            public string? U_ShiftB { get; set; }
            public string? U_ShiftC { get; set; }


            [JsonPropertyName("CCO_TRNS_PRDEXE_C1Collection")]
            public List<PrdExeC1Item> C1Collection { get; set; }

            [JsonPropertyName("CCO_TRNS_PRDEXE_C2Collection")]
            public List<PrdExeC2Item> CCO_TRNS_PRDEXE_C2Collection { get; set; }

            [JsonPropertyName("CCO_TRNS_PRDEXE_C3Collection")]
            public List<PrdExeC3Item> CCO_TRNS_PRDEXE_C3Collection { get; set; }
        }

        // C1Collection item (material / batch info)
        public class PrdExeC1Item
        {
            public int DocEntry { get; set; }
            public int LineId { get; set; }

            public string? U_Select { get; set; }
            public string? U_WONo { get; set; }
            public string? U_WOId { get; set; }
            public string? U_ItemCode { get; set; }
            public string? U_IPLn { get; set; }
            public string? U_IPItem { get; set; }
            public string? U_IPBatch { get; set; }

            public string? U_IPLevel { get; set; }
            public short? U_SchPcs { get; set; }
            public short? U_Pkts { get; set; }
            public short? U_PcPerPkt { get; set; }
            public decimal? U_SchQty { get; set; }
            public string? U_UOM { get; set; }

            public string? U_WhseCode { get; set; }
            //public string? U_MillCode { get; set; }
            //public string? U_MillName { get; set; }
            public string? U_JBCode { get; set; }
            public string? U_JBName { get; set; }
            public string? U_MaxYield { get; set; }

            public string? U_Surface { get; set; }
            public string? U_Coating { get; set; }
            public string? U_Oiling { get; set; }
            public string? U_Edge { get; set; }

            public string? U_EqSpec { get; set; }
            public string? U_Status { get; set; }
            public decimal? U_ActlQty { get; set; }
            public decimal? U_BalQty { get; set; }
            public decimal? U_RolBkQty { get; set; }
            public string? U_RolBak { get; set; } = "N";
            public DateTime? U_RcptDate { get; set; }
            public string? U_Remarks { get; set; }
            public string? U_CoilNo { get; set; }
            public short? U_StartTime { get; set; }
            public short? U_EndTime { get; set; }
            public decimal? U_Thick { get; set; }
            public decimal? U_Width { get; set; }
            public decimal? U_Length { get; set; }
            public string? U_Form { get; set; }
            public string? U_Type { get; set; }
            public string? U_Grade { get; set; }
            public string? U_Edgebur { get; set; }
            public string? U_OilStain { get; set; }
            public string? U_Coilset { get; set; }
            public string? U_Telescop { get; set; }
            public string? U_Scalmark { get; set; }
            public string? U_surfscrh { get; set; }
            public string? U_RustOxd { get; set; }
            public string? U_crosbow { get; set; }
            public string? U_Pinhole { get; set; }
            public string? U_dentgoug { get; set; }
            public string? Remark { get; set; }

        }

        // C2Collection item (operational details)
        public class PrdExeC2Item
        {
            public int DocEntry { get; set; }
            public int LineId { get; set; }

            public string? U_Select { get; set; }
            public string? U_WONo { get; set; }
            public string? U_WOId { get; set; }
            public string? U_Roll { get; set; }
            public string? U_IPLn { get; set; }
            public string? U_IPBatch { get; set; }
            public string? U_IPItem { get; set; }
            public string? U_MTS { get; set; }
            public string? U_WIP { get; set; }

            public string? U_OPLn { get; set; }
            public string? U_OPType { get; set; }
            public string? U_OPItem { get; set; }
            public string? U_CustCode { get; set; }
            public string? U_CustName { get; set; }
            public string? U_SODN { get; set; }
            public short? U_SODE { get; set; }
            public string? U_SOLn { get; set; }
            public string? U_QDCNo { get; set; }
            public string? U_QDCDE { get; set; }
            public string? U_QDCObj { get; set; }
            public string? U_OPLevel { get; set; }
            public string? U_Plan { get; set; }
            public string? U_Type { get; set; }
            public string? U_Form { get; set; }
            public string? U_Grade { get; set; }
            public string? U_FGWhs { get; set; }
            public decimal? U_SOSchQty { get; set; }
            public decimal? U_Thick { get; set; }
            public decimal? U_Width { get; set; }
            public decimal? U_Length1 { get; set; }
            public decimal? U_Length2 { get; set; }
            public decimal? U_Pitch { get; set; }
            public decimal? U_UnitWt { get; set; }
            public decimal? U_SchQty { get; set; }
            public short? U_Pcs { get; set; }
            public short? U_SchPcs { get; set; }
            public short? U_Pkts { get; set; }
            public int U_PcPerPkt { get; set; }
            public decimal? U_Qty { get; set; }
            public string? U_UOM { get; set; }
            public string? U_PassNum { get; set; }
            public decimal? U_PassLen { get; set; }
            public string? U_PassUOM { get; set; }
            public decimal? U_LastUnit { get; set; }
            public decimal? U_NextUnit { get; set; }

            public decimal? U_MinPkWt { get; set; }
            public decimal? U_MaxPkWt { get; set; }
            public string? U_Surface { get; set; }
            public string? U_Coating { get; set; }
            public string? U_Oiling { get; set; }
            public string? U_Edge { get; set; }

            public string? U_EqSpec { get; set; }
            public decimal? U_PlanOP { get; set; }
            public decimal? U_Produced { get; set; }
            public decimal? U_OpenQty { get; set; }
            public string? U_Status { get; set; }
            public string? U_CustSpec { get; set; }
            public string? U_Sequence { get; set; }
            public decimal? U_ActPkt { get; set; }
            public string? U_VPDE { get; set; }
            public string? U_VPPrcLn { get; set; }
            public string? U_VPRmks { get; set; }
            public string? U_MachCode { get; set; }
            public string? U_VPUser { get; set; }
            public string? U_Edgebur { get; set; }
            public string? U_OilStain { get; set; }
            public string? U_Coilset { get; set; }
            public string? U_Telescop { get; set; }
            public string? U_Scalmark { get; set; }
            public string? U_surfscrh { get; set; }
            public string? U_RustOxd { get; set; }
            public string? U_crosbow { get; set; }
            public string? U_Pinhole { get; set; }
            public string? U_dentgoug { get; set; }
            public string? U_Remark { get; set; }
        }

        // C3Collection item (output / result details)
        public class PrdExeC3Item
        {
            public int DocEntry { get; set; }
            public int LineId { get; set; }

            public string? U_Select { get; set; }
            public string? U_AddRow { get; set; }
            public string? U_ItemCode { get; set; }
            public string? U_AssignLn { get; set; }
            public short? U_IPPcs { get; set; }
            public decimal? U_IPQty { get; set; }
            public string? U_IPItem { get; set; }
            public string? U_OPItem { get; set; }
            public string? U_IPLn { get; set; }
            public string? U_INLn { get; set; }
            public string? U_OutLn { get; set; }
            public string? U_IPBatch { get; set; }
            public string? U_IPLevel { get; set; }
            public short? U_IPSchPcs { get; set; }
            public string? U_IPUOM { get; set; }
            public string? U_CustCode { get; set; }
            public string? U_CustName { get; set; }
            public string? U_MTS { get; set; }
            public string? U_OPLn { get; set; }
            public string? U_OPType { get; set; }
            public short? U_RBackNo { get; set; }
            public string? U_OPBatch { get; set; }
            public string? U_OPForm { get; set; }
            public string? U_OPGrade { get; set; }
            public decimal? U_OPThick { get; set; }
            public decimal? U_OPWidth { get; set; }
            public string? U_Location { get; set; }
            public decimal? U_Length1 { get; set; }
            public decimal? U_Length2 { get; set; }
            public decimal? U_Pitch { get; set; }
            public short? U_SOPcs { get; set; }
            public short? U_OPSchPcs { get; set; }
            public short? U_Pkts { get; set; }
            public short? U_PcPerPkt { get; set; }
            public decimal? U_UnitWt { get; set; }
            public decimal? U_GrossWt { get; set; }
            public decimal? U_PckgWt { get; set; }
            public decimal? U_NetWt { get; set; }
            public decimal? U_TheoWt { get; set; }
            public string? U_UOM { get; set; }
            public string? U_Whse { get; set; }
            public string? U_FGWhs { get; set; }
            public string? U_SODN { get; set; }
            public short? U_SODE { get; set; }
            public string? U_SOLn { get; set; }
            public string? U_PackID { get; set; }
            public string? U_GrpNo { get; set; }
            public string? U_QDCNo { get; set; }
            public string? U_QDCDE { get; set; }
            public string? U_QDCObj { get; set; }
            public string? U_WIP { get; set; }
            public string? U_GIDN { get; set; }
            public string? U_GIDE { get; set; }
            public string? U_GRDN { get; set; }
            public string? U_GRDE { get; set; }
            public string? U_QCSts { get; set; }
            public string? U_ExeSts { get; set; }
            public decimal? U_MinPkWt { get; set; }
            public decimal? U_MaxPkWt { get; set; }
            public string U_Surface { get; set; }
            public string? U_Coating { get; set; }
            public string? U_Oiling { get; set; }
            public string? U_Edge { get; set; }
            public string? U_CPartNo { get; set; }
            public decimal? U_PurPrc { get; set; }
            public decimal? U_PrmVal { get; set; }
            public decimal? U_ScrPrc { get; set; }
            public decimal? U_ScrVal { get; set; }
            public decimal? U_UnitPrc { get; set; }
            public string? U_selctlbl { get; set; }
            public string? U_TrnsId { get; set; }
            public string? U_EqSpec { get; set; }
            public string? U_Status { get; set; }
            public string? U_SubGR { get; set; }
            public string? U_ConSGI { get; set; }
            public string? U_CustSpec { get; set; }
            public string? U_PartNo { get; set; }
            public string? U_Seq { get; set; }
            public decimal? U_OBThick1 { get; set; }
            public decimal? U_OBThick2 { get; set; }
            public decimal? U_OBWidth1 { get; set; }
            public decimal? U_OBWidth2 { get; set; }
            public decimal? U_OBLen1 { get; set; }
            public decimal? U_OBLen2 { get; set; }
            public decimal? U_OBDig1 { get; set; }
            public decimal? U_OBDig2 { get; set; }
            public string? U_OPLevel { get; set; }
            public string? U_Type { get; set; }
            public string? U_GRRevNo { get; set; }
            public string? U_GIRevNo { get; set; }
            public object U_DwnGrd { get; set; }
            public object U_DwnItem { get; set; }

            public string? U_VPDE { get; set; }
            public string? U_VPPrcLn { get; set; }

            public string? U_MinWidth { get; set; }
            public string? U_MaxWidth { get; set; }
            public string? U_MinLen { get; set; }
            public string? U_MaxLen { get; set; }
            public string? U_MinThick { get; set; }
            public string? U_MaxThick { get; set; }

            public string? U_Attach { get; set; }

            public string? U_Bundling { get; set; }
            public short? U_ID { get; set; }
            public DateTime? U_StartDate { get; set; }
            public DateTime? U_EndDate { get; set; }
            public short? U_StartTime { get; set; }
            public short? U_EndTime { get; set; }
            public string? U_QADE { get; set; }
            public string? U_Shift { get; set; }
            public string U_HeatNo { get; set; }
            public decimal? U_OrgNetWt { get; set; }
            public decimal U_OrgGrWt { get; set; }
            public string? U_MachCode { get; set; }

            public decimal? U_OrgPkgWt { get; set; }

            public string? U_Operator { get; set; }
            public decimal? U_PWt { get; set; }
            public string? U_VPPCust { get; set; }
            public string? U_VPUser { get; set; }
            public string? U_Edgebur { get; set; }
            public string? U_OilStain { get; set; }
            public string? U_Coilset { get; set; }
            public string? U_Telescop { get; set; }
            public string? U_Scalmark { get; set; }
            public string? U_surfscrh { get; set; }
            public string? U_RustOxd { get; set; }
            public string? U_crosbow { get; set; }
            public string? U_Pinhole { get; set; }
            public string? U_dentgoug { get; set; }
            public string? U_Remark { get; set; }
        }
        public class ProductionExecutionWithC3Request
        {
            public PrdExeItem PrdExe { get; set; }
            public List<BundleDetailDto> BundleDetails { get; set; }
            public List<PrdExeC3Item> C3List { get; set; }
            public DateTime PostDate { get; set; }
            public string MachineCode { get; set; }
        }

        public class BundleDetailDto
        {
            public int LineId { get; set; }
            public int BundleQty { get; set; }
        }
        public class GoodsIssueDataDto
        {
            public int DocEntry { get; set; }
            public string Object { get; set; }
            public string U_Source { get; set; }
            public string U_SchNo { get; set; }
            public string U_SchId { get; set; }
            public string U_WhsCode { get; set; }
            public string ItemCode { get; set; }
            public decimal Quantity { get; set; }
            public string BatchNum { get; set; }
        }
        public class GoodsReceiptDataDto
        {
            public int DocEntry { get; set; }
            public string Object { get; set; }
            public string U_Source { get; set; }
            public string U_SchNo { get; set; }
            public string U_UnitCode { get; set; }
            public string U_SchId { get; set; }
            public string U_FGWhs { get; set; }
            public string U_CustCode { get; set; }
            public string U_CustName { get; set; }
            public string U_MTS { get; set; }
            public string U_WIP { get; set; }
            public string U_OPType { get; set; }
            public string U_IPItem { get; set; }
            public string U_IPBatch { get; set; }
            public string U_Whse { get; set; }
            public string U_OPItem { get; set; }
            public string U_OPBatch { get; set; }
            public string U_FGWhs2 { get; set; }  // note: column appears twice, map as needed
            public string U_SODN { get; set; }
            public int U_SODE { get; set; }
            public string U_SOLn { get; set; }
            public decimal? U_GrossWt { get; set; }
            public decimal? U_PckgWt { get; set; }
            public string U_PackID { get; set; }
            public int? U_OPSchPcs { get; set; }
            public int? U_PcPerPkt { get; set; }
            public int? U_Pkts { get; set; }
            public decimal? U_NetWt { get; set; }
            public decimal? U_UnitPrc { get; set; }
            public DateTime? U_ProdDt { get; set; }
            public string U_Location { get; set; }
            public string U_MBatch { get; set; }
            public string U_MCHallan { get; set; }
            public string U_MItem { get; set; }
            public string U_OPGrade { get; set; }
            public decimal? U_OPThick { get; set; }
            public decimal? U_OPWidth { get; set; }
            public decimal? U_Length1 { get; set; }
            public decimal? U_Length2 { get; set; }
            public string U_Type { get; set; }
            public string MotherItem { get; set; }
            public decimal? U_OBThick1 { get; set; }
            public decimal? U_OBThick2 { get; set; }
            public decimal? U_OBWidth1 { get; set; }
            public decimal? U_OBWidth2 { get; set; }
            public decimal? U_OBLen1 { get; set; }
            public decimal? U_OBLen2 { get; set; }
            public decimal? U_OBDig1 { get; set; }
            public decimal? U_OBDig2 { get; set; }
            public string U_QRmks1 { get; set; }
            public string U_QRmks2 { get; set; }
            public string U_QCSts { get; set; }
            public int? U_ID { get; set; }
            public string U_OD { get; set; }
            public string U_HeatNo { get; set; }
        }
        public class SapReceiptOIGN
        {
            public DateTime DocDate { get; set; }
            public string? Comments { get; set; }

            public string? U_DocType { get; set; }   // U_Source
            public string? U_SchNo { get; set; }
            public string? U_SrcObj { get; set; }

            public List<SapReceiptLine> DocumentLines { get; set; } = new();
        }

        public class SapReceiptLine
        {
            public string ItemCode { get; set; }
            public string WarehouseCode { get; set; }
            public decimal Quantity { get; set; }

            public List<SapBatch> BatchNumbers { get; set; } = new();
        }

        public class SapBatch
        {
            public string BatchNumber { get; set; }
            public decimal Quantity { get; set; }
        }
        public class WorkOrderLabelDto
        {
            public decimal? U_SchQty { get; set; }
            public string U_CoilNo { get; set; }
            public string U_JBName { get; set; }
            public string U_SchNo { get; set; }
            public string U_Edgebur { get; set; }
            public string U_OilStain { get; set; }
            public string U_Coilset { get; set; }
            public string U_Telescop { get; set; }
            public string U_Scalmark { get; set; }
            public string U_surfscrh { get; set; }
            public string U_RustOxd { get; set; }
            public string U_crosbow { get; set; }
            public string U_Pinhole { get; set; }
            public string U_dentgoug { get; set; }
            public string U_OPBatch { get; set; }
            public decimal? U_NetWt { get; set; }
            public decimal? U_OPThick { get; set; }
            public decimal? U_OPWidth { get; set; }
            public string U_OPGrade { get; set; }
            public int? U_SOPcs { get; set; }
            public int? U_Pkts { get; set; }
        }

        public class Last7DaysProductionDto
        {
            public string SchNo { get; set; }
            public string U_UnitCode { get; set; }
            public string CustName { get; set; }
            public string Grade { get; set; }
            public decimal OrderQty { get; set; }
            public DateTime CreateDate { get; set; }
        }

    }
}
