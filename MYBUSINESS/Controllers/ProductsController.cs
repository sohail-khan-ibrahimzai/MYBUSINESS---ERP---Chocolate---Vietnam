﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.UI.WebControls;
using MYBUSINESS.CustomClasses;
using MYBUSINESS.Models;
using System.Data.SqlClient;
using System.Configuration;
using OfficeOpenXml;
namespace MYBUSINESS.Controllers
{
    public class ProductsController : Controller
    {
        private BusinessContext db = new BusinessContext();
        private DAL DAL = new DAL();
        public string myConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public class TableType
        {
            public string ColumnName { get; set; }
            public string DataType { get; set; }
        }
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadExcelFile()
        {
            //return Json("return");
            string userId = User.Identity.GetUserId();


            if (Request.Files.Count > 0)
            {

                List<string> allColumns = new List<string>();
                List<string> allRows = new List<string>();
                List<string> allErrors = new List<string>();
                var tableName = "Product";


                try
                {
                    HttpFileCollectionBase postedFiles = Request.Files;
                    HttpPostedFileBase postedFile = postedFiles[0];
                    string filePath = string.Empty;
                    var noOfCol = 0;
                    var noOfRow = 0;
                    var tableType = new List<TableType>();

                    if (postedFile != null)
                    {
                        string path = Server.MapPath("~/Uploads/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        filePath = path + Path.GetFileName(postedFile.FileName);
                        string extension = Path.GetExtension(postedFile.FileName);
                        postedFile.SaveAs(filePath);



                        using (SqlConnection conn = new SqlConnection(myConnectionString))
                        using (SqlCommand cmd = new SqlCommand("SELECT COLUMN_NAME,DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "' and UPPER(COLUMN_NAME) <> 'Id' and UPPER(COLUMN_NAME) <> 'DateCreated'  and UPPER(COLUMN_NAME) <> 'DateModified'", conn))
                        {
                            SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                            adapt.SelectCommand.CommandType = CommandType.Text;

                            DataTable dt = new DataTable();
                            adapt.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    var tableTpe = new TableType();
                                    tableTpe.ColumnName = dt.Rows[i][0].ToString();
                                    tableTpe.DataType = dt.Rows[i][1].ToString();
                                    tableType.Add(tableTpe);
                                }

                            }
                        }

                        string fileName = postedFile.FileName;
                        string fileContentType = postedFile.ContentType;
                        byte[] fileBytes = new byte[postedFile.ContentLength];
                        var data = postedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(postedFile.ContentLength));

                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage(postedFile.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            noOfCol = workSheet.Dimension.End.Column;
                            noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 1; rowIterator <= noOfRow; rowIterator++)
                            {
                                for (int colIterator = 1; colIterator <= noOfCol; colIterator++)
                                {
                                    if (rowIterator == 1)
                                        allColumns.Add(Convert.ToString(workSheet.Cells[1, colIterator].Value));
                                    else
                                    {
                                        allRows.Add(Convert.ToString(workSheet.Cells[rowIterator, colIterator].Value));
                                    }
                                }
                            }
                        }
                    }


                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime mydatatime;
                    string Events = string.Empty;
                    bool isColEmpty = false;
                    string rowValue = string.Empty;
                    if (allErrors.Count == 0)
                    {

                        try
                        {


                            int curcol = 0;
                            Product uploadingProduct;
                            Product updatingProduct;
                            var isUpdatingProduct = false;
                            List<Product> LstUploadingProduct = new List<Product>();
                            List<Product> LstUploadingProductFiltered = new List<Product>();
                            List<Product> LstUpdateProduct = new List<Product>();
                            decimal maxId = db.Products.DefaultIfEmpty().Max(p => p == null ? 0 : p.Id);

                            for (int u = 0; u < (noOfRow - 1); u++)
                            {
                                isColEmpty = false;
                                //string query = "insert into " + tableName + " select '" + Guid.NewGuid() + "', ";
                                int colNo = 0;
                                uploadingProduct = new Product();
                                updatingProduct = new Product();

                                for (int o = colNo; o < noOfCol; o++)
                                {
                                    uploadingProduct.Id = maxId + u + 1;
                                    uploadingProduct.PerPack = 1;
                                    uploadingProduct.Saleable = true;
                                    uploadingProduct.IsService = false;
                                    uploadingProduct.Stock = 0;
                                    uploadingProduct.ShowIn = "P";
                                    //curcol = u * 5 + colNo;
                                    curcol = u * noOfCol + colNo;



                                    if (allColumns[o].Trim().ToUpper() == "BARCODE".Trim().ToUpper())
                                    {

                                        if (db.Products.ToList().Any(x => x.BarCode == allRows[curcol]))
                                        {
                                            //updatingProduct = db.Products.ToList().FirstOrDefault(x => x.BarCode == allRows[curcol]);
                                            //isUpdatingProduct = true;
                                            //updatingProduct.BarCode = allRows[curcol];
                                        }
                                        else
                                        {
                                            isUpdatingProduct = false;
                                            uploadingProduct.BarCode = allRows[curcol];
                                        }

                                    }
                                    if (allColumns[o].Trim().ToUpper() == "Product Name".Trim().ToUpper())
                                    {
                                        //if (isUpdatingProduct) updatingProduct.StyleCode = allRows[curcol];
                                        /*else*/
                                        uploadingProduct.Name = allRows[curcol];
                                    }

                                    if (allColumns[o].Trim().ToUpper() == "Purchase Price".Trim().ToUpper())
                                    {
                                        //if (isUpdatingProduct) updatingProduct.Name = allRows[curcol];
                                        /*else*/
                                        if (!string.IsNullOrEmpty(allRows[curcol].Trim())) uploadingProduct.PurchasePrice = decimal.Parse(allRows[curcol]);
                                    }

                                    if (allColumns[o].Trim().ToUpper() == "Sale Price".Trim().ToUpper())
                                    {
                                        //if (isUpdatingProduct) updatingProduct.Name = allRows[curcol];
                                        /*else*/
                                        if (!string.IsNullOrEmpty(allRows[curcol].Trim())) uploadingProduct.SalePrice = decimal.Parse(allRows[curcol]);

                                    }
                                    if (allColumns[o].Trim().ToUpper() == "Whole Sale Price".Trim().ToUpper())
                                    {
                                        //if (isUpdatingProduct) updatingProduct.Name = allRows[curcol];
                                        /*else*/
                                        if (!string.IsNullOrEmpty(allRows[curcol].Trim())) uploadingProduct.WholeSalePrice = decimal.Parse(allRows[curcol]);

                                    }
                                    if (allColumns[o].Trim().ToUpper() == "Remarks".Trim().ToUpper())
                                    {
                                        //if (isUpdatingProduct) updatingProduct.Name = allRows[curcol];
                                        /*else*/
                                        uploadingProduct.Remarks = allRows[curcol];
                                    }

                                    ++colNo;
                                }

                                //string pName = string.Empty;

                                if (!string.IsNullOrEmpty(uploadingProduct.Name.Trim()) && db.Products.Where(x => x.Name.ToUpper().Trim() == uploadingProduct.Name.ToUpper().Trim()).ToList().Count == 0
                                && LstUploadingProduct.Where(x => x.Name.ToUpper().Trim() == uploadingProduct.Name.ToUpper().Trim()).ToList().Count == 0)
                                {

                                    LstUploadingProduct.Add(uploadingProduct);

                                }



                            }
                            db.Products.AddRange(LstUploadingProduct);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }

                    }


                    //return Json(new { allColumns = allColumns, allRows = allRows, allErrors = allErrors, noOfRows = noOfRow }, JsonRequestBehavior.AllowGet);
                    return Json(new { allErrors = allErrors, noOfRows = noOfRow }, JsonRequestBehavior.AllowGet);


                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No file selected.");
            }
        }

