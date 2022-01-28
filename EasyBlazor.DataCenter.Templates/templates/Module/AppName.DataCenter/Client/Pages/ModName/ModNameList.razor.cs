using System.Net.Http.Json;
using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using AppName.DataCenter.Client.Extensions;
using AppName.DataCenter.Client.Helpers;
using AppName.DataCenter.Client.Models;
using AppName.DataCenter.Shared.Requests;
using AppName.DataCenter.Shared.ViewModels;

namespace AppName.DataCenter.Client.Pages.ModName
{
    public partial class ModNameList
    {
        private readonly ModNameListSearchFormData _formData = new()
        {
            Index = 1,
            Size = 20,
        };

        private bool _isInited = false;
        private bool _searching = false;
        private bool _showMoreConditions = false;

        private int _total = 0;
        private ModNameListItem[] _listItems;

        private Form<ModNameListSearchFormData> _searchForm;

        private readonly TypographyCopyableConfig _copyableConfig = new();

        protected override void OnInitialized()
        {
            _copyableConfig.OnCopy = async () => await MsgService.Success("已复制到剪贴板", 1d);
            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                _isInited = true;
            }
        }

        private async Task OnFinish(EditContext editContext)
        {
            if (_formData.Index != 1)
            {
                _formData.Index = 1;
            }
            else
            {
                var result = await LoadModNameListAsync();
                if (!result)
                {
                    await MsgService.Error("数据获取失败！");
                    return;
                }
            }
        }

        private void Reset(MouseEventArgs args)
        {
            _searchForm.Reset();
            _formData.PatchReset();
        }

        private void Collapse(MouseEventArgs args)
        {
            _showMoreConditions = !_showMoreConditions;
        }

        private async Task OnTableChanged(QueryModel<ModNameListItem> query)
        {
            if (!_isInited)
            {
                return;
            }
            _formData.Size = query.PageSize;
            if (_formData.Id > 0)
            {
                _formData.Index = 1;
            }
            else
            {
                //var (orderBy, isDesc) = query.SingleSortOrder();
                //_formData.SortType = orderBy;
                //_formData.Desc = isDesc;
                _formData.Index = query.PageIndex;
            }
            var result = await LoadModNameListAsync();
            if (!result)
            {
                await MsgService.Error("数据获取失败！");
                return;
            }
        }

        private async Task<bool> LoadModNameListAsync()
        {
            _searching = true;
            var pairs = new KeyValuePair<string, string>[]
            {
                new(nameof(ModNamePageListRequest.Id), _formData.Id?.ToString()),
                new(nameof(ModNamePageListRequest.CreateTimeMin), DateTimeHelper.Convert2TimestampOfDate(_formData.CreateTimeRange[0])),
                new(nameof(ModNamePageListRequest.CreateTimeMax), DateTimeHelper.Convert2TimestampOfDate(_formData.CreateTimeRange[1])),
                new(nameof(ModNamePageListRequest.Index), _formData.Index.ToString()),
                new(nameof(ModNamePageListRequest.Size), _formData.Size.ToString()),
                //new(nameof(ModNamePageListRequest.SortType), _formData.SortType),
                //new(nameof(ModNamePageListRequest.Desc), _formData.Desc.ToString()),
            };
            PageListViewModel<ModNameListItem> rsp;
            try
            {
                rsp = await DataService.GetModNamePageListAsync(pairs);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //await MsgService.Error("请求失败：" + ex.Message);
                _searching = false;
                return false;
            }
            if (rsp is null)
            {
                _listItems = Array.Empty<ModNameListItem>();
                _total = 0;
            }
            else
            {
                _listItems = rsp.Items ?? Array.Empty<ModNameListItem>();
                _total = rsp.Total;
            }
            _searching = false;
            return true;
        }
    }
}