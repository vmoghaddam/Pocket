using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPAAPI.ViewModels
{
    public class JobGroup
    {
        public int Id { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string FullCode { get; set; }
        public string Remark { get; set; }
        public string Parent { get; set; }
        public string ParentCode { get; set; }
        public string TitleFormated { get; set; }
        public int CustomerId { get; set; }
        public static void Fill(Models.JobGroup entity, ViewModels.JobGroup jobgroup)
        {
            entity.Id = jobgroup.Id;
            entity.ParentId = jobgroup.ParentId;
            entity.Title = jobgroup.Title;
            entity.Code = jobgroup.Code;
            entity.FullCode = jobgroup.FullCode;
            entity.Remark = jobgroup.Remark;
            entity.CustomerId = jobgroup.CustomerId;
        }
        public static void FillDto(Models.JobGroup entity, ViewModels.JobGroup jobgroup)
        {
            jobgroup.Id = entity.Id;
            jobgroup.ParentId = entity.ParentId;
            jobgroup.Title = entity.Title;
            jobgroup.Code = entity.Code;
            jobgroup.FullCode = entity.FullCode;
            jobgroup.Remark = entity.Remark;
            jobgroup.CustomerId = entity.CustomerId;
        }
    }
}