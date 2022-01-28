using System.Net.Http.Json;
using AntDesign;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using AppName.DataCenter.Client.Helpers;
using AppName.DataCenter.Shared.Requests;
using AppName.DataCenter.Shared.ViewModels;

namespace AppName.DataCenter.Client.Pages.ModName
{
    public partial class ModNameDetail
    {
        [Parameter]
        public int Id { get; set; }

        private readonly TypographyCopyableConfig _copyableConfig = new();

        private IForm _form;
        private ModNameDetailViewModel _detailData = new();

        private bool _showEmptyContent = true;
        private bool _interacting = false;
        private bool _readonly = false;

        protected override async Task OnInitializedAsync()
        {
            _copyableConfig.OnCopy = async () => await MsgService.Success("已复制到剪贴板", 1d);
            await LoadModNameDetailAsync();
            _readonly = true;
            _showEmptyContent = false;
        }

        private async Task UpdateModNameAsync(EditContext editContext)
        {
            if (Id < 1)
            {
                await MsgService.Error("ID不存在");
                return;
            }
            _interacting = true;
            var pairs = new KeyValuePair<string, string>[]
            {
                new(nameof(ModNameUpdateRequest.Name), _detailData.Name?.Trim()),
            };
            ModNameDetailViewModel result;
            try
            {
                result = await DataService.UpdateModNameAsync(Id, pairs);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //await MsgService.Error("请求失败：" + ex.Message);
                _interacting = false;
                return;
            }
            switch (result.Id)
            {
                case -1:
                    {
                        await MsgService.Error("ModName不存在或已删除", 1.5d);
                    }
                    break;

                case 0:
                    {
                        await MsgService.Info("服务端未检测到信息变动", 1.5d);
                    }
                    break;

                default:
                    {
                        _detailData = result;
                        await MsgService.Success("用户信息已更新", 1.5d);
                    }
                    break;
            }
            _readonly = true;
            _interacting = false;
        }

        private void RecoverModNameInfo()
        {
            _form.Reset();
            _readonly = true;
        }

        private async Task DeleteCurrentModNameAsync()
        {
            _interacting = true;
            bool ok = await DataService.DeleteModNameAsync(Id);
            if (ok)
            {
                await MsgService.Success("删除成功，即将跳转到列表页...", 2);
                NavigationManager.NavigateTo("ModName/list");
            }
            else
            {
                await MsgService.Error("删除失败!");
                _interacting = false;
            }
        }

        private async Task LoadModNameDetailAsync()
        {
            if (Id < 1)
            {
                return;
            }
            _detailData = await DataService.GetModNameDetailAsync(Id);
        }

        //private void Navi2ChildPage(string path)
        //{
        //    NavigationManager.NavigateTo($"ModName/{Id}/{path}");
        //}
    }
}