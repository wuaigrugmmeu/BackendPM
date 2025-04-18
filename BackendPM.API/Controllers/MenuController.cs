using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using BackendPM.Application.DTOs;
using BackendPM.Application.Services;

namespace BackendPM.API.Controllers
{
    /// <summary>
    /// 菜单控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenuController : BaseController
    {
        private readonly IMenuAppService _menuAppService;
        
        public MenuController(IMenuAppService menuAppService)
        {
            _menuAppService = menuAppService;
        }
        
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="keywords">搜索关键字</param>
        /// <returns>菜单列表</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMenuList([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string keywords = null)
        {
            var (data, total) = await _menuAppService.GetMenuListAsync(pageIndex, pageSize, keywords);
            return SuccessWithPagination(data, total, pageIndex, pageSize);
        }
        
        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns>菜单列表</returns>
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMenus()
        {
            var menus = await _menuAppService.GetAllMenusAsync();
            return Success(menus);
        }
        
        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>菜单详情</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMenuDetail(Guid id)
        {
            var menu = await _menuAppService.GetMenuDetailAsync(id);
            return Success(menu);
        }
        
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="createMenuDto">菜单信息</param>
        /// <returns>创建的菜单</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMenu([FromBody] CreateMenuDto createMenuDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var menu = await _menuAppService.CreateMenuAsync(createMenuDto);
            return Success(menu);
        }
        
        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <param name="updateMenuDto">更新的菜单信息</param>
        /// <returns>更新后的菜单</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMenu(Guid id, [FromBody] UpdateMenuDto updateMenuDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var menu = await _menuAppService.UpdateMenuAsync(id, updateMenuDto);
            return Success(menu);
        }
        
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>操作结果</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMenu(Guid id)
        {
            var result = await _menuAppService.DeleteMenuAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 启用菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>操作结果</returns>
        [HttpPatch("{id}/enable")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnableMenu(Guid id)
        {
            var result = await _menuAppService.EnableMenuAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 禁用菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>操作结果</returns>
        [HttpPatch("{id}/disable")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DisableMenu(Guid id)
        {
            var result = await _menuAppService.DisableMenuAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <param name="includeDisabled">是否包含禁用菜单</param>
        /// <returns>菜单树</returns>
        [HttpGet("tree")]
        public async Task<IActionResult> GetMenuTree([FromQuery] bool includeDisabled = false)
        {
            var tree = await _menuAppService.GetMenuTreeAsync(includeDisabled);
            return Success(tree);
        }
        
        /// <summary>
        /// 获取当前用户可访问的菜单树
        /// </summary>
        /// <returns>菜单树</returns>
        [HttpGet("user-menu-tree")]
        [Authorize]
        public async Task<IActionResult> GetUserMenuTree()
        {
            var userId = CurrentUserId;
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            var tree = await _menuAppService.GetUserMenuTreeAsync(userId);
            return Success(tree);
        }
        
        /// <summary>
        /// 分配权限给菜单
        /// </summary>
        /// <param name="menuAssignPermissionsDto">菜单权限分配信息</param>
        /// <returns>操作结果</returns>
        [HttpPost("assign-permissions")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignPermissions([FromBody] MenuAssignPermissionsDto menuAssignPermissionsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _menuAppService.AssignPermissionsAsync(menuAssignPermissionsDto);
            return Success(result);
        }
        
        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>权限列表</returns>
        [HttpGet("{id}/permissions")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMenuPermissions(Guid id)
        {
            var permissions = await _menuAppService.GetMenuPermissionsAsync(id);
            return Success(permissions);
        }
        
        /// <summary>
        /// 设置菜单顺序
        /// </summary>
        /// <param name="setMenuOrderDto">菜单顺序信息</param>
        /// <returns>操作结果</returns>
        [HttpPost("set-order")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetMenuOrder([FromBody] SetMenuOrderDto setMenuOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _menuAppService.SetMenuOrderAsync(setMenuOrderDto);
            return Success(result);
        }
        
        /// <summary>
        /// 设置菜单父级
        /// </summary>
        /// <param name="setMenuParentDto">菜单父级信息</param>
        /// <returns>更新后的菜单</returns>
        [HttpPost("set-parent")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetMenuParent([FromBody] SetMenuParentDto setMenuParentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var menu = await _menuAppService.SetMenuParentAsync(setMenuParentDto);
            return Success(menu);
        }
        
        /// <summary>
        /// 检查菜单编码是否存在
        /// </summary>
        /// <param name="code">菜单编码</param>
        /// <returns>是否存在</returns>
        [HttpGet("check-code")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CheckMenuCodeExists([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("菜单编码不能为空");
            }
            
            var exists = await _menuAppService.CheckMenuCodeExistsAsync(code);
            return Success(exists);
        }
    }
}