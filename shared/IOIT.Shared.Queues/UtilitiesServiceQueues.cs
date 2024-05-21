using IOIT.Shared.ViewModels.DtoQueues;

namespace IOIT.Shared.Queues
{
    public class UtilitiesServiceQueues
    {
        public class UtilitiesIdentityEmployeeCreatedQueue
        {
            public static string NameExchange = "utilities.utilities.employee.created";
            public static string NameIdentityQueue = "common.utilities.employee.created.queue";
            public DtoUtilitiesIdentityEmployeeCreatedQueue Payload { get; set; }
        }

        public class UtilitiesResidentUpdateStatusQueue
        {
            public static string NameExchange = "utilities.resident.update.status";
            public static string NameIdentityQueue = "utilities.identity.resident.update.status.queue";
            public DtoUtilitiesResidentUpdateStatusQueue Payload { get; set; }

        }

        public class UtilitiesResidentUpdateQueue
        {
            public static string NameExchange = "utilities.resident.update";
            public static string NameIdentityQueue = "utilities.identity.resident.update.queue";
            public DtoUtitlitiesResidentUpdateQueue Payload { get; set; }
        }

        public class UtilitiesServiceTechDepositStatusQueue
        {
            public static string Name = "utilities.service.tech.deposit.status.queue";
            public DtoInvoiceUtilitiesServiceTechPaidQueue Payload { get; set; }
        }

        public class UtilitiesRegConstructionDepositStatusQueue
        {
            public static string Name = "utilities.reg.construction.deposit.status.queue";
            public DtoUtilitiesRegConstructionDepositQueue Payload { get; set; }
        }

        public class UtilitiesSlaBeginQueue
        {
            public static string Name = "utilities.sla.begin.queue";
            public DtoUtilitiesSlaBeginQueue Payload { get; set; }
        }

        public class UtilitiesSlaEndQueue
        {
            public static string Name = "utilities.sla.end.queue";
            public DtoUtilitiesSlaEndQueue Payload { get; set; }
        }

        public class UtilitiesSlaContinueQueue
        {
            public static string Name = "utilities.sla.continue.queue";
            public DtoUtilitiesSlaContinueQueue Payload { get; set; }
        }
    }
}
