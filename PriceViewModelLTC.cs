using ORAGO_CC_4.Models;

namespace ORAGO_CC_4.WraperModels;

public class PriceViewModelLTC
{
    public PriceViewModelLTC(int _year, double _PreviousYearPrice, double _PreviousRawMaterial, double _PreviousPurchasedpartsexternalservice,double _costingAverageProductionQuantityIn100s, double _PreviousFormingPlant,
                            double _PreviousPersonnelcostforming, double _PreviousAttachmentsJoining, double _PreviousCost, double _PreviousPersonnelcosts, double _Amortizationprojectstatus, List<CostingParameter> _CostingParameter,Costing _SuppliedCosting, PriceViewModel _CurrentPriceViewModel)
    {
        ProductionYear = _year;
        PreviousYearPrice = _PreviousYearPrice;
        PreviousRawMaterial = _PreviousRawMaterial;
        PreviousPurchasedpartsexternalservice = _PreviousPurchasedpartsexternalservice;
        PreviousFormingPlant = _PreviousFormingPlant;
        PreviousPersonnelcostforming = _PreviousPersonnelcostforming;
        PreviousAttachmentsJoining = _PreviousAttachmentsJoining;
        PreviousPersonnelcosts = _PreviousPersonnelcosts;
        PreviousAmortizationprojectstatus = _Amortizationprojectstatus; 
        CurrentCostingParameter = _CostingParameter;
        PreviouCost = _PreviousCost;
        CurrentPriceViewModel = _CurrentPriceViewModel;
        SuppliedCosting = _SuppliedCosting;
        costingAverageProductionQuantityIn100s =  _costingAverageProductionQuantityIn100s;
        StrukturblattRows = (SuppliedCosting.StrukturblattRows ?? new List<StrukturblattRow>()).ToList();

    }
    public int ProductionYear { get; set; }
    public List<CostingParameter> CurrentCostingParameter { get; set; } = new();
    public PriceViewModel CurrentPriceViewModel { get; set; }
    public List<StrukturblattRow> StrukturblattRows { get; set; }
    private ZukaufDetail FetchedZukaufDetails { get; set; } = new();
    public Costing SuppliedCosting { get; set; } = new();
    public double PreviousYearPrice { get; set; }
    public double PreviousRawMaterial { get; set;}
    public double PreviousFormingPlant { get; set;}
    public double PreviousPersonnelcostforming { get; set;}
    public double PreviousAttachmentsJoining { get; set;}
    public double PreviousPersonnelcosts { get; set;}
    public double PreviousAmortizationprojectstatus { get; set;}
    public double PreviouCost { get; set;}
    public double PreviousPurchasedpartsexternalservice { get; set;}
    public double  costingAverageProductionQuantityIn100s { get; set;}
    

    #region BasisCalulation
    
