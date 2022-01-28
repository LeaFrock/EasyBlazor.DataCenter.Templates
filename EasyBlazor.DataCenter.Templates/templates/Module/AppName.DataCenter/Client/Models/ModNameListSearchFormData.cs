using System.ComponentModel.DataAnnotations;
using AppName.DataCenter.Shared.Requests;

namespace AppName.DataCenter.Client.Models
{
    internal class ModNameListSearchFormData : ModNamePageListRequest
    {
        public DateTime?[] CreateTimeRange { get; set; } = new DateTime?[2];

        public void PatchReset()
        {
            CreateTimeRange[0] = null;
            CreateTimeRange[1] = null;
        }
    }
}