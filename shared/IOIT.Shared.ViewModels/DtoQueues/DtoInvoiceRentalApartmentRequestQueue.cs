using System;
using System.Collections.Generic;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoInvoiceRentalApartmentRequestQueue
    {
        public long CashId { get; set; }
        public int RequestGroupId { get; set; }
        public decimal? Money { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Token { get; set; }

        public DtoInvoiceRentalApartmentRequestQueue(long cashId, int requestGroupId, decimal? money, PaymentStatus paymentStatus, DateTime? paymentDate, PaymentMethod paymentMethod, string token)
        {
            CashId = cashId;
            RequestGroupId = requestGroupId;
            Money = money;
            PaymentStatus = paymentStatus;
            PaymentDate = paymentDate;
            PaymentMethod = paymentMethod;
            Token = token;
        }
    }
}
