var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.NapCatSharp_Mod>("napcatsharp-mod");

builder.Build().Run();
