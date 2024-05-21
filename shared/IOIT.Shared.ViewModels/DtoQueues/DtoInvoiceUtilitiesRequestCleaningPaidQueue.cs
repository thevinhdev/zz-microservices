using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoInvoiceUtilitiesRequestCleaningPaidQueue
    {
        public long CashId { get; set; }
        public int RequestCleaningId { get; set; }
        public decimal? Money { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public List<long> Users { get; set; }
        public string UserName { get; set; }
        public long? UserId { get; set; }
        public string Token { get; set; }

        public DtoInvoiceUtilitiesRequestCleaningPaidQueue(long cashId, int requestCleaningId, decimal money, PaymentMethod paymentMethod, PaymentStatus paymentStatus, DateTime? paymentDate, List<long> users, string userName, long? userId, string token)
        {
            CashId = cashId;
            RequestCleaningId = requestCleaningId;
            Money = money;
            PaymentMethod = paymentMethod;
            PaymentStatus = paymentStatus;
            PaymentDate = paymentDate;
            Users = users;
            UserName = userName;
            UserId = userId;
            Token = token;
        }
    }
}