    public double BaseSellingPrice
    {
        get
        {
            return SuppliedCosting.QuotePricePerPart;
        }
        private set { }
    }
    public double BaseSalesPriceWithoutSavings
    {
        get
        {
            return SuppliedCosting.QuotePricePerPart;
        }
        private set { }
    }
    public double BaseRawMaterial
    {
        get
        {
            return (costingAverageProductionQuantityIn100s > 0 ? CurrentPriceViewModel.NetMaterialCostPerYear / costingAverageProductionQuantityIn100s : 0 );
        }
        private set { }
    }
    public double BasePurchasedpartsexternalservice
    {
        get
        {
            return (costingAverageProductionQuantityIn100s > 0 ? CurrentPriceViewModel.PurchasePartsAndServicesPerYear / costingAverageProductionQuantityIn100s : 0 );
        }
        private set { }
    }
    public double BasePurchasingexternalservices
    {
        get
        {
           return (BaseRawMaterial + BasePurchasedpartsexternalservice);
        }
        private set { }
    }
    public double BaseGrossprofit 
     {
        get
        {
            return (BaseSalesPriceWithoutSavings - BasePurchasingexternalservices);
        }
    }
    public double BaseFormingPlant
     {
        get
        {
            return (costingAverageProductionQuantityIn100s > 0 ? (CurrentPriceViewModel.PressenMachineFixedPerYear / costingAverageProductionQuantityIn100s) + (CurrentPriceViewModel.PressenMachineVariablePerYear / costingAverageProductionQuantityIn100s) + (CurrentPriceViewModel.PressenToolingAmortizationPerYear / costingAverageProductionQuantityIn100s) : 0);
        }
    }
    public double BasePersonnelcostforming
     {
        get
        {
            return (costingAverageProductionQuantityIn100s > 0 ? (CurrentPriceViewModel.PressenLaborDirectPerYear / costingAverageProductionQuantityIn100s) + (CurrentPriceViewModel.PressenLaborIndirectPerYear / costingAverageProductionQuantityIn100s):0);
        }
    }
    public double BaseAttachmentsJoining
     {
        get
        {
            return (costingAverageProductionQuantityIn100s > 0 ? (CurrentPriceViewModel.FuegenMachineFixedPerYear / costingAverageProductionQuantityIn100s) + (CurrentPriceViewModel.FuegenMachineVariablePerYear / costingAverageProductionQuantityIn100s) + 
                                                   (CurrentPriceViewModel.FuegenToolingAmortizationPerYear / costingAverageProductionQuantityIn100s) + (CurrentPriceViewModel.FuegenConsumablePerYear / costingAverageProductionQuantityIn100s): 0);
        }
    }
    public double BasePersonnelcosts
     {
        get
        {
            return costingAverageProductionQuantityIn100s > 0 ? ((CurrentPriceViewModel.FuegenLaborDirectPerYear / costingAverageProductionQuantityIn100s) + (CurrentPriceViewModel.FuegenLaborIndirectPerYear / costingAverageProductionQuantityIn100s)) :0;
        }
    }
    public double BaseManufacturingCosts
     {
        get
        {
            return (BaseFormingPlant + BasePersonnelcostforming + BaseAttachmentsJoining + BasePersonnelcosts);
        }
    }
    public double BaseManufacturingcostbeforeaddition
     {
        get
        {
            return (BaseManufacturingCosts + BasePurchasingexternalservices);
        }
    }
    public double BaseMGKRawMaterial
     {
        get
        {
            return (BaseRawMaterial * (SuppliedCosting.MGKRawMaterialPercent)/100);
        }
    }
    public double BaseMGKpurchasedpartsexternalservice
     {
        get
        {
            return (BasePurchasedpartsexternalservice * (SuppliedCosting.MGKPurchasingPercent)/100);
        }
    }
    public double BaseCommitteeonfinishedparts
     {
        get
        {
            return ((BaseManufacturingcostbeforeaddition + BaseMGKRawMaterial + BaseMGKpurchasedpartsexternalservice)*((SuppliedCosting.ProductionScrapPercent)/100));
        }
    }
    public double BaseSEK
     {
        get
        {
            return ((BaseManufacturingcostbeforeaddition + BaseMGKRawMaterial + BaseCommitteeonfinishedparts + BaseMGKpurchasedpartsexternalservice)*(SuppliedCosting.SEKPercent/100));
        }
    }
    public double BaseCostofproductionaftersurcharges
     {
        get
        {
            return ((BaseManufacturingcostbeforeaddition + BaseMGKRawMaterial + BaseCommitteeonfinishedparts + BaseMGKpurchasedpartsexternalservice + BaseSEK));
        }
    }
    public double BaseContributionmargin
     {
        get
        {
            return (BaseSellingPrice - BaseCostofproductionaftersurcharges);
        }
    }
    public double BaseCostsurcharge
     {
        get
        {
            return (BaseCostofproductionaftersurcharges *(SuppliedCosting.VVAPercent)/100);
        }
    }
    public double BaseResultcontributionbefore
     {
        get
        {
            return (BaseSellingPrice - BaseCost);
        }
    }
     public double BaseCost
     {
        get
        {
            return (BaseCostofproductionaftersurcharges + BaseCostsurcharge);
        }
    }
     public double BaseCalculationBasisFor
     {
        get
        {
            return BaseSellingPrice > 0 ? ((BaseSellingPrice - BasePurchasingexternalservices)/BaseSellingPrice) : 0;
        }
    }
   
