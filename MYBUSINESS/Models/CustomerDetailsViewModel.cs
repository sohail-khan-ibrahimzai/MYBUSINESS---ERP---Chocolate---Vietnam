namespace MYBUSINESS.Models
{
    public class CustomerDetailsViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string SalePrice { get; set; }
        public string PurchasePrice { get; set; }
        public string Quantity { get; set; }
        
    }  
    public class PaymentDetailsViewModel
    {
        public string CardVndAmount { get; set; }
        public string LeftToPayVndBalance { get; set; }

    }
    public class CustomerUpdateModel
    {
        public string VatNumber { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public string CustomerAddress { get; set; }
    }
}
