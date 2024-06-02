using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Frame
{
    public interface IFrameRepo
    {
        public Task<ApiResult<List<FrameVm>>> GetAll();
        public Task<ApiResult<bool>> CreateFrame(CreateFrameRequest request);
        public Task<ApiResult<bool>> UpdateFrame(UpdateFrameRequest request);
        public Task<ApiResult<bool>> DeleteFrame(DeleteFrameRequest request);
        public Task<ApiResult<FrameVm>> GetFrameById(string FrameId);
        public Task<ApiResult<PageResult<FrameVm>>> ViewFrameInPaging(ViewFrameRequest request);
    }
}