        // GET: Products
        public ActionResult Index()
        {
            var storeId = Session["StoreId"] as string;
            if (storeId == null)
            {
                return RedirectToAction("StoreNotFound", "UserManagement");
            }
            var parseId = int.Parse(storeId);
            ViewBag.Suppliers = DAL.dbSuppliers;
            return View(DAL.dbProducts.Where(x => x.StoreId == parseId).ToList());
        }

        public ActionResult SearchData(string suppId)
        {
            if (suppId.Trim() == string.Empty)
            {

                return PartialView("_SelectedProducts", DAL.dbProducts.OrderBy(i => i.Id).ToList());

            }
            else
            {
                int intSuppId = Int32.Parse(suppId.Trim());

                IQueryable<Product> selectedProducts = null;
                //selectedProducts = db.Products.Where(p => p.SupplierId == intSuppId);
                return PartialView("_SelectedProducts", selectedProducts.OrderBy(i => i.Id).ToList());

            }

        }


        public ActionResult Create()
        {
            var storeId = Session["StoreId"] as string;
            if (storeId == null)
            {
                return RedirectToAction("StoreNotFound", "UserManagement");
            }
            //int maxId = db.Products.Max(p => p.Id);
            decimal maxId = db.Products.DefaultIfEmpty().Max(p => p == null ? 0 : p.Id);
            maxId += 1;
            ViewBag.SuggestedId = maxId;
            ViewBag.Suppliers = DAL.dbSuppliers;
            Product prod = new Product();

            prod.PurchasePrice = 0;
            prod.SalePrice = 0;
            prod.Stock = 0;

            prod.Saleable = true;
            return View(prod);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PurchasePrice,SalePrice,WholeSalePrice,Stock,Saleable,PerPack,IsService,ShowIn,BarCode,Remarks")] Product product)
        {
            if (product.Stock == null)
            {
                product.Stock = 0;
            }

            if (product.PerPack == null || product.PerPack == 0)
            {
                product.PerPack = 1;
            }

            //if(product.IsService==true)
            //{
            //product.PurchasePrice = 0;
            //product.Stock = 0;
            //}
            //else
            //{
            product.Stock = product.Stock * product.PerPack;
            //}


            if (ModelState.IsValid)
            {
                db.Products.Add(product);

                if (product.Stock > 0)
                {
                    decimal maxId1 = (int)db.POes.DefaultIfEmpty().Max(p => p == null ? 0 : p.POSerial);
                    maxId1 += 1;
                    Employee emp = (Employee)Session["CurrentUser"];
                    decimal EmployeeId = emp.Id;

                    PO pO = new PO { Id = System.Guid.NewGuid().ToString().ToUpper(), POSerial = maxId1, BillAmount = 0, BillPaid = 0, Discount = 0, Balance = 0, PrevBalance = 0, Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time")), PurchaseReturn = false, SupplierId = 0, PurchaseOrderAmount = 0, PurchaseOrderQty = product.Stock, PaymentMethod = "Cash", EmployeeId = EmployeeId, BankAccountId = "1" };
                    db.POes.Add(pO);

                    POD pOD = new POD { POId = pO.Id, PODId = 1, ProductId = product.Id, OpeningStock = 0, Quantity = (int)product.Stock, PurchasePrice = 0, PerPack = 1, IsPack = true, SaleType = false };
                    db.PODs.Add(pOD);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //in case any error
            decimal maxId = db.Products.DefaultIfEmpty().Max(p => p == null ? 0 : p.Id);
            maxId += 1;
            ViewBag.SuggestedId = maxId;
            ViewBag.Suppliers = DAL.dbSuppliers;
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);
            product.Stock = product.Stock / product.PerPack;
            //ViewBag.SuppName = product.Supplier.Name;
            if (product == null)
            {
                return HttpNotFound();
            }
            List<MyUnitType> myUnitTypeOptionList = new List<MyUnitType> {
                            new MyUnitType {
                                Text = "Product",
                                Value = "false"
                            },
                            new MyUnitType {
                                Text = "Service",
                                Value = "true"
                            }
                        };
            ViewBag.UnitTypeOptionList = myUnitTypeOptionList;
            return View(product);
        }
        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PurchasePrice,SalePrice,WholeSalePrice,Stock,Saleable,PerPack,IsService,ShowIn,BarCode,Remarks")] Product product)
        {
            //Product prd = db.Products.Where(x => x.Id == product.Id).FirstOrDefault();
            //product.SuppId = prd.SuppId;
            if (product.Stock == null)
            {
                product.Stock = 0;
            }

            if (product.PerPack == null || product.PerPack == 0)
            {
                product.PerPack = 1;
            }

            product.Stock = product.Stock * product.PerPack;

            decimal StockInDB = (decimal)db.Products.AsNoTracking().FirstOrDefault(x => x.Id == product.Id).Stock;
            if (ModelState.IsValid)
            {

                if (product.Stock > StockInDB)
                {
                    decimal maxId1 = (int)db.POes.DefaultIfEmpty().Max(p => p == null ? 0 : p.POSerial);
                    maxId1 += 1;
                    Employee emp = (Employee)Session["CurrentUser"];
                    decimal EmployeeId = emp.Id;

                    PO pO = new PO { Id = System.Guid.NewGuid().ToString().ToUpper(), POSerial = maxId1, BillAmount = 0, BillPaid = 0, Discount = 0, Balance = 0, PrevBalance = 0, Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time")), PurchaseReturn = false, SupplierId = 0, PurchaseOrderAmount = 0, PurchaseOrderQty = (product.Stock - StockInDB), PaymentMethod = "Cash", EmployeeId = EmployeeId, BankAccountId = "1" };
                    db.POes.Add(pO);

                    POD pOD = new POD { POId = pO.Id, PODId = 1, ProductId = product.Id, OpeningStock = StockInDB, Quantity = (int)(product.Stock - StockInDB), PurchasePrice = 0, PerPack = 1, IsPack = true, SaleType = false };
                    db.PODs.Add(pOD);
                }

                if (product.Stock < StockInDB)
                {
                    decimal maxId1 = (int)db.POes.DefaultIfEmpty().Max(p => p == null ? 0 : p.POSerial);
                    maxId1 += 1;
                    Employee emp = (Employee)Session["CurrentUser"];
                    decimal EmployeeId = emp.Id;

                    PO pO = new PO { Id = System.Guid.NewGuid().ToString().ToUpper(), POSerial = maxId1, BillAmount = 0, BillPaid = 0, Discount = 0, Balance = 0, PrevBalance = 0, Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time")), PurchaseReturn = true, SupplierId = 0, PurchaseOrderAmount = 0, PurchaseOrderQty = (StockInDB - product.Stock), PaymentMethod = "Cash", EmployeeId = EmployeeId, BankAccountId = "1" };
                    db.POes.Add(pO);

                    POD pOD = new POD { POId = pO.Id, PODId = 1, ProductId = product.Id, OpeningStock = StockInDB, Quantity = (int)(StockInDB - product.Stock), PurchasePrice = 0, PerPack = 1, IsPack = true, SaleType = true };
                    db.PODs.Add(pOD);
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }


        public ActionResult CreateService()
        {
            //int maxId = db.Products.Max(p => p.Id);
            decimal maxId = db.Products.DefaultIfEmpty().Max(p => p == null ? 0 : p.Id);
            maxId += 1;
            ViewBag.SuggestedId = maxId;
            ViewBag.Suppliers = DAL.dbSuppliers;
            Product prod = new Product();

            prod.PurchasePrice = 0;
            prod.SalePrice = 0;
            prod.Stock = 0;

            prod.Saleable = true;
            return View(prod);
        }

        public ActionResult EditService(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);
            product.Stock = product.Stock / product.PerPack;
            //ViewBag.SuppName = product.Supplier.Name;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Product product = db.Products.Find(id);
            bool isPresent = false;
            if (db.PODs.FirstOrDefault(x => x.ProductId == id) != null || db.SODs.FirstOrDefault(x => x.ProductId == id) != null)
            {
                isPresent = true;
            }

            if (isPresent == false)
            {
                db.Products.Remove(product);
            }
            else
            {
                product.Status = "D";
                db.Entry(product).Property(x => x.Status).IsModified = true;

            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
