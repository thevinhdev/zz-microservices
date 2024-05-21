using IOIT.Shared.ViewModels.DtoQueues;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOIT.Shared.Queues
{
    public class CommonServiceQueues
    {
        #region "Project"
        public class CommonProjectCreatedQueue
        {
            public static string NameExchange = "common.common.project.created";
            public static string NameInvoiceQueue = "common.invoice.project.created.queue";
            public static string NameRequireSupportQueue = "common.required-support.project.created.queue";
            public static string NameIdentityQueue = "common.identity.project.created.queue";
            public static string NameUtilitiesQueue = "common.utilities.project.created.queue";
            public DtoCommonProjectCreatedQueue Payload { get; set; }

        }

        public class CommonProjectUpdatedQueue
        {
            public static string NameExchange = "common.common.project.updated";
            public static string NameInvoiceQueue = "common.invoice.project.updated.queue";
            public static string NameRequireSupportQueue = "common.required-support.project.updated.queue";
            public static string NameIdentityQueue = "common.identity.project.updated.queue";
            public static string NameUtilitiesQueue = "common.utilities.project.updated.queue";
            public DtoCommonProjectUpdatedQueue Payload { get; set; }

        }

        #endregion

        #region "Tower"
        public class CommonTowerCreatedQueue
        {
            public static string NameExchange = "common.common.tower.created";
            public static string NameInvoiceQueue = "common.invoice.tower.created.queue";
            public static string NameRequireSupportQueue = "common.required-support.tower.created.queue";
            public static string NameIdentityQueue = "common.identity.tower.created.queue";
            public static string NameUtilitiesQueue = "common.utilities.tower.created.queue";
            public DtoCommonTowerCreatedQueue Payload { get; set; }

        }

        public class CommonTowerUpdatedQueue
        {
            public static string NameExchange = "common.common.tower.updated";
            public static string NameInvoiceQueue = "common.invoice.tower.updated.queue";
            public static string NameRequireSupportQueue = "common.required-support.tower.updated.queue";
            public static string NameIdentityQueue = "common.identity.tower.updated.queue";
            public static string NameUtilitiesQueue = "common.utilities.tower.updated.queue";
            public DtoCommonTowerUpdatedQueue Payload { get; set; }

        }

        #endregion

        #region "Floor"
        public class CommonFloorCreatedQueue
        {
            public static string NameExchange = "common.common.floor.created";
            public static string NameInvoiceQueue = "common.invoice.floor.created.queue";
            public static string NameRequireSupportQueue = "common.required-support.floor.created.queue";
            public static string NameIdentityQueue = "common.identity.floor.created.queue";
            public static string NameUtilitiesQueue = "common.utilities.floor.created.queue";
            public DtoCommonFloorCreatedQueue Payload { get; set; }

        }

        public class CommonFloorUpdatedQueue
        {
            public static string NameExchange = "common.common.floor.updated";
            public static string NameInvoiceQueue = "common.invoice.floor.updated.queue";
            public static string NameRequireSupportQueue = "common.required-support.floor.updated.queue";
            public static string NameIdentityQueue = "common.identity.floor.updated.queue";
            public static string NameUtilitiesQueue = "common.utilities.floor.updated.queue";
            public DtoCommonFloorUpdatedQueue Payload { get; set; }

        }

        #endregion

        #region "Apartment"
        public class CommonApartmentCreatedQueue
        {
            public static string NameExchange = "common.common.apartment.created";
            public static string NameInvoiceQueue = "common.invoice.apartment.created.queue";
            public static string NameRequireSupportQueue = "common.required-support.apartment.created.queue";
            public static string NameIdentityQueue = "common.identity.apartment.created.queue";
            public static string NameUtilitiesQueue = "common.utilities.apartment.created.queue";
            public DtoCommonApartmentCreatedQueue Payload { get; set; }

        }

        public class CommonApartmentUpdatedQueue
        {
            public static string NameExchange = "common.common.apartment.updated";
            public static string NameInvoiceQueue = "common.invoice.apartment.updated.queue";
            public static string NameRequireSupportQueue = "common.required-support.apartment.updated.queue";
            public static string NameIdentityQueue = "common.identity.apartment.updated.queue";
            public static string NameUtilitiesQueue = "common.utilities.apartment.updated.queue";
            public DtoCommonApartmentUpdatedQueue Payload { get; set; }

        }

        #endregion

        #region "Department"
        public class CommonDepartmentCreatedQueue
        {
            public static string NameExchange = "common.common.department.created";
            public static string NameInvoiceQueue = "common.invoice.department.created.queue";
            public static string NameRequireSupportQueue = "common.required-support.department.created.queue";
            public static string NameIdentityQueue = "common.identity.department.created.queue";
            public static string NameUtilitiesQueue = "common.utilities.department.created.queue";
            public DtoCommonDepartmentCreatedQueue Payload { get; set; }

        }

        public class CommonDepartmentUpdatedQueue
        {
            public static string NameExchange = "common.common.department.updated";
            public static string NameInvoiceQueue = "common.invoice.department.updated.queue";
            public static string NameRequireSupportQueue = "common.required-support.department.updated.queue";
            public static string NameIdentityQueue = "common.identity.department.updated.queue";
            public static string NameUtilitiesQueue = "common.utilities.department.updated.queue";
            public DtoCommonDepartmentUpdatedQueue Payload { get; set; }

        }

        #endregion

        #region "TypeAttribute"
        public class CommonTypeAttributeCreatedQueue
        {
            public static string NameExchange = "common.common.typeAttribute.created";
            public static string NameInvoiceQueue = "common.invoice.typeAttribute.created.queue";
            public static string NameRequireSupportQueue = "common.required-support.typeAttribute.created.queue";
            public static string NameIdentityQueue = "common.identity.typeAttribute.created.queue";
            public static string NameUtilitiesQueue = "common.utilities.typeAttribute.created.queue";
            public DtoCommonTypeAttributeCreatedQueue Payload { get; set; }

        }

        public class CommonTypeAttributeUpdatedQueue
        {
            public static string NameExchange = "common.common.typeAttribute.updated";
            public static string NameIdentityQueue = "common.identity.typeAttribute.updated.queue";
            public static string NameUtilitiesQueue = "common.utilities.typeAttribute.updated.queue";
            public DtoCommonTypeAttributeUpdatedQueue Payload { get; set; }

        }

        #endregion

        #region "TypeAttributeItem"
        public class CommonTypeAttributeItemCreatedQueue
        {
            public static string NameExchange = "common.common.typeAttributeItem.created";
            public static string NameIdentityQueue = "common.identity.typeAttributeItem.created.queue";
            public static string NameUtilitiesQueue = "common.utilities.typeAttributeItem.created.queue";
            public static string NameRequireSupportQueue = "common.required-support.typeAttributeItem.created.queue";
            public DtoCommonTypeAttributeCreatedQueue Payload { get; set; }

        }

        public class CommonTypeAttributeItemUpdatedQueue
        {
            public static string NameExchange = "common.common.typeAttributeItem.updated";
            public static string NameIdentityQueue = "common.identity.typeAttributeItem.updated.queue";
            public static string NameUtilitiesQueue = "common.utilities.typeAttributeItem.updated.queue";
            public static string NameRequireSupportQueue = "common.required-support.typeAttributeItem.updated.queue";
            public DtoCommonTypeAttributeUpdatedQueue Payload { get; set; }

        }

        #endregion

        #region "Department Map"

        public class CommonDepartmentMapQueue
        {
            public static string NameExchange = "common.common.departmentMap";
            public static string NameRequireSupportQueue = "common.required-support.departmentMap.queue";
            public static string NameIdentityQueue = "common.identity.departmentMap.queue";
            public DtoCommonDepartmentMapQueue Payload { get; set; }

        }

        public class CommonDepartmentMapUpdatedQueue
        {
            public static string NameExchange = "common.common.departmentMap.updated";
            public static string NameRequireSupportQueue = "common.required-support.departmentMap.updated.queue";
            public static string NameIdentityQueue = "common.identity.departmentMap.updated.queue";
            public DtoCommonDepartmentMapUpdatedQueue Payload { get; set; }

        }
        #endregion

        #region "Apartment Map"

        public class CommonApartmentMapMapQueue
        {
            public static string NameExchange = "common.common.apartmentMap";
            public static string NameIdentityQueue = "common.identity.apartmentMap.queue";
            public DtoCommonApartmentMapQueue Payload { get; set; }
        }
        #endregion
    }
}
