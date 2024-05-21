using IOIT.Shared.ViewModels;
using IOIT.Shared.ViewModels.DtoQueues;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.Queues
{
    public class IdentityServiceQueues
    {
        public class IdentityUpdatedEmployeeQueue
        {
            public static string Name = "identity.updated.employee.queue";
            public DTOIdentityUpdatedEmployeeQueue Payload { get; set; }

        }

        #region "ApartmentMap"

        public class IdentityApartmentMapUpdatedQueue
        {
            public static string Name = "identity.apartmentmap.updated.queue";
            //public DtoCommonProjectUpdatedQueue Payload { get; set; }

        }

        #endregion

        #region "Employee"
        public class CommonEmployeeQueue
        {
            public static string NameExchange = "common.common.employee";
            public static string NameRequireSupportQueue = "common.required-support.employee.queue";
            public DtoCommonEmployeeQueue Payload { get; set; }

        }

        #endregion

        #region "Employee Map"
        public class CommonEmployeeMapQueue
        {
            public static string NameExchange = "common.common.employeeMap";
            public static string NameRequireSupportQueue = "common.required-support.employeeMap.queue";
            public DtoCommonEmployeeMapQueue Payload { get; set; }

        }

        public class CommonEmployeeMapUpdatedQueue
        {
            public static string NameExchange = "common.common.employeeMap.updated";
            public static string NameRequireSupportQueue = "common.required-support.employeeMap.updated.queue";
            public DtoCommonEmployeeMapUpdatedQueue Payload { get; set; }

        }

        #endregion

        #region "Resident"
        public class CommonResidentQueue
        {
            public static string NameExchange = "common.common.resident";
            public static string NameRequireSupportQueue = "common.required-support.resident.queue";
            public static string NameUtilitiesQueue = "common.utilities.resident.queue";

            public DtoCommonResidentQueue Payload { get; set; }

        }

        public class CommonResidentUpdateQueue
        {
            public static string NameExchange = "common.common.resident.update";
            public static string NameRequireSupportQueue = "common.required-support.resident.update.queue";
            public static string NameUtilitiesQueue = "common.utilities.resident.update.queue";

            public DtoCommonResidentUpdateQueue Payload { get; set; }

        }

        #endregion

        #region "User"
        public class CommonUserQueue
        {
            public static string NameExchange = "common.common.user";
            public static string NameRequireSupportQueue = "common.required-support.user.queue";
            public DtoCommonUserQueue Payload { get; set; }

        }

        #endregion

        #region "Apartment Map"
        public class IdentityApartmentMapQueue
        {
            public static string NameExchange = "identity.identity.apartmentMap";
            public static string NameCommonQueue = "identity.common.apartmentMap.queue";
            public DtoIdentityApartmentMapQueue Payload { get; set; }

        }
        #endregion

    }
}