    #endregion Base Calulation
    public double Savings1Proceed 
    {
        get
        {
            return -1 * PreviousYearPrice * CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Saving1Revenue ??0;
        }
        private set { }
    }
    public double Savings2GrossProfit 
    {
        get
        {
            return -1 * PreviousYearPrice * CalculationBasisFor * CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Saving2GrossProfit ??0;
        }
        private set { }
    }
    public double SalesPriceWithoutSavings
    {
        get
        {
            return PreviousYearPrice + Savings1Proceed + Savings2GrossProfit;
        }
        private set { }
    }
    public double RawMaterial
    {
        get
        {
            return PreviousRawMaterial  * (1 + CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.CommoditiesPrice ?? 0);
        }
        private set { }
    }
    public double Purchasedpartsexternalservice
    {
        get
        {
            return PreviousPurchasedpartsexternalservice * (1 + CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.ServicePurchasePrice ?? 0); 
        }
        private set { }
    }
    public double Purchasingexternalservices
    {
        get
        {
           return (RawMaterial + Purchasedpartsexternalservice);
        }
        private set { }
    }
     public double Grossprofit 
     {
        get
        {
            return (SalesPriceWithoutSavings - Purchasingexternalservices);
        }
    }
    public double Formingplants
     {
        get
        {
            return PreviousFormingPlant * (1 - CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.ProductivityForming ?? 0);
        }
    }
    public double Personnelcostforming
     {
        get
        {
            return PreviousPersonnelcostforming * (1 - (CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.ProductivityForming ?? 0) + (CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.PersonalCost ?? 0));
        }
    }
    public double  AttachmentsJoining
     {
        get
        {
            return PreviousAttachmentsJoining * (1 - CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.ProductivityJoining ?? 0);
        }
    }
    public double Personnelcosts
     {
        get
        {
            return (PreviousPersonnelcosts  * (1 - (CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.ProductivityJoining ?? 0) + (CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.PersonalCost ?? 0)));
        }
    }
    public double ManufacturingCosts
     {
        get
        {
            return (Formingplants + Personnelcostforming + AttachmentsJoining + Personnelcosts);
        }
    }
    public double Manufacturingcostbeforeaddition
     {
        get
        {
            return (ManufacturingCosts + Purchasingexternalservices);
        }
    }
    public double MGKRawMaterial
     {
        get
        {
            return (RawMaterial * (SuppliedCosting.MGKRawMaterialPercent)/100);
        }
    }
    public double MGKpurchasedpartsexternalservice
     {
        get
        {
            return (Purchasedpartsexternalservice * (SuppliedCosting.MGKPurchasingPercent)/100);
        }
    }
    public double Committeeonfinishedparts
     {
        get
        {
            return ((Manufacturingcostbeforeaddition + MGKRawMaterial + MGKpurchasedpartsexternalservice)*((SuppliedCosting.ProductionScrapPercent)/100));
        }
    }
    public double SEK
     {
        get
        {
            return ((Manufacturingcostbeforeaddition + MGKRawMaterial + Committeeonfinishedparts + MGKpurchasedpartsexternalservice)*(SuppliedCosting.SEKPercent/100));
        }
    }
    public double Costofproductionaftersurcharges
     {
        get
        {
            return ((Manufacturingcostbeforeaddition + MGKRawMaterial + Committeeonfinishedparts + MGKpurchasedpartsexternalservice + SEK));
        }
    }
    public double Contributionmargin
     {
        get
        {
            return (PreviousYearPrice - Costofproductionaftersurcharges);
        }
    }
    public double Costsurcharge
     {
        get
        {
            return (Costofproductionaftersurcharges *(SuppliedCosting.VVAPercent)/100);
        }
    }
    public double Cost
     {
        get
        {
            return (Costofproductionaftersurcharges + Costsurcharge);
        }
    }
    public double Resultcontributionbefore
     {
        get
        {
            return (SalesPriceWithoutSavings - Cost);
        }
    }
     public double Changes 
    {
        get
        {
             return (Cost - PreviouCost );
        }
        private set { }
    }
    public double CalculationBasisFor 
    {
        get
        {
             return ((PreviousYearPrice - Purchasingexternalservices)/PreviousYearPrice) ;
        }
        private set { }
    }
    public double DebtServiceShares 
    {
        get
        {
             return costingAverageProductionQuantityIn100s > 0 ? ((StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.CapitalServiceShare)) + 
                    StrukturblattRows.Sum(s => s.FuegenDetails.Sum(p => p.DebtserviceshareShareofperyear)) + 
                    CurrentPriceViewModel.PressenToolingAmortizationPerYear + CurrentPriceViewModel.FuegenToolingAmortizationPerYear +
                    CurrentPriceViewModel.PurchaseToolingAmortizationPerYear)/costingAverageProductionQuantityIn100s):0;
        }
        private set { }
    }

     public double Saleswithofferprice 
    {
        get
        {
            if (SuppliedCosting.QuantityScenario?.Name == "Scenario 1 Quantity") //Scenario1
            {
                return ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario1 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * 
                                    SuppliedCosting.QuotePricePerPart  / 100.00;
            }
            else if (SuppliedCosting.QuantityScenario?.Name == "Scenario 2 Quantity") //Scenario2
            {
                return ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario2 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * 
                                    SuppliedCosting.QuotePricePerPart  / 100.00;
            }
            else
            {
                return ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.IHSQuantity ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * 
                                    SuppliedCosting.QuotePricePerPart  / 100.00;
            }
        }
        private set { }
    }
    public double ReductionsSavings1_2
    {
        get
        {
            if (SuppliedCosting.QuantityScenario?.Name == "Scenario 1 Quantity") //Scenario1
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario1 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * 
                                    SalesPriceWithoutSavings / 100 ) - Saleswithofferprice;
            }
            else if (SuppliedCosting.QuantityScenario?.Name == "Scenario 2 Quantity") //Scenario2
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario2 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * 
                                    SalesPriceWithoutSavings / 100 ) - Saleswithofferprice;
            }
            else 
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.IHSQuantity ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * 
                                    SalesPriceWithoutSavings / 100 ) - Saleswithofferprice;
            }
            
        }
        private set { }
    }
    public double ReductionsSavings3
    {
        get
        {
             return (-1 * (ReductionsSavings1_2 + Saleswithofferprice) * CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Saving3Sales ??0);
        }
        private set { }
    }
    public double NetSales
    {
        get
        {
             return (ReductionsSavings3 + ReductionsSavings1_2 + Saleswithofferprice);
        }
        private set { }
    }
    public double InEURpieces
    {
        get
        {
            if (SuppliedCosting.QuantityScenario?.Name == "Scenario 1 Quantity") //Scenario1
            {
                return (NetSales / ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario1 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0)))) * 100;
            }
            else if (SuppliedCosting.QuantityScenario?.Name == "Scenario 2 Quantity") //Scenario2
            {
                return (NetSales / ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario2 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0)))) * 100;
            }
            else
            {
                return (NetSales / ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.IHSQuantity ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0)))) * 100;
            }                          
        }
        private set { }
    }
   public double PurchasingThirdpartyservices
   {
        get
        {
            if (SuppliedCosting.QuantityScenario?.Name == "Scenario 1 Quantity") //Scenario1
            {
                return ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario1 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) *(Purchasingexternalservices) / 100;
            } 
            else if (SuppliedCosting.QuantityScenario?.Name == "Scenario 2 Quantity") //Scenario2
            {
                return ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario2 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) *(Purchasingexternalservices) / 100;
            } 
            else
            {
                return ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.IHSQuantity ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) *(Purchasingexternalservices) / 100;
            } 
        }
        private set { }
    }
    public double Grosprofit
    {
        get
        {
             return (NetSales - PurchasingThirdpartyservices);
        }
        private set { }
    }
    public double InPercentofnetsales
    {
        get
        {
             return (NetSales != 0 ? Grosprofit / NetSales : 0 );
        }
        private set { }
    }
    public double manufacturingCostt
    {
        get
        {
             return (((CurrentCostingParameter.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.IHSQuantity ??0) * (SuppliedCosting.SecondaryAssemblyQuantity > 0 ? 2 : 1) * ManufacturingCosts)/100);
        }
        private set { }
    }
    public double MKGCommitteeSEK
    {
        get
        {
            if (SuppliedCosting.QuantityScenario?.Name == "Scenario 1 Quantity") //Scenario1
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario1 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * (SEK + Committeeonfinishedparts + MGKpurchasedpartsexternalservice + MGKRawMaterial))/100;
            } 
            else if (SuppliedCosting.QuantityScenario?.Name == "Scenario 2 Quantity") //Scenario2
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario2 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * (SEK + Committeeonfinishedparts + MGKpurchasedpartsexternalservice + MGKRawMaterial))/100;
            } 
            else
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.IHSQuantity ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * (SEK + Committeeonfinishedparts + MGKpurchasedpartsexternalservice + MGKRawMaterial))/100;
            } 
        }
        private set { }
    }
    public double Manufacturingcostsaccordingtosurcharges
    {
        get
        {
             return ( MKGCommitteeSEK + manufacturingCostt + PurchasingThirdpartyservices );
        }
        private set { }
    }
    public double Contributionmargn
    {
        get
        {
             return ( NetSales - Manufacturingcostsaccordingtosurcharges );
        }
        private set { }
    }
    public double INPercentofNetSales
    {
        get
        {
             return (NetSales != 0 ? Contributionmargn / NetSales :0);
        }
        private set { }
    }
   public double VVCostsurcharge
    {
        get
        {
            if (SuppliedCosting.QuantityScenario?.Name == "Scenario 1 Quantity") //Scenario1
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario1 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * Costsurcharge)/100;
            } 
            if (SuppliedCosting.QuantityScenario?.Name == "Scenario 2 Quantity") //Scenario2
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario2 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * Costsurcharge)/100;
            } 
            else
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.IHSQuantity ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * Costsurcharge)/100;
            } 
        }
        private set { }
    }
     public double CostPrice
    {
        get
        {
             return (VVCostsurcharge + Manufacturingcostsaccordingtosurcharges);
        }
        private set { }
    }
    public double ResultContribution
    {
        get
        {
             return (NetSales - CostPrice);
        }
        private set { }
    }
    public double INEurPieses
    {
        get
        {
            if (SuppliedCosting.QuantityScenario?.Name == "Scenario 1 Quantity") //Scenario1
            {
                return (ResultContribution / ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario1 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0)))) * 100;
            } 
            else if (SuppliedCosting.QuantityScenario?.Name == "Scenario 2 Quantity") //Scenario2
            {
                return (ResultContribution / ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario2 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0)))) * 100;
            } 
            else
            {
                return (ResultContribution / ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.IHSQuantity ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0)))) * 100;
            } 
            
        }
        private set { }
    }
    public double InOFnetSales
    {
        get
        {
             return (NetSales !=0 ? ResultContribution / NetSales :0);
        }
        private set { }
    }
    public double Debtserviceviaparts
    {
        get
        {
            if (SuppliedCosting.QuantityScenario?.Name == "Scenario 1 Quantity") //Scenario1
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario1 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * (DebtServiceShares))/100;
            } 
            else if (SuppliedCosting.QuantityScenario?.Name == "Scenario 2 Quantity") //Scenario2
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario2 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * (DebtServiceShares))/100;
            } 
            else
            {
                return (((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.IHSQuantity ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))) * (DebtServiceShares))/100;
            } 
        }
        private set { }
    }
    public double EBITDA
    {
        get
        {
             return (ResultContribution + Debtserviceviaparts);
        }
        private set { }
    }
    public double INEurPIESES
    {
        get
        {
            if (SuppliedCosting.QuantityScenario?.Name == "Scenario 1 Quantity") //Scenario1
            {
                return (EBITDA / ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario1 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))))*100;
            } 
            else if (SuppliedCosting.QuantityScenario?.Name == "Scenario 2 Quantity") //Scenario2
            {
                return (EBITDA / ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.Scenario2 ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))))*100;
            } 
            else
            {
                return (EBITDA / ((SuppliedCosting.CostingParameters?.FirstOrDefault(p => p.ProductionYear == ProductionYear)?.IHSQuantity ??0) * 
                            ((SuppliedCosting.CalculatePrimaryAssembly ? SuppliedCosting.PrimaryAssemblyQuantity : 0) + 
                                (SuppliedCosting.CalculateSecondaryAssembly ? SuppliedCosting.SecondaryAssemblyQuantity : 0))))*100;
            } 
        }
        private set { }
    }
    public double InOFnetSALES
    {
        get
        {
             return (NetSales !=0 ? EBITDA / NetSales :0);
        }
        private set { }
    }
    public double InvestmentsForming
    {
        get
        {
             return ( StrukturblattRows.Sum(s => s.PressenDetails.Sum(p => p.PressenTools.Where(t => !t.DND).Sum(t => t.TotalCost))));
        }
        private set { }
    }
   public double InvestmentsPurchase
    {
        get
        {
             return ( (StrukturblattRows.Sum(z => z.ZukaufDetails.Where(d => !d.WerkzeugeDND).Sum(d => d.WerkzeugeCost))) + 
                      (StrukturblattRows.Sum(z => z.ZukaufDetails.Where(d => !d.LehrenDND).Sum(d => d.LehrenCost))) + 
                      (StrukturblattRows.Sum(z => z.ZukaufDetails.Where(d => !d.VorrichtungenDND).Sum(d => d.VorrichtungenCost)))) ;
        }
        private set { }
    }   
     public double InvestmentsJoiningAppendix
    {
        get
        {
             return StrukturblattRows.Sum(s => s.FuegenDetails.Sum(f => f.InvestmentPortionTP));
        }
        private set { }
    }
     public double InvestmentsJoiningTVA
    {
        get
        {
             return StrukturblattRows.Sum(s => s.FuegenDetails.Sum(f => f.FuegenEquipments.Sum(e => e.TVAValue)));
        }
        private set { }
    }
    public double Investment
    {
        get
        {
             return (InvestmentsForming + InvestmentsPurchase + InvestmentsJoiningAppendix + InvestmentsJoiningTVA);
        }
        private set { }
    }
    public double Amortizationprojectstatus
    {
        get
        {
             return (PreviousAmortizationprojectstatus + EBITDA);
        }
        private set { }
    }
    public double AmortizationInMonths
    {
        get
        {
             return (Amortizationprojectstatus < 0 ? 12 :
                    PreviousAmortizationprojectstatus > 0 ? 0 :
                    EBITDA == 0 ? 0 :
                    - PreviousAmortizationprojectstatus / EBITDA * 12);
        }
        private set { }
    }
    public double IstContributionMargin
    {
        get
        {
             return (NetSales != 0 ? Investment/NetSales :0);
        }
        private set { }
    }
    public double IstResultOperatingResources
    {
        get
        {
             return (SuppliedCosting.QuoteTooling - CurrentPriceViewModel.CustomerBilledSetup);
        }
        private set { }
    }
}