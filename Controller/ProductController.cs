using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPB1SFIntegration.Controller
{
    public sealed class ProductControlller : BaseController
    {
        public ProductControlller(AuthenticationController authenticationController) : base(authenticationController) { }

        public override void Execute()
        {         
            try
            {
                PricebookEntityController pricebookEntityController = new PricebookEntityController(authenticationController);

                //retrieve price list (OPLN) from SAP B1 where "U_IdSalesForce" is null (complement with your bussiness logic to filter)
                List<Model.PriceListModel> priceLists = new List<Model.PriceListModel>(); 

                foreach (Model.PriceListModel priceList in priceLists)
                {
                    string idSalesForce = string.Empty;

                    string json = string.Empty;

                    try
                    {
                        Model.PricebookEntity pricebookEntity = new Model.PricebookEntity();
                        pricebookEntity.IsActive = true;
                        pricebookEntity.Name = priceList.Name;

                        if (priceList.IdSalesForce == string.Empty)
                        {
                            idSalesForce = pricebookEntityController.Post(pricebookEntity, ref json);

                            //create an method to update OPLN."U_IdSalesForce" = idSalesForce in SAP B1 here (through servicelayer, DI API, etc)
                            //UpdateIdSalesForceOPLN()

                            SaveLog($"PriceList {priceList.Name} created in SalesForce", json, false);
                        }
                        else
                        {
                            idSalesForce = priceList.IdSalesForce;

                            pricebookEntityController.Patch(pricebookEntity, idSalesForce, new Helper.PatchListPrice());

                            SaveLog($"PriceList {priceList.Name} updated in SalesForce", json, false);
                        }                            
                    }
                    catch (Exception ex)
                    {
                        SaveLog($"Error integrating priceList {priceList.Name} in SalesForce: {ex.Message}", json, true);
                    }
                }

                ProductEntityController productEntityController = new ProductEntityController(authenticationController);

                //retrieve products here (OITM) from SAP B1 (complement with your own business logic)
                List<Model.ProductModel> products = new List<Model.ProductModel>();

                foreach (Model.ProductModel product in products)
                {
                    string idSalesForce = string.Empty;

                    string json = string.Empty;

                    try
                    {

                        Model.ProductEntity productEntity = new Model.ProductEntity();
                        productEntity.ProductCode = product.ItemCode;
                        productEntity.Name = product.ItemName;
                        productEntity.Description = product.ItemName;
                        productEntity.IsActive = product.Active == "Y";

                        idSalesForce = product.IdSalesForce;

                        //in the query to retrieve products from SAP B1 add one int column to sinalize the operation according to your business logic (e.g if U_IdSalesForce is empty, it is an insertion; if in AITM the product was changed since last integration with salesforce it is an update)
                        switch (product.Operation)
                        {
                            case 1:
                                idSalesForce = productEntityController.Post(productEntity, ref json);

                                //create an method to update OITM."U_IdSalesForce" = idSalesForce in SAP B1 here (through servicelayer, DI API, etc)
                                //UpdateIdSalesForceOITM()

                                SaveLog($"Product {product.ItemCode} created in SalesForce", json, false);
                                break;
                            case 2:
                                productEntityController.Patch(productEntity, idSalesForce, ref json);

                                SaveLog($"Product {product.ItemCode} updated in SalesForce", json, false);
                                break;
                            case 3:
                                productEntityController.Delete(productEntity, idSalesForce);

                                SaveLog($"Product {product.ItemCode} deleted in SalesForce", json, false);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        SaveLog($"Error integrating product {product.ItemCode} in SalesForce: {ex.message}", json, true);
                    }
                }

                PricebookEntryEntityController pricebookEntryEntityController = new PricebookEntryEntityController(authenticationController);

                //retrieve products prices here (ITM1) from SAP B1 (complement with your own business logic)
                List<Model.PriceListItem> priceListItens = new List<Model.PriceListItem>();

                foreach (Model.PriceListItem priceListItem in priceListItens)
                {
                    string idSalesForce = string.Empty;

                    string json = string.Empty;

                    try
                    {
                        Model.PricebookEntryEntity pricebookEntryEntity = new Model.PricebookEntryEntity();
                        pricebookEntryEntity.Pricebook2Id = priceListItem.IdSalesForceList;
                        pricebookEntryEntity.Product2Id = priceListItem.IdSalesForceProduct;
                        pricebookEntryEntity.UnitPrice = priceListItem.Price;
                        pricebookEntryEntity.IsActive = true;

                        if (priceListItem.IdSalesForce == string.Empty)
                        {
                            idSalesForce = pricebookEntryEntityController.Post(pricebookEntryEntity, ref json);

                            SaveLog($"Item price {priceListItem.PriceListName + "-" + priceListItem.ItemCode } created in SalesForce", json, false);
                        }
                        else
                        {
                            idSalesForce = priceListItem.IdSalesForce;

                            pricebookEntryEntityController.Patch(pricebookEntryEntity, idSalesForce, new Helper.PatchListPriceItem());

                            SaveLog($"Item price {priceListItem.PriceListName + "-" + priceListItem.ItemCode } update in SalesForce", json, false);
                        }

                    }
                    catch (Exception ex)
                    {
                        SaveLog($"Error integration item price {priceListItem.PriceListName + "-" + priceListItem.ItemCode } in SalesForce", json, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"General error ocurrend in excecution: {ex.Message}")
            }            
        }
    }
}
