using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.ViewModels
{
    public class DtoUpdateInvoicePaymentConfirmedQueue
    {
        public string vpc_Amount { get; set; }
        public string vpc_OrderAmount { get; set; }
        public string vpc_TxnResponseCode { get; set; }
        public string vpc_TransactionNo { get; set; }
        public string vpc_Message { get; set; }
        public string vpc_Card { get; set; }
        public string vpc_PayChannel { get; set; }
        public string vpc_CardUid { get; set; }
        public string vpc_SecureHash { get; set; }
        public string vpc_MerchTxnRef { get; set; }
    }
}
