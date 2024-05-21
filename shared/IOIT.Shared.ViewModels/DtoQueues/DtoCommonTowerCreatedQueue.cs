﻿using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoCommonTowerCreatedQueue
    {
        public int TowerId { get; set; }
        public int? OneSid { get; set; }
        public int? ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PersonContact { get; set; }
        public string PhoneContact { get; set; }
        public string Note { get; set; }
        public string LatLong { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UserId { get; set; }
        public EntityStatus? Status { get; set; }
        public string ApiCarParking { get; set; }
        public string UserNameCarParking { get; set; }
        public string PasswordCarParking { get; set; }
    }
}
