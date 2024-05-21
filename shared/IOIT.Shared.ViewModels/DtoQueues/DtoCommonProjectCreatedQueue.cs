using System;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Shared.ViewModels.DtoQueues
{
    public class DtoCommonProjectCreatedQueue
    {
        public int ProjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string PersonContact { get; set; }
        public string PhoneContact { get; set; }
        public string Note { get; set; }
        public string Image { get; set; }
        public string LatLong { get; set; }
        public string CarPakingRegulations { get; set; }
        public string EmailUserName { get; set; }
        public string EmailPasswordHash { get; set; }
        public string EmailDisplayName { get; set; }
        public string EmailHost { get; set; }
        public int? EmailPort { get; set; }
        public string ApiOneS { get; set; }
        public string UserNameOneS { get; set; }
        public string PasswordOneS { get; set; }
        public string ApiCarParking { get; set; }
        public string UserNameCarParking { get; set; }
        public string PasswordCarParking { get; set; }
        public int? OneSId { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public virtual EntityStatus Status { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public long? UserId { get; set; }
        public bool IsPublic { get; set; }
    }
}
