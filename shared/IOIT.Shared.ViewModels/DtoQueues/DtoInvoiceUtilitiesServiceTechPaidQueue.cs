using System;
using System.Collections.Generic;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoInvoiceUtilitiesServiceTechPaidQueue
    {
        public long ServiceTechId { get; set; }
        public DateTime? DepositDate { get; set; }
        public decimal? AmountDeposit { get; set; }
        public DepositType? DepositType { get; set; }
        public string Note { get; set; }
        public long? UserId { get; set; }
        public string Token { get; set; }
        public bool IsDeposit { get; set; }

        public DtoInvoiceUtilitiesServiceTechPaidQueue(long serviceTechId, DateTime? depositDate, 
            decimal? amountDeposit, DepositType? depositType, string note, long? userId, string token, bool isDeposit)
        {
            ServiceTechId = serviceTechId;
            DepositDate = depositDate;
            AmountDeposit = amountDeposit;
            DepositType = depositType;
            Note = note;
            UserId = userId;
            Token = token;
            IsDeposit = isDeposit;
        }
    }
}
