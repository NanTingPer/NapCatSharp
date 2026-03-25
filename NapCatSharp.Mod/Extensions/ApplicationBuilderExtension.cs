namespace NapCatSharp.Mod.Extensions;

public static class ApplicationBuilderExtension
{
    public static WebApplicationBuilder InitializationMod(this WebApplicationBuilder builder)
    {
        // 1. 加载Mod并实例化
        // 2. 将List<Mod>作为单例加入到Services中
        // 3. 将ModManager作为单例加入到Services中
        // 4. ModManager需要List<Mod>作为参数传入
        // 5. 将NapCatHttpServer作为单例加入到Services中 配置使用json
        //    - 或许应该在Web中配置

        // 6. 创建后台服务
        //    - ctor需要使用ModManager
        //    - 创建NapCatSocket 订阅ModManager中的方法
        return builder;
    }
}
