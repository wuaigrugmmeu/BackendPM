using BackendPM.API.Configurations;
using BackendPM.API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // 添加全局异常过滤器
    options.Filters.Add<GlobalExceptionFilter>();
    // 添加请求响应日志过滤器
    options.Filters.Add<RequestResponseLogFilter>();
    // 添加API性能过滤器
    options.Filters.Add<ApiPerformanceFilter>();
});

// 配置 Swagger
SwaggerConfiguration.ConfigureServices(builder.Services);

// 配置 CORS
CorsConfiguration.ConfigureServices(builder.Services, builder.Configuration);

// 配置身份验证
AuthenticationConfiguration.ConfigureServices(builder.Services, builder.Configuration);

// 配置依赖注入
DependencyInjectionConfiguration.ConfigureServices(builder.Services, builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 使用 SwaggerConfiguration 中定义的方法配置 Swagger
    app.UseSwaggerServices();
    app.UseDeveloperExceptionPage();
}
else 
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// 使用 CORS - 必须在其他中间件之前调用
app.UseCors(CorsConfiguration.DefaultCorsPolicyName);

app.UseHttpsRedirection();

// 使用静态文件
app.UseStaticFiles();

// 使用路由
app.UseRouting();

// 使用身份验证和授权
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
