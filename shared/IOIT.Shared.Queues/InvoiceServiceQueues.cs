using IOIT.Shared.ViewModels;
using IOIT.Shared.ViewModels.DtoQueues;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.Queues
{
    public class InvoiceServiceQueues
    {
        public class InvoicePaymentConfirmedQueue
        {
            public static string Name = "invoice.payment.confirmed.queue";
            public DtoInvoicePaymentConfirmedQueue Payload { get; set; }

        }

        public class UpdateInvoicePaymentConfirmedQueue
        {
            public static string Name = "update.invoice.payment.confirmed.queue";
            public DtoUpdateInvoicePaymentConfirmedQueue Payload { get; set; }

        }

        public class InvoicePaidRequestGroupQueue
        {
            public static string Name = "invoice.utilities.request_group.paid.queue";
            public DtoInvoiceUtilitiesRequestGroupPaidQueue Payload { get; set; }
        }

        public class InvoicePaidRequestCleaningQueue
        {
            public static string Name = "invoice.utilities.request_cleaning.paid.queue";
            public DtoInvoiceUtilitiesRequestCleaningPaidQueue Payload { get; set; }
        }

        public class InvoiceRentalApartmentRequestQueue
        {
            public static string Name = "invoice.rental_apartment_request.queue";
            public DtoInvoiceRentalApartmentRequestQueue Payload { get; set; }
        }

        public class InvoiceUpdateUserInfoFirebaseQueue
        {
            public static string Name = "invoice.indentity.userfirebase.update.queue";
            public DtoUpdateInfoFirebaseQueue Payload { get; set; }
        }
    }
}
