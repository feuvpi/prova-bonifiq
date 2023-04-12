namespace ProvaPub.Models
{
    public class PaymentResult
    {
        #region [ PROPRIEDADES ]
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public PaymentStatus Status { get; set; }
        #endregion

        public enum PaymentStatus
        {
            Success,
            Failure,
            Pending
        }


    }
}
