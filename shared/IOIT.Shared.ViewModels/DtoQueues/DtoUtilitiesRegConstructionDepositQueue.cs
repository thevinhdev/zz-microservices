using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoUtilitiesRegConstructionDepositQueue
    {
        public long RegisterConstructionFormId { get; set; }
        public DateTime? DepositDate { get; set; }
        public decimal? AmountDeposit { get; set; }
        public DepositType? DepositType { get; set; }
        public string Note { get; set; }
        public long? UserId { get; set; }
        public string Token { get; set; }

        public DtoUtilitiesRegConstructionDepositQueue (long registerConstructionFormId, DateTime? depositDate, decimal? amountDeposit, DepositType? depositType, string note, long? userId, string token)
        {
            RegisterConstructionFormId = registerConstructionFormId;
            DepositDate = depositDate;
            AmountDeposit = amountDeposit;
            DepositType = depositType;
            Note = note;
            UserId = userId;
            Token = token;
        }
    }
}
