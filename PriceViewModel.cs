using ORAGO_CC_4.Models;

namespace ORAGO_CC_4.WraperModels;

public class PriceViewModel
{
    public PriceViewModel(List<PurchasedPartServiceType> _GlobalPurchasedPartServiceTypes, Costing _CurrentCosting)
    {
        GlobalPurchasedPartServiceTypes = _GlobalPurchasedPartServiceTypes;

        CurrentCosting = _CurrentCosting;
        StrukturblattRows = (_CurrentCosting.StrukturblattRows ?? new List<StrukturblattRow>()).ToList();
        productionYears = (_CurrentCosting.CostingEOP).Year - (_CurrentCosting.CostingSOP).Year + 1;
        // averageAnnualProductionQuantity = (productionYears == 0 || CurrentCosting.CostingParameters is null) ? 0 : 
        //     CurrentCosting.CostingParameters.Sum(p => p.IHSQuantity) * 
        //     ((CurrentCosting.CalculatePrimaryAssembly ? CurrentCosting.PrimaryAssemblyQuantity : 0) + (CurrentCosting.CalculateSecondaryAssembly ? CurrentCosting.SecondaryAssemblyQuantity : 0)) / productionYears;
    }
    public List<StrukturblattRow> StrukturblattRows { get; set; }
    public List<PurchasedPartServiceType> GlobalPurchasedPartServiceTypes { get; set; }
    public double productionYears;
    public Costing CurrentCosting { get; set; }
    public ZukaufDetail ZukaufDetails { get; set; }
    public double averageAnnualProductionQuantity 
    {
        get 
        {
            if (CurrentCosting?.QuantityScenario?.Name  == "Scenario 1 Quantity")
            {
                return (productionYears == 0 || CurrentCosting.CostingParameters is null) ? 0 : 
                        CurrentCosting.CostingParameters.Sum(p => p.Scenario1) * 
                        ((CurrentCosting.CalculatePrimaryAssembly ? CurrentCosting.PrimaryAssemblyQuantity : 0) + (CurrentCosting.CalculateSecondaryAssembly ? CurrentCosting.SecondaryAssemblyQuantity : 0)) / productionYears;
            }
            else if (CurrentCosting?.QuantityScenario?.Name == "Scenario 2 Quantity")
            {
                return (productionYears == 0 || CurrentCosting.CostingParameters is null) ? 0 : 
                        CurrentCosting.CostingParameters.Sum(p => p.Scenario2) * 
                        ((CurrentCosting.CalculatePrimaryAssembly ? CurrentCosting.PrimaryAssemblyQuantity : 0) + (CurrentCosting.CalculateSecondaryAssembly ? CurrentCosting.SecondaryAssemblyQuantity : 0)) / productionYears;
            }
            else
            {
                return (productionYears == 0 || CurrentCosting.CostingParameters is null) ? 0 : 
                        CurrentCosting.CostingParameters.Sum(p => p.IHSQuantity) * 
                        ((CurrentCosting.CalculatePrimaryAssembly ? CurrentCosting.PrimaryAssemblyQuantity : 0) + (CurrentCosting.CalculateSecondaryAssembly ? CurrentCosting.SecondaryAssemblyQuantity : 0)) / productionYears;
            }
        }
        private set { }
    }
    #region pressen
    public double PressenProductionHoursPerYear
    {
        get { return StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.PartsPerStroke * p.ProductivityPercent * p.AverageProductionQuantity != 0 ? p.AverageProductionQuantity / ((p.ProductivityPercent / 100.00) * ((p.StrokesPerMinute * 60.00) * p.PartsPerStroke)) : 0)); }
        private set { }
    }
    #region per year
    public double PressenMachineFixedPerYear
    {
        get { return StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.LotSize == 0 ? 0 : (p.MachineCostFixedPerLot / p.LotSize) * p.AverageProductionQuantity)); } /// CurrentPressenDetail.LotSize * CurrentPressenDetail.AverageProductionQuantity
        private set { }
    }
    public double PressenMachineVariablePerYear
    {
        get { return StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.LotSize == 0 ? 0 : p.MachineCostVariablePerLot / p.LotSize * p.AverageProductionQuantity)); }
        private set { }
    }
    public double PressenLaborDirectPerYear
    {
        get { return StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.LotSize == 0 ? 0 : p.LaborCostDirectPerLot / p.LotSize * p.AverageProductionQuantity)); }
        private set { }
    }
    public double PressenLaborIndirectPerYear
    {
        get { return StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.LotSize == 0 ? 0 : p.LaborCostIndirectPerLot / p.LotSize * p.AverageProductionQuantity)); }
        private set { }
    }
    public double PressenToolingAmortizationPerYear
    {
        get { return CurrentCosting.CalculatePrimaryAssembly ? StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.ToolingAmortizationPerYear)) : 0; }
        private set { }
    }
    public double PressenManufacturingPerYear
    {
        get { return PressenMachineFixedPerYear + PressenMachineVariablePerYear + PressenLaborDirectPerYear + PressenLaborIndirectPerYear + PressenToolingAmortizationPerYear; }
        private set { }
    }
    #endregion
    public double PressenProductionCostPerHour
    {
        get { return PressenProductionHoursPerYear != 0 ? PressenManufacturingPerYear / PressenProductionHoursPerYear : 0; }
        private set { }
    }
    #endregion
    #region Fuegen
    public double FuegenProductionHoursPerYear
    {
        get { return StrukturblattRows.Sum(s => s.FuegenDetails.Sum(f => f.PartsPerHour != 0 ? f.AverageProductionQuantity / f.PartsPerHour : 0)); }
        private set { }
    }

    #region Per year
    public double FuegenMachineFixedPerYear
    {
        get { return StrukturblattRows.Sum(s => s.FuegenDetails.Sum(p => p.MachineCostFixedPerLot * p.LotsPerYear)); }
        private set { }
    }
    public double FuegenMachineVariablePerYear
    {
        get { return StrukturblattRows.Sum(s => s.FuegenDetails.Sum(p => p.MachineCostVariablePerLot * p.LotsPerYear)); }
        private set { }
    }
    public double FuegenLaborDirectPerYear
    {
        get { return StrukturblattRows.Sum(s => s.FuegenDetails.Sum(p => p.LaborCostDirectPerLot * p.LotsPerYear)); }
        private set { }
    }
    public double FuegenLaborIndirectPerYear
    {
        get { return StrukturblattRows.Sum(s => s.FuegenDetails.Sum(p => p.LaborCostIndirectPerLot * p.LotsPerYear)); }
        private set { }
    }
    public double FuegenToolingAmortizationPerYear
    {
        get { return StrukturblattRows.Sum(s => s.FuegenDetails.Sum(p => p.EquipmentAmortizationPerYear)); }
        private set { }
    }
    public double FuegenConsumablePerYear
    {
        get { return StrukturblattRows.Sum(s => s.FuegenDetails.Sum(p => p.AverageProductionQuantity * p.FuegenConsumableCostPer100Pcs / 100.00)); }
        private set { }
    }
    public double FuegenManufacturingPerYear
    {
        get { return FuegenMachineFixedPerYear + FuegenMachineVariablePerYear + FuegenLaborDirectPerYear + FuegenLaborIndirectPerYear + FuegenToolingAmortizationPerYear + FuegenConsumablePerYear; }
        private set { }
    }
    #endregion
    public double FuegenProductionCostPerHour
    {
        get { return FuegenProductionHoursPerYear != 0 ? FuegenManufacturingPerYear / FuegenProductionHoursPerYear : 0; }
        private set { }
    }
    #endregion
    #region Gesamt
    #region Per year
    public double MachineFixedPerYear
    {
        get { return PressenMachineFixedPerYear + FuegenMachineFixedPerYear; }
        private set { }
    }
    public double MachineVariablePerYear
    {
        get { return PressenMachineVariablePerYear + FuegenMachineVariablePerYear; }
        private set { }
    }
    public double LaborDirectPerYear
    {
        get { return PressenLaborDirectPerYear + FuegenLaborDirectPerYear; }
        private set { }
    }
    public double LaborIndirectPerYear
    {
        get { return PressenLaborIndirectPerYear + FuegenLaborIndirectPerYear; }
        private set { }
    }
    public double ToolingAmortizationPerYear
    {
        get { return PressenToolingAmortizationPerYear + FuegenToolingAmortizationPerYear; }
        private set { }
    }
    public double ConsumablePerYear
    {
        get { return StrukturblattRows.Sum(s => s.FuegenDetails.Sum(p => p.AverageProductionQuantity * p.FuegenConsumableCostPer100Pcs / 100.00)); }
        private set { }
    }
    public double ManufacturingPerYear
    {
        get { return PressenManufacturingPerYear + FuegenManufacturingPerYear; }
        private set { }
    }
    #endregion
    #endregion
    #region Raw material
    public double MaterialTonPerYear
    {
        get { return StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.MaterialTonPerYear)); }
        private set { }
    }
    public double MaterialCostPerYear
    {
        get { return StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.MaterialCostPerYear)); }
        private set { }
    }
    public double ScrapTonPerYear
    {
        get { return StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.ScrapPerYear / 1000.00)); }
        private set { }
    }
    public double ScrapReturnPerYear
    {
        get { return StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.ScrapReturnPerYear)); }
        private set { }
    }
    public double NetMaterialCostPerYear
    {
        get { return MaterialCostPerYear - ScrapReturnPerYear; }
        private set { }
    }
    public double NetMaterialTonPerYear
    {
        get { return MaterialTonPerYear - ScrapTonPerYear; }
        private set { }
    }
    #endregion
    #region Purchasing
    public double PurchasedPartsCostPerYear
    {
        get { return StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.PurchasedPartServiceTypeId == (GlobalPurchasedPartServiceTypes.Find(s => s.Name == "Zukaufteile")).Id).Sum(z => z.PurchaseCostPerYear)); }
        private set { }
    }
    public double StandardPartsCostPerYear
    {
        get { return StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.PurchasedPartServiceTypeId == (GlobalPurchasedPartServiceTypes.Find(s => s.Name == "Normteile")).Id).Sum(z => z.PurchaseCostPerYear)); }
        private set { }
    }
    public double SurfaceTreatmentCostPerYear
    {
        get { return StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.PurchasedPartServiceTypeId == (GlobalPurchasedPartServiceTypes.Find(s => s.Name == "Oberfläche")).Id).Sum(z => z.PurchaseCostPerYear)); }
        private set { }
    }
    public double ContainerCostPerYear
    {
        get { return StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.PurchasedPartServiceTypeId == (GlobalPurchasedPartServiceTypes.Find(s => s.Name == "Behälter")).Id).Sum(z => z.PurchaseCostPerYear)); }
        private set { }
    }
    public double LogisticCostPerYear
    {
        get { return StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.PurchasedPartServiceTypeId == (GlobalPurchasedPartServiceTypes.Find(s => s.Name == "Logistik")).Id).Sum(z => z.PurchaseCostPerYear)) + (CurrentCosting.Logistik * averageAnnualProductionQuantity);}
        private set { }
    }
    public double OtherPurchaseCostPerYear
    {
        get { return StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.PurchasedPartServiceTypeId == (GlobalPurchasedPartServiceTypes.Find(s => s.Name == "andere")).Id).Sum(z => z.PurchaseCostPerYear)) + CurrentCosting.LaunchCost + CurrentCosting.RnDCost + CurrentCosting.QualityCost; }
        private set { }
    }
    public double PurchaseToolingAmortizationPerYear
    {
        get
        {
            return StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => !z.WerkzeugeDND).Sum(z => z.WerkzeugeCost) * (1.00 + 0.02 * productionYears)) +
                        StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => !z.LehrenDND).Sum(z => z.LehrenCost) * (1.00 + 0.02 * productionYears)) +
                        StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => !z.VorrichtungenDND).Sum(z => z.VorrichtungenCost) * (1.00 + 0.02 * productionYears));
        }
        private set { }
    }
    public double PurchasePartsAndServicesPerYear
    {
        get { return PurchasedPartsCostPerYear + StandardPartsCostPerYear + SurfaceTreatmentCostPerYear + ContainerCostPerYear + LogisticCostPerYear + OtherPurchaseCostPerYear + PurchaseToolingAmortizationPerYear; }
        private set { }
    }
    public double PurchaseCostPerYear
    {
        get { return NetMaterialCostPerYear + PurchasePartsAndServicesPerYear; }
        private set { }
    }
    #endregion
    public double ProductionCostBeforeSurchargesPerYear
    {
        get { return ManufacturingPerYear + PurchaseCostPerYear; }
        private set { }
    }
    #region Zuschl�gen
    public double MGKRawMaterialPerYear
    {
        get { return NetMaterialCostPerYear * (CurrentCosting.MGKRawMaterialPercent / 100.00); }
        private set { }
    }
    public double MGKPurchasingPerYear
    {   
        // get { return (PurchasedPartsCostPerYear + StandardPartsCostPerYear + SurfaceTreatmentCostPerYear + ContainerCostPerYear + LogisticCostPerYear + OtherPurchaseCostPerYear) * (CurrentCosting.MGKPurchasingPercent / 100.00); }
        // private set { }
        get { return (StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.SurchargeApplicable == true).Sum(z => z.PurchaseCostPerYear))) * (CurrentCosting.MGKPurchasingPercent / 100.00); }
        private set { }
    }
    public double ProductionScrapPerYear
    {
        get { return (ProductionCostBeforeSurchargesPerYear + MGKRawMaterialPerYear + MGKPurchasingPerYear) * (CurrentCosting.ProductionScrapPercent / 100.00); }
        private set { }
    }
    public double SEKPerYear
    {
        get { return (ProductionCostBeforeSurchargesPerYear + MGKRawMaterialPerYear + MGKPurchasingPerYear + ProductionScrapPerYear) * (CurrentCosting.SEKPercent / 100.00); }
        private set { }
    }
    #endregion
    public double ProductionCostAfterSurchargesPerYear
    {
        get { return ProductionCostBeforeSurchargesPerYear + MGKRawMaterialPerYear + MGKPurchasingPerYear + ProductionScrapPerYear + SEKPerYear; }
        private set { }
    }
    public double VVAPerYear
    {
        get { return (ProductionCostBeforeSurchargesPerYear + MGKRawMaterialPerYear + MGKPurchasingPerYear + ProductionScrapPerYear + SEKPerYear) * (CurrentCosting.VVAPercent / 100.00); }
        private set { }
    }
    public double TotalProductionCostPerYear
    {
        get { return ProductionCostAfterSurchargesPerYear + VVAPerYear; }
        private set { }
    }
    public double BaselineSalesPricePerPart
    {
        get { return averageAnnualProductionQuantity != 0 ? 100.00 * (TotalProductionCostPerYear / averageAnnualProductionQuantity) / (1 - CurrentCosting.MinMarginPercent / 100.00) : 0; }
        private set { }
    }
    public double QuotePricePerYear
    {
        get { return CurrentCosting.QuotePricePerPart * averageAnnualProductionQuantity; }
        private set { }
    }
    #region Tooling
    public double PressenWerkzeuge
    {
        get
        {
            return CurrentCosting.CalculatePrimaryAssembly ? StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.PressenTools.Where(t => t.DND && t.ToolType == "Werkzeug").Sum(t => t.TotalCost))) +
                        StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.WerkzeugeDND).Sum(z => z.WerkzeugeCost)) : 0;
        }
        private set { }
    }
    public double Werkzeuge
    {
        get { return PressenWerkzeuge; }
        private set { }
    }
    public double PressenVorrichtungen
    {
        get
        {
            return CurrentCosting.CalculatePrimaryAssembly ? StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.PressenTools.Where(t => t.DND && t.ToolType == "Transfer").Sum(t => t.TotalCost))) +
                        StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.PressenTools.Where(t => t.DND && t.ToolType == "Lehre").Sum(t => t.TotalCost))) +
                        StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.LehrenDND).Sum(z => z.LehrenCost)) : 0;
        }
        private set { }
    }
    public double FuegenVorrichtungen
    {
        get
        {
            // return CurrentCosting.CalculatePrimaryAssembly ? StrukturblattRows.Sum(s => s.FuegenDetails.Sum(f => f.FuegenEquipments.Sum(e => e.VorrichtungKosten))) +
            //             StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.VorrichtungenDND).Sum(z => z.VorrichtungenCost)) : 0;
            return  FuegenLehren + (CurrentCosting.CalculatePrimaryAssembly ? StrukturblattRows.Sum(s => s.FuegenDetails.Sum(f => f.FuegenEquipments.Sum(e => e.VorrichtungKosten))) +
                        StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.VorrichtungenDND).Sum(z => z.VorrichtungenCost)) : 0);
        }
    }
    public double Vorrichtungen
    {
        get { return PressenVorrichtungen + FuegenVorrichtungen; }
        private set { }
    }
    public double FuegenAnlage
    {
        get { return CurrentCosting.CalculatePrimaryAssembly ? StrukturblattRows.Sum(s => s.FuegenDetails.Sum(f => f.FuegenEquipments.Sum(e => e.AnlageKosten))) : 0; }
        private set { }
    }
    public double FuegenLehren
    {
        get { return CurrentCosting.CalculatePrimaryAssembly ? StrukturblattRows.Sum(s => s.FuegenDetails.Sum(f => f.FuegenEquipments.Sum(e => e.LehreKosten))) : 0; }
        private set { }
    }
    public double Anlage
    {
        get { return FuegenAnlage; }
        private set { }
    }
    public double PressenAmortizedSetup
    {
        get
        {
            return CurrentCosting.CalculatePrimaryAssembly ? StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.PressenTools.Where(t => !t.DND).Sum(t => t.TotalCost))) +
                StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => !z.WerkzeugeDND).Sum(z => z.WerkzeugeCost)) +
                StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => !z.LehrenDND).Sum(z => z.LehrenCost)) : 0;
        }
        private set { }
    }
    public double FuegenAmortizedSetup
    {
        get
        {
            return CurrentCosting.CalculatePrimaryAssembly ? StrukturblattRows.Sum(s => s.FuegenDetails.Sum(f => f.FuegenEquipments.Sum(e => e.TVAValue))) +
                        StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => !z.VorrichtungenDND).Sum(z => z.VorrichtungenCost)) : 0;
        }
        private set { }
    }
    public double AmortizedSetup
    {
        get { return PressenAmortizedSetup + FuegenAmortizedSetup; }
        private set { }
    }
    public double PressenCustomerBilledSetup
    {
        get
        {
            return CurrentCosting.CalculatePrimaryAssembly ? StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.PressenTools.Where(t => t.DND).Sum(t => t.TotalCost))) +
                StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.WerkzeugeDND).Sum(z => z.WerkzeugeCost)) +
                StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.LehrenDND).Sum(z => z.LehrenCost)) : 0;
        }
        private set { }
    }
    public double FuegenCustomerBilledSetup
    {
        get
        {
            return CurrentCosting.CalculatePrimaryAssembly ? StrukturblattRows.Sum(s => s.FuegenDetails.Sum(f => f.FuegenEquipments.Sum(e => e.VorrichtungKosten + e.LehreKosten))) +     //Anlage is not billed to customer irrespective of TVA
                        StrukturblattRows.Sum(s => s.ZukaufDetails.Where(z => z.VorrichtungenDND).Sum(z => z.VorrichtungenCost)) : 0;
        }
        private set { }
    }
    public double CustomerBilledSetup
    {
        get { return PressenCustomerBilledSetup + FuegenCustomerBilledSetup; }
        private set { }
    }
    public double PressenToolingTotal
    {
        get { return PressenWerkzeuge + PressenVorrichtungen + PressenAmortizedSetup; }
        private set { }
    }
    public double FuegenToolingTotal
    {
        get { return FuegenVorrichtungen + FuegenAnlage + FuegenLehren; }
        private set { }
    }
    public double ToolingTotal
    {
        get { return PressenToolingTotal + FuegenToolingTotal; }
        private set { }
    }
    public double PressenSelfInvest
    {
        get { return PressenToolingTotal - PressenCustomerBilledSetup; }
        private set { }
    }
    public double FuegenSelfInvest
    {
        get { return FuegenToolingTotal - FuegenCustomerBilledSetup; }
    }
    public double SelfInvest
    {
        get { return PressenSelfInvest + FuegenSelfInvest; }
        private set { }
    }
    #endregion
    public double VVSurchargeValue
    {
        get { return CurrentCosting.VVSurchargeToolingPercent * CustomerBilledSetup; }
        private set { }
    }
    public double FinancingSurchargeValue
    {
        get { return CurrentCosting.FinancingSurchargePercent * CustomerBilledSetup; }
        private set { }
    }
    public double CalculatedSetupProject
    {
        get { return CurrentCosting.AdditionalProjectCost + VVSurchargeValue + FinancingSurchargeValue + CustomerBilledSetup; }
        private set { }
    }
    public double LMELZWeight
    {
        get {  
            if(CurrentCosting.SelectionLMELZReferenceBasis == "Operating Weight")
            {
                return averageAnnualProductionQuantity != 0 ? ((MaterialTonPerYear * 1000)/averageAnnualProductionQuantity) : 0;
            }
            else
            {
                return averageAnnualProductionQuantity != 0 ? ((NetMaterialTonPerYear * 1000)/averageAnnualProductionQuantity) : 0 ;
            }
         }
        private set { }
    }
    public double LMELZCost100Pcs
    {
        get { return LMELZWeight * CurrentCosting.PriceLMELZPerKg * 100; }
        private set { }
    }
    public double QuotePriceInclLMELZ
    {
         get { return CurrentCosting.PriceLMELZPerKg != 0 ? CurrentCosting.QuotePricePerPart + LMELZCost100Pcs : 0; }
        private set { }
    }
}